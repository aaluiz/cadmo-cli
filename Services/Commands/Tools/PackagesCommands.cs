namespace Services.Commands.Tools
{
	public static class PackagesCommands
	{
		private static string GenerateAddPackageCommand(string package)
		{
			return $"add package {package}";
		}

		public static string[] PackagesForApi()
		{
			return new string[]{
			GenerateAddPackageCommand("Microsoft.AspNetCore.Authentication.JwtBearer"),
			GenerateAddPackageCommand("Microsoft.AspNetCore.Identity.EntityFrameworkCore"),
			GenerateAddPackageCommand("Microsoft.EntityFrameworkCore"),
			GenerateAddPackageCommand("Microsoft.EntityFrameworkCore.Design"),
			GenerateAddPackageCommand("Microsoft.EntityFrameworkCore.SqlServer"),
			GenerateAddPackageCommand("NLog.Extensions.Hosting"),
			GenerateAddPackageCommand("NLog.Extensions.Logging"),
			GenerateAddPackageCommand("NLog.Web.AspNetCore"),
			GenerateAddPackageCommand("System.Data.SqlClient"),
	};
		}

		public static string[] PackagesForModels()
		{
			return new string[]{
			GenerateAddPackageCommand("AutoMapper"),
			GenerateAddPackageCommand("Microsoft.AspNetCore.Identity.EntityFrameworkCore"),
			GenerateAddPackageCommand("Microsoft.EntityFrameworkCore"),
			GenerateAddPackageCommand("Microsoft.Extensions.Identity.Core"),
	};
		}

		public static string[] PackagesForRepository()
		{
			return new string[]{
			GenerateAddPackageCommand("Microsoft.Extensions.Configuration.Binder"),
			GenerateAddPackageCommand("Newtonsoft.Json"),
		};
		}


		public static string[] PackagesForTools()
		{
			return new string[]{
			GenerateAddPackageCommand("NLog.Extensions.Logging"),
		};
		}

		public static string[] PackagesForServices()
		{
			return new string[]{
			GenerateAddPackageCommand("AutoMapper"),
			GenerateAddPackageCommand("Microsoft.Extensions.Configuration"),
			GenerateAddPackageCommand("Microsoft.IdentityModel.Tokens"),
			GenerateAddPackageCommand("System.IdentityModel.Tokens.Jwt"),

	};
		}

	}
}