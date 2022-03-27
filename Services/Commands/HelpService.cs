using Contracts.Interfaces;
using Spectre.Console;
using System.Text;

[AddService]
public class HelpService : IHelpService
{
    public int Execute(string[] args)
    {
        var panel = new Panel(Help());
        panel.Border = BoxBorder.Double;
        panel.Expand();

        AnsiConsole.Write(panel);
//        AnsiConsole.Markup(Help());
        return 1;
    }


    private string Help()
    {
        StringBuilder commandsHelp = new StringBuilder();

        commandsHelp.AppendLine("[bold]Help :sos_button: [/]\n");
        commandsHelp.AppendLine(@"Create New Web Api:
[blue]cm new[/] [hotpink]<APP_NAME>[/]
");
        commandsHelp.AppendLine(@"Create Model (And ViewModels) Template:
[blue]cm  (g|generate)  model [/] [hotpink]<MODEL_NAME>[/]
");
        commandsHelp.AppendLine(@"Create Model from json file script: 
[blue]cm (g|generate) model [/] [teal]--with-script [/] [hotpink]<MODEL_JSON_FILE_NAME>[/]
[yellow]Warning:[/] Json`s file script are in Entities/JsonModelsDefinition folder. 
");
        commandsHelp.AppendLine(@"Create Model from all  json files  overwriting generated files: 
[blue]cm (g|generate) model[/] [teal]--with-all-scripts[/] [hotpink]<MODEL_JSON_FILE_NAME>[/] [teal]--safety[/]
[yellow]Waring:[/] Json`s file script are in Entities/JsonModelsDefinition folder. 
");
        commandsHelp.AppendLine(@"Create Model from all  json files without overwritting generated files: 
[blue]cm (g|generate) model[/] [teal]--with-all-scripts[/] [hotpink]<MODEL_JSON_FILE_NAME>[/] [teal]--force[/]
[yellow]Waring:[/] Json`s file script are in Entities/JsonModelsDefinition folder. 
");
        commandsHelp.AppendLine(@"Generate Repository based on Model:
[blue]cm (g|generate) repository [/][teal]--model[/] [hotpink]<MODEL_NAME>[/]
");
        commandsHelp.AppendLine(@"Generate CRUD Service based on Model:
[blue]cm (g|generate) service-crud[/] [teal]--model[/] [hotpink]<MODEL_NAME>[/]
");
        commandsHelp.AppendLine(@"Generate CRUD Controller based on Model:
[blue]cm (g|generate) controller-crud-curd [/] [teal]--model[/] [hotpink]<MODEL_NAME>[/]
");
        commandsHelp.AppendLine(@"Update Repository Extension to dependency injection mapping:
[blue]cm repository-di[/]
");
        commandsHelp.AppendLine(@"Update Service Extension to dependency injection mapping:
[blue]cm service-di[/]
");
        commandsHelp.AppendLine(@"Add Packge in current project:
[blue]cm add[/] [hotpink]<PACKAGE_NAME>[/]
or 
[blue]cm add[/] [hotpink]<PACKAGE_NAME>[/] [teal]--verison[/] [hotpink]<VERSION_NUMBER>[/]
");
       commandsHelp.AppendLine(@"[bold]Entity Framework Commands[/]
[yellow]Warning:[/] require dotnet-ef installed and execute on [yellow]Api[/] folder:file_folder:. 
");
        commandsHelp.AppendLine(@"Add Migration:
[blue]cm add-migration[/] [hotpink]<MIGRATION_NAME>[/]
");
        commandsHelp.AppendLine(@"Remove Migration:
[blue]cm remove-migration[/] 
");
        commandsHelp.AppendLine(@"List Migration:
[blue]cm list-migration[/]
");
        commandsHelp.AppendLine(@"List Migration:
[blue]cm update-database[/]");
         return commandsHelp.ToString();
    }
}
