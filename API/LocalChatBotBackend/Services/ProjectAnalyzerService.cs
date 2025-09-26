using LocalChatBotBackend.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LocalChatBotBackend.Services
{
    /// <summary>
    /// Service responsible for analyzing projects and returning structured JSON representations
    /// Supports Angular and .NET projects.
    /// </summary>
    public class ProjectAnalyzerService : IProjectAnalyzerService
    {
        /// <summary>
        /// Analyzes a project given its path and type.
        /// </summary>
        /// <param name="path">File system path to the project folder</param>
        /// <param name="type">Project type: 'angular' or 'dotnet'</param>
        /// <returns>Task that resolves to an object containing the structured project analysis</returns>
        public async Task<object> AnalyzeProjectAsync(string path, string type)
        {
            if (!Directory.Exists(path))
                return new { error = "Path does not exist" };

            if (type.ToLower() == "angular")
                return await AnalyzeAngularProject(path);
            else if (type.ToLower() == "dotnet")
                return await AnalyzeDotNetProject(path);
            else
                return new { error = "Unsupported project type" };
        }

        /// <summary>
        /// Analyzes an Angular project.
        /// Finds components, services, interfaces, and modules.
        /// </summary>
        private Task<object> AnalyzeAngularProject(string path)
        {
            // Lists to hold these specific angular items
            var components = new List<object>();
            var services = new List<object>();
            var interfaces = new List<object>();
            var modules = new List<object>();

            // Iterate through all TypeScript files recursively
            foreach (var file in Directory.GetFiles(path, "*.ts", SearchOption.AllDirectories))
            {
                // Extract class name from file or fallback to file name
                var content = File.ReadAllText(file);
                var nameMatch = Regex.Match(content, @"export class (\w+)");
                var name = nameMatch.Success ? nameMatch.Groups[1].Value : Path.GetFileNameWithoutExtension(file);

                if (content.Contains("@Component"))
                    components.Add(new { name, path = file, content });
                else if (content.Contains("@Injectable"))
                    services.Add(new { name, path = file, content });
                else if (content.Contains("interface "))
                    interfaces.Add(new { name, path = file, content });
                else if (content.Contains("NgModule"))
                    modules.Add(new { name, path = file, content });
            }

            return Task.FromResult<object>(new
            {
                type = "angular",
                components,
                services,
                interfaces,
                modules
            });
        }

        /// <summary>
        /// Analyzes a .NET project.
        /// Finds controllers, services, models, and interfaces using simple heuristics.
        /// </summary>
        private Task<object> AnalyzeDotNetProject(string path)
        {
            // Lists to hold these specific .NET items
            var controllers = new List<object>();
            var servicesList = new List<object>();
            var models = new List<object>();
            var interfaces = new List<object>();

            // Iterate through all C# files recursively
            foreach (var file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
            {
                // Extract class name from file or fallback to file name
                var content = File.ReadAllText(file);
                var nameMatch = Regex.Match(content, @"class (\w+)");
                var name = nameMatch.Success ? nameMatch.Groups[1].Value : Path.GetFileNameWithoutExtension(file);

                if (file.EndsWith("Controller.cs"))
                    controllers.Add(new { name, path = file, content });
                else if (file.Contains(Path.Combine("Services", "")))
                    servicesList.Add(new { name, path = file, content });
                else if (file.Contains(Path.Combine("Models", "")))
                    models.Add(new { name, path = file, content });
                else if (file.Contains(Path.Combine("Interfaces", "")))
                    interfaces.Add(new { name, path = file, content });
            }

            return Task.FromResult<object>(new
            {
                type = ".net",
                controllers,
                services = servicesList,
                models,
                interfaces
            });
        }
    }
}
