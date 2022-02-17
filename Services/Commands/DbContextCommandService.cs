using Contracts.Interfaces;
using Services.Abstract;
using Models;
using System.Collections.Immutable;

namespace Services.Commands
{

	[AddService]
	public class DbContextCommandService : AbstractService, IDbContextCommandService
	{
		private readonly IMethodDefinition _methodDefinition;
		private readonly ICodeGenerator _codeGenerator;
		public DbContextCommandService(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
		}

		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;
			
			_codeGenerator.FileBuilder.WriteFile(GenerateApiDbContext(), $"{CurrentDirectory}/Entities/Data");
			System.Console.WriteLine("GENERATED ../Entities/Data/ApiDbContext.cs");

			return 1;
		}

		private FileCode GenerateApiDbContext()
		{
			var imports = new string[]{
				"Entities.Models",
				"Microsoft.EntityFrameworkCore",
			}.ToImmutableList();

			var inharitage = new string[]{
				"DbContext"
			}.ToImmutableList();

			var result = _codeGenerator.ClassGenerator
			.CreateClass(imports, "ApiDbContext", "Entities.Data", Construtor(), Properties(), inharitage);

			return result;
		}

		private ImmutableList<IMethodDefinition> Construtor()
		{

			var baseObject = new string[]{
				"options"
			}.ToImmutableList();

			var parameters = new Parameter[]{
				new Parameter{
					Type = "DbContextOptions<ApiDbContext>",
					Name = "options"
				}
			}.ToImmutableList();


			var constructor = _methodDefinition.Builder
			.Name("ApiDbContext")
			.BaseImplementationObjects(baseObject)
			.Parameters(parameters)
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return new IMethodDefinition[] { constructor }.ToImmutableList();

		}

		private ImmutableList<Property> Properties()
		{
			var models = GetModels();

			return models!.Select(model =>
			{
				var result = new Property
				{
					Name = $"{model}s",
					TypeProperty = $"DbSet<{model}>?",
					Visibility = Visibility.Public,
					hasGeterAndSeter = true
			};

				return result;
			}
			).ToImmutableList();
		}

		private List<string>? GetModels()
		{
			var fileModels = Directory.GetFiles($"{CurrentDirectory}/Entities/Models").ToList();
			var Models = fileModels.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

			return Models;
		}

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("dbcontext", args);
		}
	}
}