
using Models;

public partial class CreateProjectService
{
	private void SetupTools(){
		WriteFileFromAsset("StringExtension.txt", "StringExtension.cs", "/Tools/Extension/String/");
	}
}
