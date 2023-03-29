namespace CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Models
{
    public class Agora
    {
        public Agora(string cacheStatus, 
            string hTTPMethod, 
            int responseSize, 
            int statusCode, 
            int timeTaken,
            string uriPath)
        {
            CacheStatus = cacheStatus;
            HTTPMethod = hTTPMethod;
            ResponseSize = responseSize;
            StatusCode = statusCode;
            TimeTaken = timeTaken;
            UriPath = uriPath;
        }

        public string CacheStatus { get; }
        public string HTTPMethod { get; }
        public string Provider { get; } = "\"Minha CDN\"";
        public int ResponseSize { get; }
        public int StatusCode { get; }
        public int TimeTaken { get; }
        public string UriPath { get; }
    }
}
