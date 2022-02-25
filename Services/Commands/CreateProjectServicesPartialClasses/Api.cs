
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


}
