namespace Ordering.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAiServices(this IServiceCollection services)
		{
			return services;
		}

		public static WebApplication UseApiServices(this WebApplication webApplication)
		{
			return webApplication;
		}
	}
}
