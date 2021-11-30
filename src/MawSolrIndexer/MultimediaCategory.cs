using System;

namespace MawSolrIndexer
{
    public class MultimediaCategory
    {
        public string[] CameraMakes { get; set; }
        public string[] CameraModels { get; set; }
        public DateTime[] CommentEntryDates { get; set; }
        public string[] CommentMessages { get; set; }
        public float GpsLatitude { get; set; }
        public float GpsLongitude { get; set; }
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public string[] PoiNames { get; set; }
        public string[] PoiTypes { get; set; }
        public string[] RgAdministrativeAreaLevel1 { get; set; }
        public string[] RgAdministrativeAreaLevel2 { get; set; }
        public string[] RgAdministrativeAreaLevel3 { get; set; }
        public string[] RgCountry { get; set; }
        public string[] RgFormattedAddress { get; set; }
        public string[] RgLocality { get; set; }
        public string[] RgNeighborhood { get; set; }
        public string[] RgPostalCode { get; set; }
        public string[] RgPostalCodeSuffix { get; set; }
        public string[] RgPremise { get; set; }
        public string[] RgRoute { get; set; }
        public string[] RgStreetNumber { get; set; }
        public string[] RgSubLocalityLevel1 { get; set; }
        public string[] RgSubLocalityLevel2 { get; set; }
        public string[] RgSubPremise { get; set; }
        public string SolrId { get; set; }
        public int TeaserPhotoHeight { get; set; }
        public int TeaserPhotoWidth { get; set; }
        public string TeaserPhotoPath { get; set; }
        public int TeaserPhotoSqHeight { get; set; }
        public int TeaserPhotoSqWidth { get; set; }
        public string TeaserPhotoSqPath { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
    }
}