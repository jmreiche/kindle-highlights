using System.Collections.Generic;

namespace kindle_highlight_app.ClientApp.Dtos
{
    public class FileContent
    {
        public bool Failed { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, SortedDictionary<string, List<string>>> Content { get; set; }
    }
}