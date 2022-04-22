using Contracts.Interfaces;
using Spectre.Console;

namespace Services.Commands
{
    [AddService]
    public class VersionService : IVersionService
    {
        public int Execute(string[] args)
        {
            string version = "1.1.8-beta";
            AnsiConsole.Write(
                new FigletText("CADMO-CLI")
                    .LeftAligned()
                    .Color(Color.LightSkyBlue1));

            AnsiConsole.WriteLine($"Version: {version}");
            return 1;
        }
    }
}
