using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using ThreadStarvationDemo.SyncWebAPI.Models;

namespace ThreadStarvationDemo.SyncWebAPI.Repositories;

public class ProductsRepository(IOptions<ProductDatabaseSettings> dbOptions)
{
    private IDbConnection DbConnection => new SqlConnection(dbOptions.Value.ConnectionString);

    public IEnumerable<Product> GetProducts()
    {
        using IDbConnection db = DbConnection;
        return db.Query<Product>(@"WAITFOR DELAY '00:00:00.3'; SELECT Id, Name FROM wh.Product;");
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        using IDbConnection db = DbConnection;
        return await db.QueryAsync<Product>(@"WAITFOR DELAY '00:00:00.3'; SELECT Id, Name FROM wh.Product;");
    }
}
