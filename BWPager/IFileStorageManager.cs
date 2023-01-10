namespace BWShared
{
    public interface IFileStorageManager
    {
        /// <summary>
        /// Flie(Blob) Upload
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        /// <param name="overwrite"></param>
        /// <returns>New FileName</returns>
        Task<string> UploadAsync(byte[] bytes, string fileName, string folderPath, bool overwrite);
        Task<string> UploadAsync(Stream stream, string fileName, string folderPath, bool overwrite);   

        /// <summary>
        /// Flie(Blob) Download
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        /// <returns>File or Blob</returns>
        Task<byte[]> DownloadAsync(string fileName, string folderPath);

        /// <summary>
        /// File(Blob) Delete
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folderPath"></param>
        /// <returns>true or false</returns>
        Task<bool> DeleteAsync(string fileName, string folderPath);

        /// <summary>
        /// Get Sub Folder with string
        /// </summary>
        string GetFolderPath(string ownerType, string ownerId, string fileType);

        /// <summary>
        /// Get Sub Folder with long
        /// </summary>
        string GetFolderPath(string ownerType, long ownerId, string fileType);

        /// <summary>
        /// Get Sub Folder with int
        /// </summary>
        string GetFolderPath(string ownerType, int ownerId, string fileType);
    }
}
