using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MawSolrIndexer
{
    public class Repository
    {
        ICategorySource[] _sources;

        public Repository(params ICategorySource[] sources)
        {
            if(sources == null || sources.Length == 0)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            _sources = sources;
        }

        public async Task<IEnumerable<MultimediaCategory>> GetCategoriesAsync() {
            var results = new List<MultimediaCategory>();

            foreach(var source in _sources)
            {
                results.AddRange(await source.GetCategoriesAsync());
            }

            return results;
        }
    }
}