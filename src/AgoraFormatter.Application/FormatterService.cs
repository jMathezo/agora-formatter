using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Adapter;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Exceptions;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Models;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Service;
using System.Globalization;
using System.Text;

namespace AgoraFormatter.Application
{
    public class FormatterService : IFormatterService
    {
        private string _appVersion = "";
        private readonly IFileAdapter _fileAdapter;

        public FormatterService(IFileAdapter fileService)
        {
            _fileAdapter = fileService ??
                throw new ArgumentNullException(nameof(fileService));
        }

        public async Task<List<MinhaCDN>> GetLogs(string sourceUrl)
        {
            if (string.IsNullOrWhiteSpace(sourceUrl))
                throw new ArgumentNullException(nameof(sourceUrl));

            InvalidUrlException.ThrowIfInvalid(sourceUrl);

            var fileStream = await _fileAdapter.GetFileStreamAsync(sourceUrl);
            var logFile = await _fileAdapter.ReadFileAsync(fileStream);

            var lines = ConvertFileInLogList(logFile);

            var logs = new List<MinhaCDN>();
            foreach (var line in lines)
            {
                var values = SplitValuesInColumns(line);

                var log = new MinhaCDN(
                    cacheStatus: values[2],
                    hTTPMethod: values[3],
                    protocolVersion: values[5],
                    responseSize: Int16.Parse(values[0]),
                    statusCode: Int16.Parse(values[1]),
                    timeTaken: Math.Truncate(Decimal.Parse(values[6], CultureInfo.InvariantCulture)),
                    uriPath: values[4]
                );

                logs.Add(log);
            }

            return logs;
        }

        public async Task FormatMinhaCDN<T>(List<MinhaCDN> minhaCDNLogs, string pathToExport)
        {
            if (minhaCDNLogs == null)
                throw new ArgumentNullException(nameof(minhaCDNLogs));

            if (string.IsNullOrWhiteSpace(pathToExport))
                throw new ArgumentNullException(nameof(pathToExport));

            _appVersion = GetAssemblyVersion<T>();
            var agoraLogs = ConvertMinhaCDNLogsToAgoraLogs(minhaCDNLogs);
            var newfile = WriteAgoraData(agoraLogs);
            await _fileAdapter.SaveFileAsync(newfile, pathToExport);
        }

        #region .: Private Methods :.
        private List<string> SplitValuesInColumns(string line)
        {
            var values = line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            var fields = new List<string>();
            foreach (var value in values)
            {
                fields.Add(value);
            }
            return fields;
        }

        private List<string> ConvertFileInLogList(string file)
        {
            var logLines = new List<string>();
            file = file.Replace("\"", "").Replace(" ", "|");
            string[] lines = file.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                logLines.Add(line);
            }
            return logLines;
        }

        private string GetAssemblyVersion<T>()
        {
            var version = typeof(T).Assembly.GetName().Version;
            return $"{version.Major}.{version.Minor}";
        }

        private List<Agora> ConvertMinhaCDNLogsToAgoraLogs(List<MinhaCDN> minhaCDNLogs)
        {
            var agoraLogs = new List<Agora>();

            foreach (var log in minhaCDNLogs)
            {
                var agora = new Agora(
                    cacheStatus: log.CacheStatus,
                    hTTPMethod: log.HTTPMethod,
                    responseSize: log.ResponseSize,
                    statusCode: log.StatusCode,
                    timeTaken: (int)log.TimeTaken,
                    uriPath: log.UriPath
                );

                agoraLogs.Add(agora);
            }
            return agoraLogs;
        }

        private string WriteAgoraData(List<Agora> agoraLogs)
        {
            var currentTime = DateTime.UtcNow.ToString("dd/MM/yyyy H:mm:ss");

            string file = $"#Version: {_appVersion}\n";
            file += $"#Date: {currentTime}\n";
            file += "#Fields: provider http-method status-code uri-path time-taken response-size cache-status\n";

            foreach (var log in agoraLogs)
            {
                var line = $"{log.Provider}\t{log.HTTPMethod}\t{log.StatusCode}\t{log.UriPath}\t{log.TimeTaken}\t{log.ResponseSize}\t{log.CacheStatus}\n";
                file += line;
            }

            return file;
        }
        #endregion
    }
}
