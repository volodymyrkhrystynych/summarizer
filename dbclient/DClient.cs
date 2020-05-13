using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace dbclient
{
    class DClient
    {
        AmazonDynamoDBClient client;

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
            Console.WriteLine("  -- Creating a new table named {0}...", request.TableName);
            if (await checkingTableExistence_async(request.TableName))
            {
                Console.WriteLine("     -- No need to create a new table...");
                return;
            }

            Task<bool> newTbl = CreateNewTable_async(request);
            await newTbl;
        }

        /*--------------------------------------------------------------------------
         *                      checkingTableExistence_async
         *--------------------------------------------------------------------------*/
        async Task<bool> checkingTableExistence_async(string tblNm)
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
    }
}