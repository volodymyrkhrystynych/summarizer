using System;
using System.Threading.Tasks;

namespace dbclient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try{
                // connect
                var dclient = new DClient();

                dclient.createClient(true);

                // create table
                var summaries = Requests.Summaries();

                // reset table
                // await dclient.DeletingTable_async(summaries.TableName);

                // create table
                if (!await dclient.checkingTableExistence_async(summaries.TableName))
                {
                    Console.WriteLine("  -- Creating a new table named {0}...", summaries.TableName);
                    await dclient.CreatingTable_async(Requests.Summaries());
                }

                var table = dclient.LoadTable(summaries.TableName);

                // upload data
                Console.WriteLine("  -- Reading the data from a JSON file...");
                var array = await dclient.ReadJsonFile_async("../.demo/summaries.json");
                if (array != null){
                    await dclient.LoadJsonData_async(table, array);
                }

                // exact match
                //await dclient.ReadingTable_async(table, "5-Apr-20", "SpaceX unveils users' guide for giant Starship rocket", true);

                var query = table.Query(Requests.pubDate("5-Apr-20"));
                await dclient.SearchListing_async(query);

                Console.WriteLine("done");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
