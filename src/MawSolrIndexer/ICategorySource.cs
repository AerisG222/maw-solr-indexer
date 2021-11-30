using System.Collections.Generic;
using System.Threading.Tasks;

namespace MawSolrIndexer
{
    public interface ICategorySource
    {
        Task<IEnumerable<MultimediaCategory>> GetCategoriesAsync();
    }
}