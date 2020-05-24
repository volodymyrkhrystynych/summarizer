using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dbclient
{
    class DClient
    {
        IAmazonDynamoDB client;

        /*-----------------------------------------------------------------------------------
          *  If you are creating a client for the DynamoDB service, make sure your credentials
          *  are set up first, as explained in:
          *  https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/SettingUp.DynamoWebService.html,
          *
          *  If you are creating a client for DynamoDBLocal (for testing purposes),
          *  DynamoDB-Local should be started first. For most simple testing, you can keep
          *  data in memory only, without writing anything to disk.  To do this, use the
          *  following command line:
          *
          *    java -Djava.library.path=./DynamoDBLocal_lib -jar DynamoDBLocal.jar -inMemory
          *
          *  For information about DynamoDBLocal, see:
          *  https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html.
          *-----------------------------------------------------------------------------------*/
        /*--------------------------------------------------------------------------
         *          createClient
         *--------------------------------------------------------------------------*/
        public void createClient(bool useDynamoDBLocal)
        {
            if (useDynamoDBLocal)
            {
                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                bool localFound = false;
                try
                {
                    using (var tcp_client = new TcpClient())
                    {
                        var result = tcp_client.BeginConnect("localhost", 8000, null, null);
                        localFound = result.AsyncWaitHandle.WaitOne(3000); // Wait 3 seconds
                        tcp_client.EndConnect(result);
                    }
                }
                catch
                {
                    localFound = false;
                }
                if (!localFound)
                {
                    throw new Exception("DynamoDB Local does not appear to have been started..., (checked port 8000)");
                }

                // If DynamoDB-Local does seem to be running, so create a client
                Console.WriteLine("  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)");
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = "http://localhost:8000";
                // todo use file
                var credentials = new Amazon.Runtime.BasicAWSCredentials("fakeAccessKeyId", "fakeAWSSecretAccessKey");
                client = new AmazonDynamoDBClient(credentials, ddbConfig);
            }
            else
            {
                client = new AmazonDynamoDBClient();
            }
        }

        /*--------------------------------------------------------------------------
         *                       CreatingTable_async
         *--------------------------------------------------------------------------*/
        public async Task CreatingTable_async(CreateTableRequest request)
        {
            Task<bool> newTbl = CreateNewTable_async(request);
            await newTbl;
        }

        /*--------------------------------------------------------------------------
         *                      checkingTableExistence_async
         *--------------------------------------------------------------------------*/
        public async Task<bool> checkingTableExistence_async(string tblNm)
        {
            DescribeTableResponse descResponse;

            ListTablesResponse tblResponse = await client.ListTablesAsync();
            if (tblResponse.TableNames.Contains(tblNm))
            {
                Console.WriteLine("     A table named {0} already exists in DynamoDB!", tblNm);

                // If the table exists, get its description

                try
                {
                    descResponse = await client.DescribeTableAsync(tblNm);
                }
                catch (Exception ex)
                {
                    throw new Exception($"     However, its description is not available ({ex.Message})");
                }

                Console.WriteLine("     Status of the table: '{0}'.", descResponse.Table.TableStatus);
                return (true);
            }
            return (false);
        }

        /*--------------------------------------------------------------------------
         *                CreateNewTable_async
         *--------------------------------------------------------------------------*/
        public async Task<bool> CreateNewTable_async(CreateTableRequest request)
        {
            CreateTableResponse response;
            try
            {
                Task<CreateTableResponse> makeTbl = client.CreateTableAsync(request);
                response = await makeTbl;
                Console.WriteLine("     -- Created the \"{0}\" table successfully!", request.TableName);
            }
            catch (Exception ex)
            {
                throw new Exception($"     FAILED to create the new table, because: {ex.Message}.");
            }

            // Report the status of the new table...
            Console.WriteLine("     Status of the new table: '{0}'.", response.TableDescription.TableStatus);
            return (true);
        }


        /*--------------------------------------------------------------------------
        *     LoadingData_async
        *--------------------------------------------------------------------------*/
        public Table LoadTable(string tableName)
        {
            return Table.LoadTable(client, tableName);
        }

        /*--------------------------------------------------------------------------
        *                             ReadJsonFile_async
        *--------------------------------------------------------------------------*/
        public async Task<JArray> ReadJsonFile_async(string JsonFilePath)
        {
            try
            {
                using (var sr = new StreamReader(JsonFilePath))
                using (var jtr = new JsonTextReader(sr))
                {
                    var obj = await JToken.ReadFromAsync(jtr);
                    return (JArray)obj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR: could not read the file!\n          Reason: {ex.ToString()}.");
            }
        }

        /*--------------------------------------------------------------------------
         *                LoadJsonData_async
         *--------------------------------------------------------------------------*/
        public async Task LoadJsonData_async(
                        Table table,
                        JArray array)
        {
            var n = Math.Max(array.Count, 99);
            Console.WriteLine("     -- Starting to load {0:#,##0} records into the {1} table ...", array.Count, table.TableName);

            for (int i = 0; i < array.Count; i++)
            {
                try
                {
                    string itemJson = array[i].ToString();
                    var doc = Document.FromJson(itemJson);
                    Task putItem = table.PutItemAsync(doc);
                    await putItem;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Could not write the record #{i:#,##0}, because:\n       {ex.ToString()}");
                }
            }
            Console.WriteLine("\n     -- Finished writing all records to DynamoDB!");
        }

        /*--------------------------------------------------------------------------
         *                             ReadingTable_async
         *--------------------------------------------------------------------------*/
        public async Task<bool> ReadingTable_async(Table table, string datePub, string title, bool report)
        {
            // Create Primitives for the HASH and RANGE portions of the primary key
            var hash  = new Primitive(datePub, false);
            var range = new Primitive(title, false);

            try
            {
                var token = default(System.Threading.CancellationToken);
                Task<Document> docLoaded = table.GetItemAsync(hash, range, token);
                if (report)
                    Console.WriteLine("  -- Reading the {0} summary \"{1}\" from the Summaries table...", datePub, title);
                var record = await docLoaded;
                if (record == null)
                {
                    if (report)
                        Console.WriteLine("     -- Sorry, that summary isn't in the Summaries table.");
                    return (false);
                }
                else
                {
                    if (report)
                        Console.WriteLine("     -- Found it!  The summary record looks like this:\n" +
                                            record.ToJsonPretty());
                    return (true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("     FAILED to get the summary, because: {0}.", ex.Message);
            }
            return (false);
        }

        /*--------------------------------------------------------------------------
         *                DeletingTable_async
         *--------------------------------------------------------------------------*/
        public async Task<bool> DeletingTable_async(string tableName)
        {
            Console.WriteLine("  -- Trying to delete the table named \"{0}\"...", tableName);
            Task tblDelete = client.DeleteTableAsync(tableName);
            try
            {
                await tblDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("     ERROR: Failed to delete the table, because:\n            " + ex.Message);
                return (false);
            }
            Console.WriteLine("     -- Successfully deleted the table!");
            return (true);
        }

        /*--------------------------------------------------------------------------
         *                             SearchListing_async
         *--------------------------------------------------------------------------*/
        public async Task<bool> SearchListing_async(Search search)
        {
            int i = 0;
            List<Document> docList = new List<Document>();

            Console.WriteLine("         Here are the record retrieved:\n" +
                               "         --------------------------------------------------------------------------");
            Task<List<Document>> getNextBatch;
            do
            {
                try
                {
                    getNextBatch = search.GetNextSetAsync();
                    docList = await getNextBatch;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("        FAILED to get the next batch of records from Search! Reason:\n          " +
                                       ex.Message);
                    return (false);
                }

                foreach (Document doc in docList)
                {
                    i++;
                    // print doc
                    Console.WriteLine(doc.ToJsonPretty());
                }

                if (i >= search.Limit)
                    break;
                    
            } while (!search.IsDone);

            Console.WriteLine("     -- Retrieved {0} records.", i);
            return (true);
        }

        /*--------------------------------------------------------------------------
         *                             ClientQuerying_async
         *--------------------------------------------------------------------------*/
        public async Task<bool> ClientQuerying_async(QueryRequest qRequest)
        {
            QueryResponse qResponse;
            try
            {
                Task<QueryResponse> clientQueryTask = client.QueryAsync(qRequest);
                qResponse = await clientQueryTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("      The low-level query FAILED, because:\n       {0}.", ex.Message);
                return (false);
            }
            Console.WriteLine("     -- The low-level query succeeded, and returned {0} records!", qResponse.Items.Count);
            Console.WriteLine("         Here are the records retrieved:" +
                               "         --------------------------------------------------------------------------");
            foreach (Dictionary<string, AttributeValue> items in qResponse.Items)
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            return (true);
        }
    }
}