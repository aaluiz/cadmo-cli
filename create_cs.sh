echo $1
if  $1 == "clear"
then
  echo Clean up
  [ -d "cadmo-cli/" ] && [ ! -L "cadmo-cli" ] && rm -rf cadmo-cli
  [ -d "Tests/" ] && [ ! -L "Tests" ] && rm -rf Tests
  [ -d "Constracts/" ] && [ ! -L "Constracts" ] && rm -rf Constracts
  [ -d "Models/" ] && [ ! -L "Models" ] && rm -rf Models
  [ -d "Services/" ] && [ ! -L "Services" ] && rm -rf Services
  [ -d "Tools/" ] && [ ! -L "Tools" ] && rm -rf Tools 
  [ -d "SourceGenerator/" ] && [ ! -L "SouceGenerator" ] && rm -rf SouceGenerator 
  [ -f "CadmoSolution.sln" ] && rm -rf CadmoSolution.sln
fi
dotnet new sln -n CadmoSolution
dotnet new console -o cadmo-cli
dotnet new nunit -o Tests
dotnet new classlib -o Constracts
dotnet new classlib -o Models
dotnet new classlib -o Services
dotnet new classlib -o Tools
dotnet new classlib -o SourceGenerator
cd cadmo-cli
dotnet add package Microsoft.Extensions.DependencyInjection --version 6.0.0
dotnet add package Microsoft.Extensions.Hosting
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Services/Services.csproj
dotnet add reference ../Tools/Tools.csproj
cd ..
cd Tests
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Services/Services.csproj
dotnet add reference ../Tools/Tools.csproj
cd ..
cd SouceGenerator
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Services/Services.csproj
dotnet add reference ../Tools/Tools.csproj
cd ..
cd Services
dotnet add reference ../Constracts/Constracts.csproj
dotnet add reference ../Models/Models.csproj
dotnet add reference ../Tools/Tools.csproj
cd ..
dotnet sln CadmoSolution.sln add cadmo-cli/cadmo-cli.csproj
dotnet sln CadmoSolution.sln add Constracts/Constracts.csproj
dotnet sln CadmoSolution.sln add Tests/Tests.csproj
dotnet sln CadmoSolution.sln add Models/Models.csproj
dotnet sln CadmoSolution.sln add Services/Services.csproj

read -p "Press any key to resume..."
