using System;
using System.Linq;
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
            }

            var photoDb = new PhotoDatabase(args[0]);
            var videoDb = new VideoDatabase(args[0]);
            var repo = new Repository(photoDb, videoDb);

            var categories = await repo.GetCategoriesAsync();

            Console.WriteLine(categories.Count());
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
