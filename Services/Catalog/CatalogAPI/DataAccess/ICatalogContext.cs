using CatalogAPI.Entities;
using MongoDB.Driver;

namespace CatalogAPI.DataAccess
{
	public interface ICatalogContext
	{
		IMongoCollection<Product> Products { get; }
	}
}
