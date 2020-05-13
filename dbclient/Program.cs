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
                await dclient.CreatingTable_async(Requests.Summaries());

                // upload data
                Console.WriteLine("  -- Reading the data from a JSON file...");
                var movieArray = await dclient.ReadJsonFile_async("../.demo/summaries.json");
                if (movieArray != null){
                    var table = dclient.LoadTable(summaries.TableName);
                    await dclient.LoadJsonData_async(table, movieArray);
                }

                Console.WriteLine("done");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
