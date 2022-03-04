
using Models;

public partial class CreateProjectService
{
	private void WriteFileFromAsset(string fileNameAsset, string filenNameDestiny, string pathDestiny)
	{
		var ProgramFile = new FileCode
		{
			Code = File.ReadAllText(_directoryHandler.GetFileFromAsset(fileNameAsset)),
			FileName = filenNameDestiny
		};
		_codeGenerator
			.FileBuilder
				.WriteFile(ProgramFile,
				$"{CurrentDirectory}{pathDestiny}");
	}

	private void WriteFileDiretct(string fileNameAsset, string filenNameDestiny, string pathDestiny){
		string content = File.ReadAllText(_directoryHandler.GetFileFromAsset(fileNameAsset));
		File.WriteAllText($"{CurrentDirectory}{pathDestiny}/{filenNameDestiny}", content);
	}


	private void WriteApiAsset()
	{
		WriteFileFromAsset("ControllerAbstract.txt",
							"ControllerAbstract.cs",
							"/Api/Controllers/Abstract/");
		WriteFileFromAsset("ValidationFilterAttribute.txt",
							"ValidationFilterAttribute.cs",
							"/Api/Controllers/ActionFilters/");
		WriteFileFromAsset("AuthenticationExtension.txt",
							"AuthenticationExtension.cs",
							"/Api/Extensions/");
		WriteFileFromAsset("AppExtensions.txt",
							"AppExtensions.cs",
							"/Api/Extensions/");
		WriteFileFromAsset("ServicesExtensions.txt",
							"ServicesExtensions.cs",
							"/Api/Extensions/");
		WriteFileFromAsset("DetailError.txt",
							"DetailError.cs",
							"/Api/Models/");
		WriteFileFromAsset("ResponseModel.txt",
							"ResponseModel.cs",
							"/Api/Models/");
		WriteFileFromAsset("appsettings.Development.json",
							"appsettings.Development.json",
							"/Api/");
		WriteFileFromAsset("appsettings.Development.json",
							"appsettings.Development.json",
							"/Api/");
		WriteFileFromAsset("ViewModel.txt", "ViewModel.cs", 
							"/Entities/ViewModels/");

		WriteFileDiretct("nlog.config", "nlog.config", "/API/");
		WriteFileFromAsset("Startup.txt", "Startup.cs", "/API/");
		WriteFileFromAsset("NlogSetup.txt", "NlogSetup.cs", "/API/");

		System.Console.WriteLine("GENERATED Api initial files.");
	}

}
