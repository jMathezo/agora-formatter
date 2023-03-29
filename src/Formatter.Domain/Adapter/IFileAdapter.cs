namespace CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Adapter
{
    public interface IFileAdapter
    {
        /// <summary>
        /// save log file to specified path
        /// </summary>
        /// <param name="file">File content to save</param>
        /// <param name="outPath">path to save</param>
        /// <returns>A completed Task</returns>
        Task SaveFileAsync(string file, string outPath);

        /// <summary>
        /// Get stream content from a url
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<Stream> GetFileStreamAsync(string file);

        /// <summary>
        /// read a stream content and returns a string
        /// </summary>
        /// <param name="file">file to read</param>
        /// <returns>A string with file content</returns>
        Task<string> ReadFileAsync(Stream file);
    }
}
