using CatalogAPI.DataAccess;
using CatalogAPI.Entities;
using CatalogAPI.Repository;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Threading;
using MongoDB.Driver.Core.Operations;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace CatalogApi.Test
{
	public class ProductRepoTest
	{
		Mock<CatalogContext> _catalogContextMock;
		Mock<ProductRepository> _productRepositoryMock;
		Mock<IMongoCollection<Product>> _mockedProducts ;
		Mock<IConfiguration> _mockedConfig;
		Mock<IMongoDatabase> _dbMock;
		Mock<IMongoClient> _clientMock;
		Mock<IConfigurationSection> _mockSection;


		[SetUp]
		public void Setup()
		{
			_mockedConfig = new Mock<IConfiguration>();
			_mockSection = new Mock<IConfigurationSection>();
			
			//Mock Section to return string for any section which is fetched by config
			_mockSection.Setup(x => x.Value).Returns("ConfigValue");
			_mockedConfig.Setup(x => x.GetSection(It.IsAny<string>())).Returns(_mockSection.Object);

			var list = GetPreConfigProducts().ToList();

			//Mock IAsyncCursor
			var mockCursor = new Mock<IAsyncCursor<Product>>();
			mockCursor.Setup(x => x.Current).Returns(list);
			mockCursor.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
			mockCursor.SetupSequence(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(true)).Returns(Task.FromResult(false));

			//Mock Collection and setup its aggregate and find methods
			_mockedProducts = new Mock<IMongoCollection<Product>>();
			_mockedProducts.Setup(x => x.AggregateAsync(It.IsAny<PipelineDefinition<Product, Product>>(), It.IsAny<AggregateOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);
			_mockedProducts.Setup(x => x.FindAsync(It.IsAny<FilterDefinition<Product>>(), It.IsAny<FindOptions<Product,Product>>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);

			//Mock Imongodatabase and setup GetCollection method to return the mocked product collection
			_dbMock = new Mock<IMongoDatabase>();
			_dbMock.Setup(x => x.GetCollection<Product>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(_mockedProducts.Object);

			_clientMock = new Mock<IMongoClient>();
			_clientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(_dbMock.Object);


			_catalogContextMock = new Mock<CatalogContext>(_mockedConfig.Object, _clientMock.Object);
			_catalogContextMock.CallBase = true;

			_productRepositoryMock = new Mock<ProductRepository>(_catalogContextMock.Object);
			_productRepositoryMock.CallBase = true;


		}

		[Test]
		[TestCase("602d2149e773f2a3990b47f5", true)]
		[TestCase("602d2149e773f2a3990b4799", false)]
//		[Ignore("Need to fix -- Better to test our Service rather than mocking the db context for mongodb api")]
		public async Task ValidID_ReturnsProduct(string productId, bool expectedResult)
		{	
			var result = await _productRepositoryMock.Object.GetProductById(productId);
			Assert.AreEqual(result.Id == productId, expectedResult);
			Assert.Pass();
		}
		private IEnumerable<Product> GetPreConfigProducts()
		{
			return new List<Product>()
			{
				new Product()
				{
					Id = "602d2149e773f2a3990b47f5",
					Name = "Mocked Iphone X",
					Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
					Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
					ImageFile = "product-1.png",
					Price = 950.00M,
					Category = "Smart Phone"
				},
			};
		}
	}
}