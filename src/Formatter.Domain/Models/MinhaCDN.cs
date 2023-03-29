namespace CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Models
{
    public class MinhaCDN
    {
        public MinhaCDN(string cacheStatus, 
            string hTTPMethod, 
            string protocolVersion, 
            int responseSize, 
            int statusCode, 
            decimal timeTaken, 
            string uriPath)
        {
            CacheStatus = cacheStatus;
            HTTPMethod = hTTPMethod;
            ProtocolVersion = protocolVersion;
            ResponseSize = responseSize;
            StatusCode = statusCode;
            TimeTaken = timeTaken;
            UriPath = uriPath;
        }

        public string CacheStatus { get; }
        public string HTTPMethod { get;  }
        public string ProtocolVersion { get;  }
        public int ResponseSize { get;  }
        public int StatusCode { get;  }
        public decimal TimeTaken { get;  }
        public string UriPath { get;  }
    }
}