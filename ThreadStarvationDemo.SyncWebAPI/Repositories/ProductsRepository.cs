using Dapper;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using ThreadStarvationDemo.SyncWebAPI.Models;

namespace ThreadStarvationDemo.SyncWebAPI.Repositories;

public partial class ProductsRepository
{
    private readonly ProductDatabaseSettings _dbOptions;
    private IDbConnection DbConnection => new SqlConnection(_dbOptions.ConnectionString);

    public ProductsRepository(IOptions<ProductDatabaseSettings> dbOptions) => _dbOptions = dbOptions.Value;

    public IEnumerable<Product> GetProducts()
    {
        using var db = DbConnection;
        return db.Query<Product>(@"WAITFOR DELAY '00:00:00.3'; SELECT Id, Name FROM wh.Product;");
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        using var db = DbConnection;
        return await db.QueryAsync<Product>(@"WAITFOR DELAY '00:00:00.3'; SELECT Id, Name FROM wh.Product;");
    }
}
