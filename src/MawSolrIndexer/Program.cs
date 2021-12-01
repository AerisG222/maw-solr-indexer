using System;
using System.Threading.Tasks;
using MawSolrIndexer.Database;

namespace MawSolrIndexer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if(args.Length != 2) {
                ShowUsage();
                Environment.Exit(1);
            }

            var photoDb = new PhotoDatabase(args[0]);
            var videoDb = new VideoDatabase(args[0]);
            var repo = new Repository(photoDb, videoDb);
            var uploader = new SolrUploader(args[1]);

            try
            {
                Console.WriteLine("Querying Categories...");
                var categories = await repo.GetCategoriesAsync();

                Console.WriteLine("Clearing Index...");
                await uploader.ClearIndex();

                Console.WriteLine("Uploading Full Index...");
                await uploader.UploadFullIndex(categories);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error encountered: {ex.Message}");
                Environment.Exit(2);
            }

            Console.WriteLine("Index Load completed successfully!");
        }

        static void ShowUsage()
        {
            Console.WriteLine("A small utility to load photo and video categories to the maw solr index.");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("    MawSolrIndex <db_conn_str> <solr_url>");
            Console.WriteLine();
        }
    }
}
