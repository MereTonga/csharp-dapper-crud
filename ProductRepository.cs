using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DapperCRUD
{
    internal class ProductRepository
    {
        readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE: Yeni bir ürün ekler.
        public async Task<int> CreateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = "INSERT INTO Products (Name_, Price) VALUES (@Name_, @Price)";
            return await db.ExecuteAsync(sql, product);
        }

        // READ: Tüm ürünleri listeler.
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = "SELECT ID, Name_, Price FROM Products";
            return await db.QueryAsync<Product>(sql);
        }

        // READ: Belirli bir ID'ye sahip ürünü bulur.
        public async Task<Product> GetByIdAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = "SELECT ID, Name_, Price FROM Products WHERE ID = @Id";
            return await db.QuerySingleOrDefaultAsync<Product>(sql, new { Id = id });
        }

        // UPDATE: Mevcut bir ürünü günceller.
        public async Task<int> UpdateAsync(Product product)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = "UPDATE Products SET Name_ = @Name_, Price = @Price WHERE ID = @Id";
            return await db.ExecuteAsync(sql, product);
        }

        // DELETE: Belirli bir ID'ye sahip ürünü siler.
        public async Task<int> DeleteAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Products WHERE ID = @Id";
            return await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
