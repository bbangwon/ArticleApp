using Microsoft.AspNetCore.Components.Forms;

namespace NoticeApp.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment environment;

        public FileUploadService(IWebHostEnvironment env)
        {
            this.environment = env;
        }

        public async Task UploadAsync(IBrowserFile file)
        {
            var directory = Path.Combine(environment.WebRootPath, "Upload");
            var path = Path.Combine(directory, file.Name);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            await using FileStream fs = new FileStream(path, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);
        }
    }
}
