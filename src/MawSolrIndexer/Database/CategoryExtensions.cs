namespace MawSolrIndexer.Database
{
    public static class CategoryExtensions
    {
        public static MultimediaCategory ToMultimediaCategory(this Category category)
        {
            return new MultimediaCategory {
                Type = category.Type,
                SolrId = category.SolrId,
                Id = category.Id,
                Year = category.Year,
                IsPrivate = category.IsPrivate,
                Name = category.Name,
                TeaserPhotoHeight = category.TeaserPhotoHeight,
                TeaserPhotoWidth = category.TeaserPhotoWidth,
                TeaserPhotoPath = category.TeaserPhotoPath,
                TeaserPhotoSqHeight = category.TeaserPhotoSqHeight,
                TeaserPhotoSqWidth = category.TeaserPhotoSqWidth,
                TeaserPhotoSqPath = category.TeaserPhotoSqPath,
                GpsLatitude = category.GpsLatitude,
                GpsLongitude = category.GpsLongitude
            };
        }
    }
}