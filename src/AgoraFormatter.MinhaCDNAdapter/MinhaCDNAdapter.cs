using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Adapter;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Models;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Service;

namespace AgoraFormatter.MinhaCDNAdapter
{
    public class MinhaCDNAdapter : IMinhaCDNAdapter
    {
        private readonly IFileService _fileService;

        public MinhaCDNAdapter(IFileService fileService)
        {
            _fileService = fileService ??
                throw new ArgumentNullException(nameof(fileService));
        }

        public async Task<List<MinhaCDN>> GetLogs(string sourceUrl)
        {
            var fileStream = await _fileService.GetFileStreamAsync(sourceUrl);
            var logFile = await _fileService.ReadFileAsync(fileStream);

            var lines = ListLogLines(logFile);

            var logs = new List<MinhaCDN>();
            foreach (var line in lines)
            {
                var values = SplitColumnValues(line);

                var log = new MinhaCDN(
                    cacheStatus: values[2],
                    hTTPMethod: values[3],
                    protocolVersion: values[5],
                    responseSize: Int16.Parse(values[0]),
                    statusCode: Int16.Parse(values[1]),
                    timeTaken: Decimal.Parse(values[6]),
                    uriPath: values[4]
                );

                logs.Add(log);
            }
            return logs;
        }

        
    }
}

