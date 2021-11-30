namespace MawSolrIndexer.Database {
    public class Category {
        public string Type { get; set; }
        public string SolrId { get; set; }
        public int Id { get; set; }
        public int Year { get; set; }
        public bool IsPrivate { get; set; }
        public string Name { get; set; }
        public int TeaserPhotoWidth { get; set; }
        public int TeaserPhotoHeight { get; set; }
        public string TeaserPhotoPath { get; set; }
        public int TeaserPhotoSqHeight { get; set; }
        public int TeaserPhotoSqWidth { get; set; }
        public string TeaserPhotoSqPath { get; set; }
        public float GpsLatitude { get; set; }
        public float GpsLongitude { get; set; }
    }
}
