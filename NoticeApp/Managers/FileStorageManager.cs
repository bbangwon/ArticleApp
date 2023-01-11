using BWShared;

namespace NoticeApp.Managers
{
    public class FileStorageManager : IFileStorageManager
    {
        public async Task<bool> DeleteAsync(string fileName, string folderPath)
        {
            var path = Path.Combine(folderPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return await Task.FromResult(true);
        }

        public async Task<byte[]?> DownloadAsync(string fileName, string folderPath)
        {
            var path = Path.Combine(folderPath, fileName);
            if(File.Exists(path))
            {
                byte[] bytes= await File.ReadAllBytesAsync(path);
                return bytes;
            }
            return null;
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
            var path = Path.Combine(folderPath, fileName);
            await File.WriteAllBytesAsync(path, bytes);
            return fileName;
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string folderPath, bool overwrite)
        {
            var path = Path.Combine(folderPath, fileName);
            using var fileStream = new FileStream(path, FileMode.Create);
            await stream.CopyToAsync(fileStream);
            return fileName;
        }
    }
}
