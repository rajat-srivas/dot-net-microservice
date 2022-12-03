using Dapper;
using DiscountAPI.Entities;
using DiscountAPI.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DiscountAPI.Repository
{
	public class DiscountRepository : IDiscountRepository
	{
		IConfiguration _configuration;
		string _connectionString;
		ILogger<DiscountRepository> _logger;

		public DiscountRepository(IConfiguration config, ILogger<DiscountRepository> log)
		{
			_configuration = config;
			_connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
			_logger= log;
			_logger.LogInformation(_connectionString);
		}

		public async Task<bool> CreateDiscountCoupon(Coupon coupon)
		{
			using var _connection = new NpgsqlConnection(_connectionString);
			string query = EmbedResourceReader.GetScriptsByName("CreateDiscountCoupon.sql");

			var inserted = await _connection.ExecuteAsync(query,
				new
				{
					CouponCode = coupon.CouponCode,
					Description = coupon.Description,
					Amount = coupon.Amount,
					Status = coupon.Status
				});

			return inserted == 0 ? false : true;
		}

		public async Task<bool> DeleteDiscountCopon(string couponCode)
		{
			using var _connection = new NpgsqlConnection(_connectionString);
			string query = EmbedResourceReader.GetScriptsByName("DeleteDiscountCoupon.sql");

			var deleted = await _connection.ExecuteAsync(query,
				new
				{
					CouponCode = couponCode
				});

			return deleted == 0 ? false : true;
		}

		public async Task<bool> UpdateDiscountCoupon(Coupon coupon)
		{
			using var _connection = new NpgsqlConnection(_connectionString);
			string query = EmbedResourceReader.GetScriptsByName("UpdateDiscountCoupon.sql");

			var updated = await _connection.ExecuteAsync(query,
				new
				{
					Description = coupon.Description,
					Amount = coupon.Amount,
					Status = coupon.Status,
					Id = coupon.Id
				});

			return updated == 0 ? false : true;
		}

		public async Task<Coupon> VerifyCoupon(string couponCode)
		{
			using var _connection = new NpgsqlConnection(_connectionString);

			var query = EmbedResourceReader.GetScriptsByName("VerifyCoupon.sql");

			var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>(
				query,new { CouponCode = couponCode});

			if(coupon == null)
			{
				return null;
			}
			return coupon;
		}
	}
}
