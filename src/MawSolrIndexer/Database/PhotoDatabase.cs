using System.Collections.Generic;
using System.Threading.Tasks;

namespace MawSolrIndexer.Database;

public class PhotoDatabase
    : Database, ICategorySource
{
    public PhotoDatabase(string connString)
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
       cat.teaser_photo_width,
       cat.teaser_photo_height,
       cat.teaser_photo_path,
       cat.teaser_photo_sq_height,
       cat.teaser_photo_sq_width,
       cat.teaser_photo_sq_path,
       cat.gps_latitude,
       cat.gps_longitude,
       -- comments
       ARRAY_AGG(DISTINCT cmt.entry_date) FILTER (WHERE cmt.entry_date IS NOT NULL) AS comment_entry_dates,
       ARRAY_AGG(DISTINCT cmt.message) FILTER (WHERE cmt.message IS NOT NULL) AS comment_messages,
       -- make and model
       ARRAY_AGG(DISTINCT mk.name) FILTER (WHERE mk.name IS NOT NULL) AS camera_makes,
       ARRAY_AGG(DISTINCT mdl.name) FILTER (WHERE mdl.name IS NOT NULL) AS camera_models,
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
           SELECT 'photo' AS type,
                  CONCAT('photo_', CAST(c.id AS TEXT)) AS solr_id,
                  c.id,
                  c.year,
                  ARRAY (
                      SELECT r.name
                        FROM maw.role r
                       INNER JOIN photo.category_role cr
                               ON r.id = cr.role_id
                              AND cr.category_id = c.id
                  ) AS allowed_roles,
                  c.name,
                  c.teaser_photo_width,
                  c.teaser_photo_height,
                  c.teaser_photo_path,
                  c.teaser_photo_sq_height,
                  c.teaser_photo_sq_width,
                  c.teaser_photo_sq_path,
                  c.gps_latitude,
                  c.gps_longitude
             FROM photo.category c
       ) cat
 INNER JOIN photo.photo p
         ON p.category_id = cat.id
  LEFT OUTER JOIN photo.comment cmt
          ON cmt.photo_id = p.id
  LEFT OUTER JOIN photo.make mk
          ON mk.id = p.make_id
  LEFT OUTER JOIN photo.model mdl
          ON mdl.id = p.model_id
  LEFT OUTER JOIN photo.reverse_geocode rg
          ON rg.photo_id = p.id
  LEFT OUTER JOIN photo.point_of_interest poi
          ON poi.photo_id = p.id
GROUP BY cat.type,
         cat.solr_id,
         cat.id,
         cat.year,
         cat.allowed_roles,
         cat.name,
         cat.teaser_photo_width,
         cat.teaser_photo_height,
         cat.teaser_photo_path,
         cat.teaser_photo_sq_height,
         cat.teaser_photo_sq_width,
         cat.teaser_photo_sq_path,
         cat.gps_latitude,
         cat.gps_longitude;
";

        return await GetSourceCategoriesAsync(sql);
    }
}
