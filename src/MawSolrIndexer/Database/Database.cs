using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace MawSolrIndexer.Database {
    public abstract class Database {
        readonly string _connString;

        static Database() {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        public Database(string connString) {
            if(string.IsNullOrWhiteSpace(connString)) {
                throw new ArgumentNullException(nameof(connString));
            }

            _connString = connString;
        }

        protected async Task<IEnumerable<Category>> GetSourceCategoriesAsync(string sql)
        {
            return await QueryAsync<Category>(sql);
        }

        protected async Task EnrichWithCommentsAsync(MultimediaCategory category, string sql)
        {
            var comments = await QueryAsync<Comment>(sql);

            category.CommentEntryDates = comments.UniqueArray(x => x.EntryDate);
            category.CommentMessages = comments.UniqueArray(x => x.Message);
        }

        protected async Task EnrichWithLocationAsync(MultimediaCategory category, string sql)
        {
            var locations = await QueryAsync<Location>(sql);

            category.RgAdministrativeAreaLevel1 = locations.UniqueArray(x => x.AdministrativeAreaLevel1);
            category.RgAdministrativeAreaLevel2 = locations.UniqueArray(x => x.AdministrativeAreaLevel2);
            category.RgAdministrativeAreaLevel3 = locations.UniqueArray(x => x.AdministrativeAreaLevel3);
            category.RgCountry = locations.UniqueArray(x => x.Country);
            category.RgFormattedAddress = locations.UniqueArray(x => x.FormattedAddress);
            category.RgLocality = locations.UniqueArray(x => x.Locality);
            category.RgNeighborhood = locations.UniqueArray(x => x.Neighborhood);
            category.RgPostalCode = locations.UniqueArray(x => x.PostalCode);
            category.RgPostalCodeSuffix = locations.UniqueArray(x => x.PostalCodeSuffix);
            category.RgPremise = locations.UniqueArray(x => x.Premise);
            category.RgRoute = locations.UniqueArray(x => x.Route);
            category.RgStreetNumber = locations.UniqueArray(x => x.StreetNumber);
            category.RgSubLocalityLevel1 = locations.UniqueArray(x => x.SubLocalityLevel1);
            category.RgSubLocalityLevel2 = locations.UniqueArray(x => x.SubLocalityLevel2);
            category.RgSubPremise = locations.UniqueArray(x => x.SubPremise);
        }

        protected async Task EnrichWithPointsOfInterestAsync(MultimediaCategory category, string sql)
        {
            var pois = await QueryAsync<PointOfInterest>(sql);

            category.PoiNames = pois.UniqueArray(x => x.PoiName);
            category.PoiTypes = pois.UniqueArray(x => x.PoiType);
        }

        protected async Task EnrichWithMakeAndModelAsync(MultimediaCategory category, string sql)
        {
            var makes = await QueryAsync<MakeAndModel>(sql);

            category.CameraMakes = makes.UniqueArray(x => x.Make);
            category.CameraModels = makes.UniqueArray(x => x.Model);
        }

        async Task<IEnumerable<T>> QueryAsync<T>(string sql) {
            return await RunAsync(conn => conn.QueryAsync<T>(sql));
        }

        async Task<T> RunAsync<T>(Func<IDbConnection, Task<T>> queryData)
        {
            if(queryData == null)
            {
                throw new ArgumentNullException(nameof(queryData));
            }

            using var conn = await GetConnectionAsync();

            return await queryData(conn).ConfigureAwait(false);
        }

        async Task<IDbConnection> GetConnectionAsync()
        {
            var conn = new NpgsqlConnection(_connString);

            await conn.OpenAsync().ConfigureAwait(false);

            return conn;
        }
    }
}