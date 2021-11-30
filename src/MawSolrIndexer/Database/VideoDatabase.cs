using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MawSolrIndexer.Database {
    public class VideoDatabase
        : Database, ICategorySource
    {
        public VideoDatabase(string connString)
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
                    EnrichWithLocationAsync(multimediaCategory),
                    EnrichWithPointsOfInterestAsync(multimediaCategory)
                };

                await Task.WhenAll(tasks.ToArray());
            }

            return multimediaCategories;
        }

        async Task<IEnumerable<Category>> GetSourceCategoriesAsync()
        {
            var sql = "SELECT 'video' AS type, "
                    + "       CONCAT('video_', CAST(id AS TEXT)) AS solr_id, "
                    + "       id, "
                    + "       year, "
                    + "       is_private, "
                    + "       name, "
                    + "       teaser_image_width AS teaser_photo_width, "
                    + "       teaser_image_height AS teaser_photo_height, "
                    + "       teaser_image_path AS teaser_photo_path, "
                    + "       teaser_image_sq_height AS teaser_photo_sq_height, "
                    + "       teaser_image_sq_width AS teaser_photo_sq_width, "
                    + "       teaser_image_sq_path AS teaser_photo_sq_path, "
                    + "       gps_latitude, "
                    + "       gps_longitude "
                    + "  FROM video.category";

            return await GetSourceCategoriesAsync(sql);
        }

        Task EnrichWithCommentsAsync(MultimediaCategory category)
        {
            var sql = $"SELECT c.entry_date, "
                    + $"       c.message "
                    + $"  FROM video.video v "
                    + $"  LEFT OUTER JOIN video.comment c ON v.id = c.video_id "
                    + $" WHERE c.message IS NOT NULL "
                    + $"   AND CONCAT('video_', CAST(v.category_id AS TEXT)) = '${category.Id}'";

            return EnrichWithCommentsAsync(category, sql);
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
                    + $"  FROM video.video v "
                    + $"  LEFT OUTER JOIN video.reverse_geocode rg ON rg.video_id = v.id "
                    + $" WHERE CONCAT('video_', CAST(v.category_id AS TEXT)) = '${category.Id}' "
                    + $"   AND rg.video_id IS NOT NULL";

            return EnrichWithLocationAsync(category, sql);
        }

        Task EnrichWithPointsOfInterestAsync(MultimediaCategory category)
        {
            var sql = $"SELECT DISTINCT "
                    + $"       poi.poi_name, "
                    + $"       poi.poi_type "
                    + $"  FROM video.video v "
                    + $"  LEFT OUTER JOIN video.point_of_interest poi ON poi.video_id = v.id "
                    + $" WHERE CONCAT('video_', CAST(v.category_id AS TEXT)) = '${category.Id}' "
                    + $"   AND poi.video_id IS NOT NULL";

            return EnrichWithPointsOfInterestAsync(category, sql);
        }
    }
}