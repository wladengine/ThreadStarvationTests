using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ThreadStarvationDemo.CoreWebAPI.Models;

namespace ThreadStarvationDemo.CoreWebAPI.Repositories
{
    public class ProductsRepository
    {
        private readonly ProductDatabaseSettings _dbOptions;
        private IDbConnection DbConnection => new SqlConnection(_dbOptions.ConnectionString);

        public ProductsRepository(IOptions<ProductDatabaseSettings> dbOptions) => _dbOptions = dbOptions.Value;

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
}
