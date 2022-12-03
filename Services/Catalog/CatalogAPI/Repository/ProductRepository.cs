using CatalogAPI.DataAccess;
using CatalogAPI.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CatalogAPI.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext _catalogContext;
		public ProductRepository(ICatalogContext context)
		{
			_catalogContext = context;
		}
		public async Task CreateProduct(Product product)
		{
			if (!string.IsNullOrEmpty(product.Name) && !string.IsNullOrEmpty(product.Category))
				await _catalogContext.Products.InsertOneAsync(product);
		}

		public async Task<bool> DeleteProduct(string id)
		{
			FilterDefinition<Product> deleteFilter = Builders<Product>.Filter.Eq(x => x.Id, id);
			var deleteResult = await _catalogContext.Products.DeleteOneAsync(deleteFilter);
			return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;

		}

		public async Task<Product> GetProductById(string id)
		{
			FilterDefinition<Product> idFilter = Builders<Product>.Filter.Eq(x => x.Id, id);
			return await _catalogContext.Products
				.Find(idFilter).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await _catalogContext.
				Products.Find(x => true).
				ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
		{
			FilterDefinition<Product> categoryFilter = Builders<Product>.Filter.Eq(x => x.Category, category);
			return await _catalogContext.Products
				.Find(categoryFilter).ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductsByName(string name)
		{
			FilterDefinition<Product> nameFilter = Builders<Product>.Filter.Eq(x => x.Name, name);
			return await _catalogContext.Products
				.Find(nameFilter).ToListAsync();
		}

		public async Task<bool> UpdateProduct(Product product)
		{
			var updateResult = await _catalogContext.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);
			return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
		}
	}
}
