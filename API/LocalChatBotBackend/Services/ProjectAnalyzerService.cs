using LocalChatBotBackend.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LocalChatBotBackend.Services
{
    public class ProjectAnalyzerService : IProjectAnalyzerService
    {
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

        private Task<object> AnalyzeAngularProject(string path)
        {
            var components = new List<object>();
            var services = new List<object>();
            var interfaces = new List<object>();
            var modules = new List<object>();

            foreach (var file in Directory.GetFiles(path, "*.ts", SearchOption.AllDirectories))
            {
                var content = File.ReadAllText(file);
                var nameMatch = Regex.Match(content, @"export class (\w+)");
                var name = nameMatch.Success ? nameMatch.Groups[1].Value : Path.GetFileNameWithoutExtension(file);

                if (content.Contains("@Component"))
                    components.Add(new { name, path = file, content });

                if (content.Contains("@Injectable"))
                    services.Add(new { name, path = file, content });

                if (content.Contains("interface "))
                    interfaces.Add(new { name, path = file, content });

                if (content.Contains("NgModule"))
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

        private Task<object> AnalyzeDotNetProject(string path)
        {
            var controllers = new List<object>();
            var servicesList = new List<object>();
            var models = new List<object>();
            var interfaces = new List<object>();

            foreach (var file in Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories))
            {
                var content = File.ReadAllText(file);
                var nameMatch = Regex.Match(content, @"class (\w+)");
                var name = nameMatch.Success ? nameMatch.Groups[1].Value : Path.GetFileNameWithoutExtension(file);

                // Controllers
                if (file.EndsWith("Controller.cs"))
                    controllers.Add(new { name, path = file, content });

                // Services heuristic: files in Services folder
                else if (file.Contains(Path.Combine("Services", "")))
                    servicesList.Add(new { name, path = file, content });

                // Models heuristic: files in Models folder
                else if (file.Contains(Path.Combine("Models", "")))
                    models.Add(new { name, path = file, content });

                // Interfaces heuristic: files ending with .cs in Interfaces folder
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
