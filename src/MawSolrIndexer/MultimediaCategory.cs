using System;
using System.Text.Json.Serialization;

namespace MawSolrIndexer
{
    public class MultimediaCategory
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("solr_id")]
        public string SolrId { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("allowed_roles")]
        public string[] AllowedRoles { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("teaser_photo_width")]
        public int TeaserPhotoWidth { get; set; }

        [JsonPropertyName("teaser_photo_height")]
        public int TeaserPhotoHeight { get; set; }

        [JsonPropertyName("teaser_photo_path")]
        public string TeaserPhotoPath { get; set; }

        [JsonPropertyName("teaser_photo_sq_height")]
        public int TeaserPhotoSqHeight { get; set; }

        [JsonPropertyName("teaser_photo_sq_width")]
        public int TeaserPhotoSqWidth { get; set; }

        [JsonPropertyName("teaser_photo_sq_path")]
        public string TeaserPhotoSqPath { get; set; }

        [JsonPropertyName("gps_latitude")]
        public float? GpsLatitude { get; set; }

        [JsonPropertyName("gps_longitude")]
        public float? GpsLongitude { get; set; }

        // comments

        [JsonPropertyName("comment_entry_dates")]
        public DateTime[] CommentEntryDates { get; set; }

        [JsonPropertyName("comment_messages")]
        public string[] CommentMessages { get; set; }

        // makes + models

        [JsonPropertyName("camera_makes")]
        public string[] CameraMakes { get; set; }

        [JsonPropertyName("camera_models")]
        public string[] CameraModels { get; set; }

        // reverse geocode

        [JsonPropertyName("rg_administrative_area_level_1")]
        public string[] RgAdministrativeAreaLevel1 { get; set; }

        [JsonPropertyName("rg_administrative_area_level_2")]
        public string[] RgAdministrativeAreaLevel2 { get; set; }

        [JsonPropertyName("rg_administrative_area_level_3")]
        public string[] RgAdministrativeAreaLevel3 { get; set; }

        [JsonPropertyName("rg_country")]
        public string[] RgCountry { get; set; }

        [JsonPropertyName("rg_formatted_address")]
        public string[] RgFormattedAddress { get; set; }

        [JsonPropertyName("rg_locality")]
        public string[] RgLocality { get; set; }

        [JsonPropertyName("rg_neighborhood")]
        public string[] RgNeighborhood { get; set; }

        [JsonPropertyName("rg_postal_code")]
        public string[] RgPostalCode { get; set; }

        [JsonPropertyName("rg_postal_code_suffix")]
        public string[] RgPostalCodeSuffix { get; set; }

        [JsonPropertyName("rg_premise")]
        public string[] RgPremise { get; set; }

        [JsonPropertyName("rg_route")]
        public string[] RgRoute { get; set; }

        [JsonPropertyName("rg_street_number")]
        public string[] RgStreetNumber { get; set; }

        [JsonPropertyName("rg_sub_locality_level_1")]
        public string[] RgSubLocalityLevel1 { get; set; }

        [JsonPropertyName("rg_sub_locality_level_2")]
        public string[] RgSubLocalityLevel2 { get; set; }

        [JsonPropertyName("rg_sub_premise")]
        public string[] RgSubPremise { get; set; }

        // points of interest

        [JsonPropertyName("poi_names")]
        public string[] PoiNames { get; set; }

        [JsonPropertyName("poi_types")]
        public string[] PoiTypes { get; set; }
    }
}