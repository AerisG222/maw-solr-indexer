using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace MawSolrIndexer.Database;

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

    protected async Task<IEnumerable<MultimediaCategory>> GetSourceCategoriesAsync(string sql)
    {
        return await QueryAsync<MultimediaCategory>(sql);
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
