using Microsoft.AspNetCore.Components.Forms;

namespace NoticeApp.Services
{
    public interface IFileUploadService
    {
        Task UploadAsync(IBrowserFile file);
    }
}
