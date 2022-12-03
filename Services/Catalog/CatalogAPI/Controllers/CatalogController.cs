using CatalogAPI.Entities;
using CatalogAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CatalogAPI.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		private readonly IProductRepository _productRepository;
		private readonly ILogger<CatalogController> _logger;
		public CatalogController(IProductRepository product, ILogger<CatalogController> log)
		{
			_productRepository = product;
			_logger = log;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> GetProducts()
		{
			var result = await _productRepository.GetProducts();
			return Ok(result);
		}


		[HttpGet("{id}")]
		[ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> GetProductById(string id)
		{
			var result = await _productRepository.GetProductById(id);
			if (result == null)
			{
				_logger.LogError($"Product with id {id}, not found.");
				return NotFound();
			}
			return Ok(result);
		}

		[Route("category/{category}")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> GetProductByCategory(string category)
		{
			var result = await _productRepository.GetProductsByCategory(category);
			if (result == null || result.Count() <= 0)
			{
				_logger.LogError($"Products with category {category}, not found.");
				return NotFound();
			}
			return Ok(result);
		}

		[Route("name/{name}")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> GetProductByName(string name)
		{
			var result = await _productRepository.GetProductsByName(name);
			if (result == null || result.Count() <= 0)
			{
				_logger.LogError($"Products with name {name}, not found.");
				return NotFound();
			}
			return Ok(result);
		}

		[HttpPost]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		public async Task CreateProduct(Product product)
		{
			await _productRepository.CreateProduct(product);
			_logger.LogInformation($"Product {product.Name} created, with the id {product.Id}");
		}

		[HttpPut]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> UpdateProduct(Product product)
		{
			var result = await _productRepository.UpdateProduct(product);
			if (result)
			{
				_logger.LogInformation($"Product {product.Name} updated");
				return Ok(result);
			}
			else
			{
				_logger.LogInformation($"Product {product.Name} wiht id {product.Id} not found.");
				return NotFound(result);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(bool), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> DeleteProduct(string id)
		{
			var result = await _productRepository.DeleteProduct(id);
			if (result)
			{
				_logger.LogInformation($"Product with id {id} has been deleted");
				return Ok(result);
			}
			else
			{
				_logger.LogInformation($"Product with id {id} not found.");
				return NotFound(result);
			}
		}
	}
}
