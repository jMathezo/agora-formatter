using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Models;

namespace CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Service
{
    public interface IFormatterService
    {
        /// <summary>
        /// Get logs from url
        /// </summary>
        /// <param name="sourceUrl">Url to get logs</param>
        /// <returns>A list of MinhaCDN logs</returns>
        Task<List<MinhaCDN>> GetLogs(string sourceUrl);

        /// <summary>
        /// Format MinhaCDNLogs into AgoraLogs and export results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="minhaCDNLogs">Logs to format</param>
        /// <param name="pathToExport">destination path to export</param>
        /// <returns>a txt file into path</returns>
        Task FormatMinhaCDN<T>(List<MinhaCDN> minhaCDNLogs, string pathToExport);
    }
}
