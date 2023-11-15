using System.Collections.Generic;
using System.Threading.Tasks;

namespace MawSolrIndexer.Database;

public class VideoDatabase
    : Database, ICategorySource
{
    public VideoDatabase(string connString)
        : base(connString)
    {

    }

    public async Task<IEnumerable<MultimediaCategory>> GetCategoriesAsync()
    {
        var sql =
@"
SELECT cat.type,
       cat.solr_id,
       cat.id,
       cat.year,
       cat.allowed_roles,
       cat.name,
       cat.teaser_image_width AS teaser_photo_width,
       cat.teaser_image_height AS teaser_photo_height,
       cat.teaser_image_path AS teaser_photo_path,
       cat.teaser_image_sq_height AS teaser_photo_sq_height,
       cat.teaser_image_sq_width AS teaser_photo_sq_width,
       cat.teaser_image_sq_path AS teaser_photo_sq_path,
       cat.gps_latitude,
       cat.gps_longitude,
       -- comments
       ARRAY_AGG(DISTINCT cmt.entry_date) FILTER (WHERE cmt.entry_date IS NOT NULL) AS comment_entry_dates,
       ARRAY_AGG(DISTINCT cmt.message) FILTER (WHERE cmt.message IS NOT NULL) AS comment_messages,
       -- reverse geocode
       ARRAY_AGG(DISTINCT rg.administrative_area_level_1) FILTER (WHERE rg.administrative_area_level_1 IS NOT NULL AND rg.administrative_area_level_1 <> '') AS rg_administrative_area_level_1,
       ARRAY_AGG(DISTINCT rg.administrative_area_level_2) FILTER (WHERE rg.administrative_area_level_2 IS NOT NULL AND rg.administrative_area_level_2 <> '') AS rg_administrative_area_level_2,
       ARRAY_AGG(DISTINCT rg.administrative_area_level_3) FILTER (WHERE rg.administrative_area_level_3 IS NOT NULL AND rg.administrative_area_level_3 <> '') AS rg_administrative_area_level_3,
       ARRAY_AGG(DISTINCT rg.country) FILTER (WHERE rg.country IS NOT NULL AND rg.country <> '') AS rg_country,
       ARRAY_AGG(DISTINCT rg.formatted_address) FILTER (WHERE rg.formatted_address IS NOT NULL AND rg.formatted_address <> '') AS rg_formatted_address,
       ARRAY_AGG(DISTINCT rg.locality) FILTER (WHERE rg.locality IS NOT NULL AND rg.locality <> '') AS rg_locality,
       ARRAY_AGG(DISTINCT rg.neighborhood) FILTER (WHERE rg.neighborhood IS NOT NULL AND rg.neighborhood <> '') AS rg_neighborhood,
       ARRAY_AGG(DISTINCT rg.postal_code) FILTER (WHERE rg.postal_code IS NOT NULL AND rg.postal_code <> '') AS rg_postal_code,
       ARRAY_AGG(DISTINCT rg.postal_code_suffix) FILTER (WHERE rg.postal_code_suffix IS NOT NULL AND rg.postal_code_suffix <> '') AS rg_postal_code_suffix,
       ARRAY_AGG(DISTINCT rg.premise) FILTER (WHERE rg.premise IS NOT NULL AND rg.premise <> '') AS rg_premise,
       ARRAY_AGG(DISTINCT rg.route) FILTER (WHERE rg.route IS NOT NULL AND rg.route <> '') AS rg_route,
       ARRAY_AGG(DISTINCT rg.street_number) FILTER (WHERE rg.street_number IS NOT NULL AND rg.street_number <> '') AS rg_street_number,
       ARRAY_AGG(DISTINCT rg.sub_locality_level_1) FILTER (WHERE rg.sub_locality_level_1 IS NOT NULL AND rg.sub_locality_level_1 <> '') AS rg_sub_locality_level_1,
       ARRAY_AGG(DISTINCT rg.sub_locality_level_2) FILTER (WHERE rg.sub_locality_level_2 IS NOT NULL AND rg.sub_locality_level_2 <> '') AS rg_sub_locality_level_2,
       ARRAY_AGG(DISTINCT rg.sub_premise) FILTER (WHERE rg.sub_premise IS NOT NULL AND rg.sub_premise <> '') AS rg_sub_premise,
       -- points of interest
       ARRAY_AGG(DISTINCT poi.poi_name) FILTER (WHERE poi.poi_name IS NOT NULL AND poi.poi_name <> '') AS poi_names,
       ARRAY_AGG(DISTINCT poi.poi_type) FILTER (WHERE poi.poi_type IS NOT NULL AND poi.poi_type <> '') AS poi_types
  FROM (
           SELECT 'video' AS type,
                  CONCAT('video_', CAST(c.id AS TEXT)) AS solr_id,
                  c.id,
                  c.year,
                  ARRAY (
                      SELECT r.name
                        FROM maw.role r
                       INNER JOIN video.category_role cr
                               ON r.id = cr.role_id
                              AND cr.category_id = c.id
                  ) AS allowed_roles,
                  c.name,
                  c.teaser_image_width,
                  c.teaser_image_height,
                  c.teaser_image_path,
                  c.teaser_image_sq_height,
                  c.teaser_image_sq_width,
                  c.teaser_image_sq_path,
                  c.gps_latitude,
                  c.gps_longitude
             FROM video.category c
       ) cat
 INNER JOIN video.video v
         ON v.category_id = cat.id
  LEFT OUTER JOIN video.comment cmt
          ON cmt.video_id = v.id
  LEFT OUTER JOIN video.reverse_geocode rg
          ON rg.video_id = v.id
  LEFT OUTER JOIN video.point_of_interest poi
          ON poi.video_id = v.id
GROUP BY cat.type,
         cat.solr_id,
         cat.id,
         cat.year,
         cat.allowed_roles,
         cat.name,
         cat.teaser_image_width,
         cat.teaser_image_height,
         cat.teaser_image_path,
         cat.teaser_image_sq_height,
         cat.teaser_image_sq_width,
         cat.teaser_image_sq_path,
         cat.gps_latitude,
         cat.gps_longitude;
";

        return await GetSourceCategoriesAsync(sql);
    }
}
