using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MawSolrIndexer.Database {
    public class PhotoDatabase
        : Database, ICategorySource
    {
        public PhotoDatabase(string connString)
            : base(connString)
        {

        }

        public async Task<IEnumerable<MultimediaCategory>> GetCategoriesAsync()
        {
            var categories = await GetSourceCategoriesAsync();
            var multimediaCategories = categories.Select(x => x.ToMultimediaCategory());

            foreach(var multimediaCategory in multimediaCategories)
            {
                var tasks = new List<Task>
                {
                    EnrichWithCommentsAsync(multimediaCategory),
                    EnrichWithMakeAndModelAsync(multimediaCategory),
                    EnrichWithLocationAsync(multimediaCategory),
                    EnrichWithPointsOfInterestAsync(multimediaCategory)
                };

                await Task.WhenAll(tasks.ToArray());
            }

            return multimediaCategories;
        }

        async Task<IEnumerable<Category>> GetSourceCategoriesAsync()
        {
            var sql = "SELECT 'photo' AS type, "
                    + "       CONCAT('photo_', CAST(id AS TEXT)) AS solr_id, "
                    + "       id, "
                    + "       year, "
                    + "       is_private, "
                    + "       name, "
                    + "       teaser_photo_width, "
                    + "       teaser_photo_height, "
                    + "       teaser_photo_path, "
                    + "       teaser_photo_sq_height, "
                    + "       teaser_photo_sq_width, "
                    + "       teaser_photo_sq_path, "
                    + "       gps_latitude, "
                    + "       gps_longitude "
                    + "  FROM photo.category";

            return await GetSourceCategoriesAsync(sql);
        }

        Task EnrichWithCommentsAsync(MultimediaCategory category)
        {
            var sql = $"SELECT c.entry_date, "
                    + $"       c.message "
                    + $"  FROM photo.photo p "
                    + $"  LEFT OUTER JOIN photo.comment c ON p.id = c.photo_id "
                    + $" WHERE c.message IS NOT NULL "
                    + $"   AND CONCAT('photo_', CAST(p.category_id AS TEXT)) = '${category.Id}'";

            return EnrichWithCommentsAsync(category, sql);
        }

        Task EnrichWithMakeAndModelAsync(MultimediaCategory category)
        {
            var sql = $"SELECT DISTINCT "
                    + $"       mk.name AS make, "
                    + $"       mdl.name AS model "
                    + $"  FROM photo.photo p "
                    + $"  LEFT OUTER JOIN photo.make mk ON mk.id = p.make_id "
                    + $"  LEFT OUTER JOIN photo.model mdl ON mdl.id = p.model_id "
                    + $" WHERE CONCAT('photo_', CAST(p.category_id AS TEXT)) = '${category.Id}' "
                    + $"   AND ( "
                    + $"           p.make_id IS NOT NULL "
                    + $"           OR "
                    + $"           p.model_id IS NOT NULL "
                    + $"       )";

            return EnrichWithMakeAndModelAsync(category, sql);
        }

        Task EnrichWithLocationAsync(MultimediaCategory category)
        {
            var sql = $"SELECT DISTINCT "
                    + $"       rg.administrative_area_level_1, "
                    + $"       rg.administrative_area_level_2, "
                    + $"       rg.administrative_area_level_3, "
                    + $"       rg.country, "
                    + $"       rg.formatted_address, "
                    + $"       rg.locality, "
                    + $"       rg.neighborhood, "
                    + $"       rg.postal_code, "
                    + $"       rg.postal_code_suffix, "
                    + $"       rg.premise, "
                    + $"       rg.route, "
                    + $"       rg.street_number, "
                    + $"       rg.sub_locality_level_1, "
                    + $"       rg.sub_locality_level_2, "
                    + $"       rg.sub_premise "
                    + $"  FROM photo.photo p "
                    + $"  LEFT OUTER JOIN photo.reverse_geocode rg ON rg.photo_id = p.id "
                    + $" WHERE CONCAT('photo_', CAST(p.category_id AS TEXT)) = '${category.Id}' "
                    + $"   AND rg.photo_id IS NOT NULL";

            return EnrichWithLocationAsync(category, sql);
        }

        Task EnrichWithPointsOfInterestAsync(MultimediaCategory category)
        {
            var sql = $"SELECT DISTINCT "
                    + $"       poi.poi_name, "
                    + $"       poi.poi_type "
                    + $"  FROM photo.photo p "
                    + $"  LEFT OUTER JOIN photo.point_of_interest poi ON poi.photo_id = p.id "
                    + $" WHERE CONCAT('photo_', CAST(p.category_id AS TEXT)) = '${category.Id}' "
                    + $"   AND poi.photo_id IS NOT NULL";

            return EnrichWithPointsOfInterestAsync(category, sql);
        }
    }
}