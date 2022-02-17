using Services.Abstract;
using Contracts.Interfaces;
using Models;
using System.Collections.Immutable;
using System.Text;

namespace Services.Commands
{
	[AddService]
	public class AutoMapperCommandService : AbstractService, IAutoMapperCommandService
	{
		ICodeGenerator _codeGenerator;
		IMethodDefinition _methodDefinition;
		public AutoMapperCommandService(ICodeGenerator codeGenerator, IMethodDefinition methodDefinition)
		{
			_codeGenerator = codeGenerator;
			_methodDefinition = methodDefinition;
		}
		public int Execute(string[] args)
		{
			if (!ValidateArgs(args)) return -1;

			_codeGenerator.FileBuilder.WriteFile(GetFileCodeAutoMapper(), $"{CurrentDirectory}/Entities/AutoMapper");
			System.Console.WriteLine("GENERATED ../Entities/AutoMapper/AutoMapperProfile.cs");

			return 1;
		}

		public FileCode GetFileCodeAutoMapper()
		{

			var imports = new string[]{
				"AutoMapper",
				"Entities.Models",
				"Entities.ViewModels",
				"System"
			}.ToImmutableList();

			var inharitage = new string[]{
				"Profile",
			}.ToImmutableList();

			var properties = new Property[]{
				ProfileNameProperty()
			}.ToImmutableList();

			var methods = GetMethods();

			var result = _codeGenerator.ClassGenerator
				.CreateClass(imports, "AutoMapperProfile", "Entities.AutoMapper", methods, properties, inharitage);

			return result;
		}

		private ImmutableList<IMethodDefinition> GetMethods(){
			var methods = new List<IMethodDefinition>();
			methods.Add(MainContructorCreation());
			methods.Add(ConstructorWithBaseObject());
			methods.Add(ConstructoWithBaseObjectAction());
			methods.Add(GetHashCodeMethod());
			methods.Add(EqualsMethod());
			methods.Add(ToStringMethod());
			return methods.ToImmutableList();
		}

		private IMethodDefinition MainContructorCreation(){
			var constructor = _methodDefinition.Builder
			.Name("AutoMapperProfile")
			.LogiContent(MappingModels())
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return constructor;
		}

		private string MappingModels(){
			var fileModels = Directory.GetFiles($"{CurrentDirectory}/Entities/Models").ToList();
			var Models = fileModels.Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

			var result = string.Join("\n ",
				Models.Select( x => {
					StringBuilder content = new StringBuilder();
					content.AppendLine($"//--------- Model {x}-----------------");
					content.AppendLine($"CreateMap<{x}, {x}ViewModel>();");
					content.AppendLine($"CreateMap<{x}NewViewModel, {x}>();");
					content.AppendLine($"CreateMap<{x}UpdateViewModel, {x}>();\n");
					return content.ToString();
				})
			);
			return result;
		}

		private IMethodDefinition ContructorCreation(){
			var constructor = _methodDefinition.Builder
			.Name("AutoMapperProfile")
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return constructor;
		}

		private IMethodDefinition ConstructorWithBaseObject(){
			var baseObject = new string[]{
				"profileName"
			}.ToImmutableList();

			var parameters = new Parameter[]{
				new Parameter{
					Type = "string",
					Name = "profileName"
				}
			}.ToImmutableList();


			var constructor = _methodDefinition.Builder
			.Name("AutoMapperProfile")
			.BaseImplementationObjects(baseObject)
			.Parameters(parameters)
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return constructor;
		}

		private IMethodDefinition ConstructoWithBaseObjectAction(){
			var baseObject = new string[]{
				"profileName",
				"configurationAction"
			}.ToImmutableList();

			var parameters = new Parameter[]{
				new Parameter{
					Type = "string",
					Name = "profileName"
				},
				new Parameter{
					Type = "Action<IProfileExpression>",
					Name = "configurationAction"
				},
			}.ToImmutableList();


			var constructor = _methodDefinition.Builder
			.Name("AutoMapperProfile")
			.BaseImplementationObjects(baseObject)
			.Parameters(parameters)
			.ReturnDefinition(new ReturnDefinition
			{
				IsConstructor = true,
				Type = ""
			})
			.Create();
			return constructor;
		}

		private Property ProfileNameProperty(){
			var result = new Property
			{
				Name = "ProfileName",
				TypeProperty = "override string",
				Visibility = Visibility.Public,
				Attribuition = "> base.ProfileName"
			};
			return result;
		} 

		private IMethodDefinition EqualsMethod(){

			var parameters = new Parameter[]{
				new Parameter{
					Type = "object?",
					Name = "obj"
				}
			}.ToImmutableList();

			var method = _methodDefinition.Builder
			.Name("Equals")
			.Parameters(parameters)
			.LogiContent("return base.Equals(obj);")
			.ReturnDefinition(new ReturnDefinition
			{
				Visibility = Visibility.Public,
				Type = "override bool"
			})
			.Create();
			return method;
		}

		private IMethodDefinition GetHashCodeMethod(){

			var method = _methodDefinition.Builder
			.Name("GetHashCode")
			.LogiContent("return base.GetHashCode();")
			.ReturnDefinition(new ReturnDefinition
			{
				Visibility = Visibility.Public,
				Type = "override int"
			})
			.Create();
			return method;
		}

		private IMethodDefinition ToStringMethod(){

			var method = _methodDefinition.Builder
			.Name("ToString")
			.LogiContent("return base.ToString()!;")
			.ReturnDefinition(new ReturnDefinition
			{
				Visibility = Visibility.Public,
				Type = "override string"
			})
			.Create();
			return method;
		}

		

		protected override bool ValidateArgs(string[] args)
		{
			if (!IsValidArgs(args)) return false;
			return IsTheReservedWord("automapper", args);
		}
	}
}