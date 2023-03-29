using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Adapter;
using System.Text;

namespace CandidateTesting.JoaoMatheus.AgoraFormatter.FileAdapter
{
    public class FileAdapter : IFileAdapter
    {
        const int bufferSize = 4096;

        public async Task<Stream> GetFileStreamAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl)) throw new ArgumentNullException(nameof(fileUrl));
            
            try
            {
                using var webClient = new HttpClient();
                return await webClient.GetStreamAsync(fileUrl);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> ReadFileAsync(Stream file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            try
            {
                using var reader = new StreamReader(file);
                return await reader.ReadToEndAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SaveFileAsync(string file, string outPath)
        {
            if (string.IsNullOrEmpty(file)) throw new ArgumentNullException(nameof(file));
            if (string.IsNullOrEmpty(outPath)) throw new ArgumentNullException(nameof(outPath));

            try
            {
                var folderPath = Path.GetDirectoryName(outPath).ToString();
                Directory.CreateDirectory(folderPath);
                var filename = Path.GetFileName(outPath);
                var fullPath = Path.Combine(folderPath, filename);
                byte[] fileBytes = new UTF8Encoding(true).GetBytes(file);

                using var sourceStream =
                    new FileStream(fullPath, FileMode.Create,
                    FileAccess.Write, FileShare.None,
                    bufferSize, useAsync: true);

                await sourceStream.WriteAsync(fileBytes, 0, fileBytes.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
