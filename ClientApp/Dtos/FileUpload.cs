using Microsoft.AspNetCore.Http;

namespace kindle_highlight_app.ClientApp.Dtos
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
    }
}