using BWShared;

namespace NoticeApp.Managers
{
    public class FileStorageManager : IFileStorageManager
    {
        private readonly IWebHostEnvironment environment;

        public FileStorageManager(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public Task<bool> DeleteAsync(string fileName, string folderPath)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadAsync(string fileName, string folderPath)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, string ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, long ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, int ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(byte[] bytes, string fileName, string folderPath, bool overwrite)
        {
            var path = Path.Combine(environment.WebRootPath, "files", fileName);
            await File.WriteAllBytesAsync(path, bytes);
            return fileName;
        }
    }
}
