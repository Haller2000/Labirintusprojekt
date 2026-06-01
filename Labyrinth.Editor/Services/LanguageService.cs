using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Labyrinth.Editor.Services
{
    public class LanguageService
    {
        public Dictionary<string, string> LoadLanguage(string path)
        {
            string json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
    }
}