using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Adapter;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Exceptions;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Models;
using CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Service;
using Moq;
using System.Text;

namespace AgoraFormatter.Application.UnitTests
{
    public class FormatterServiceTest
    {
        private readonly Mock<IFileAdapter> _mockFileAdapter;
        private readonly FormatterService _formatterService;
        private string _validURL = "https://s3.amazonaws.com/uux-itaas-static/minha-cdn-logs/input-01.txt";
        private string _fileContent = $"312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2\r\n" +
                                      $"199|404|MISS|\"GET /not-found HTTP/1.1\"|142.9";
        public FormatterServiceTest()
        {
            _mockFileAdapter = new Mock<IFileAdapter>();
            _formatterService = new FormatterService(_mockFileAdapter.Object);
        }

        [Fact]
        [Trait(nameof(IFormatterService.GetLogs), "Success")]
        public async Task GetLogs_Without_Parameter_Should_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            var error =
                await Assert.ThrowsAnyAsync<ArgumentNullException>(
                    async () => await _formatterService.GetLogs(null));

            //Assert
            Assert.IsType<ArgumentNullException>(error);
            Assert.Equal("sourceUrl", error.ParamName);
        }

        [Fact]
        [Trait(nameof(IFormatterService.GetLogs), "Success")]
        public async Task GetLogs_With_InvalidURL_Should_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            var error =
                await Assert.ThrowsAnyAsync<InvalidUrlException>(
                    async () => await _formatterService.GetLogs("www.whatever@.com"));

            //Assert
            Assert.IsType<InvalidUrlException>(error);
            Assert.Equal("Invalid URL", error.Message);
        }

        [Fact]
        [Trait(nameof(IFormatterService.FormatMinhaCDN), "Success")]
        public async Task FormatMinhaCDN_Without_minhaCDNLogsParameter_Should_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            var error =
                await Assert.ThrowsAnyAsync<ArgumentNullException>(
                    async () => await _formatterService.FormatMinhaCDN<FormatterServiceTest>(null, "c:"));

            //Assert
            Assert.IsType<ArgumentNullException>(error);
            Assert.Equal("minhaCDNLogs", error.ParamName);
        }

        [Fact]
        [Trait(nameof(IFormatterService.FormatMinhaCDN), "Success")]
        public async Task FormatMinhaCDN_Without_pathToExportParameter_Should_ThrowArgumentNullException()
        {
            //Arrange

            //Act
            var error =
                await Assert.ThrowsAnyAsync<ArgumentNullException>(
                    async () => await _formatterService.FormatMinhaCDN<FormatterServiceTest>(new List<MinhaCDN>(), null));

            //Assert
            Assert.IsType<ArgumentNullException>(error);
            Assert.Equal("pathToExport", error.ParamName);
        }

        [Fact]
        [Trait(nameof(IFormatterService.GetLogs), "Success")]
        public async Task GetLogs_WithValidURL_Should_ReturnListOfMinhaCDN()
        {
            //Arrange
            using var streamFile = new MemoryStream(Encoding.UTF8.GetBytes(_fileContent));

            _mockFileAdapter.Setup(s => s.GetFileStreamAsync(It.IsAny<string>()))
                .ReturnsAsync(streamFile)
                .Verifiable();

            _mockFileAdapter.Setup(s => s.ReadFileAsync(It.IsAny<Stream>()))
                .ReturnsAsync(_fileContent)
                .Verifiable();

            //Act
            var result = await _formatterService.GetLogs(_validURL);

            //Assert
            _mockFileAdapter.Verify(s => s.GetFileStreamAsync(It.IsAny<string>()));
            _mockFileAdapter.Verify(s => s.ReadFileAsync(It.IsAny<Stream>()));
            Assert.Collection(result,
                p1 =>
                {
                    Assert.Equal("HIT", p1.CacheStatus);
                    Assert.Equal("GET", p1.HTTPMethod);
                    Assert.Equal("HTTP/1.1", p1.ProtocolVersion);
                    Assert.Equal(312, p1.ResponseSize);
                    Assert.Equal(200, p1.StatusCode);
                    Assert.Equal(1002, p1.TimeTaken);
                    Assert.Equal("/robots.txt", p1.UriPath);
                },
                p2 =>
                {
                    Assert.Equal("MISS", p2.CacheStatus);
                    Assert.Equal("GET", p2.HTTPMethod);
                    Assert.Equal("HTTP/1.1", p2.ProtocolVersion);
                    Assert.Equal(199, p2.ResponseSize);
                    Assert.Equal(404, p2.StatusCode);
                    Assert.Equal(1429, p2.TimeTaken);
                    Assert.Equal("/not-found", p2.UriPath);
                }
            );
        }

        [Fact]
        [Trait(nameof(IFormatterService.FormatMinhaCDN), "Success")]
        public async Task FormatMinhaCDN_WithValidParameters_Should_ReturnTaskCompleted()
        {
            //Arrange
            var minhaCDN = new List<MinhaCDN>()
            {
                new MinhaCDN(cacheStatus:"HIT", hTTPMethod:"GET",protocolVersion:"HTTP/1.1",responseSize:312, statusCode:200, timeTaken:1002,uriPath:"/robots.txt" ),
                new MinhaCDN(cacheStatus:"MISS", hTTPMethod:"POST",protocolVersion:"HTTP/1.1",responseSize:101, statusCode:200, timeTaken:3194,uriPath:"/myImages" )
            };

            _mockFileAdapter.Setup(s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            //Act
            await _formatterService.FormatMinhaCDN<FormatterServiceTest>(minhaCDN, "c:/output/minhaCdn1.txt");

            //Assert
            _mockFileAdapter.Verify(s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}