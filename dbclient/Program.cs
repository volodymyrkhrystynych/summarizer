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
                await dclient.CreatingTable_async(Requests.Summaries());

                // upload data
                Console.WriteLine("done");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
