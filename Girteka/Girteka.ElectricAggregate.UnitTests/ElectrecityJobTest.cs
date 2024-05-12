using System.Net;
using Girteka.ElectricAggregate.Integrations;
using Moq;
using Moq.Protected;

namespace Girteka.ElectricAggregate.UnitTests;

public class ElectrecityJobTest
{
    private HttpClient _httpClient;
    private CSVFileFromHTTTP _csvFileFromHTTTP;


    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
        _csvFileFromHTTTP = new CSVFileFromHTTTP(_httpClient);
    }

    [Test]
    public void CSVFileFromHTTTPTest()
    {
        // Arrange
        var localPath = "/Users/sabakoghuashvili/Desktop/Temp/";
        var path = "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv";

        var response = new HttpResponseMessage(HttpStatusCode.OK);
        var contentStream = new MemoryStream();
        response.Content = new StreamContent(contentStream);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        var csvFileFromHTTTP = new CSVFileFromHTTTP(httpClient);

        // Act
        var result = csvFileFromHTTTP.Do(localPath, path);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<MemoryStream>(result);
    }

    [Test]
    public void CSVFileFromLocalDiskTest()
    {
        // Arrange
        var fileName = "2022-04.csv";
        var path = "/Users/sabakoghuashvili/Desktop/Temp/";

        var csvFileFromLocalDisk = new CSVFileFromLocalDisk();

        // Act
        var result = csvFileFromLocalDisk.Do(fileName, path);

        // Assert
        Assert.NotNull(result);
        Assert.IsInstanceOf<FileStream>(result);
    }

    [Test]
    public async Task CanDownloadCsvFiles()
    {
        var csvFileUrls = new[]
        {
            "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv",
            "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv",
            "https://data.gov.lt/dataset/1975/download/10764/2022-03.csv",
            "https://data.gov.lt/dataset/1975/download/10763/2022-02.csv"
        };

        foreach (var csvFileUrl in csvFileUrls)
        {
            var response = await _httpClient.GetAsync(csvFileUrl);

            response.EnsureSuccessStatusCode();

            var csvFileContent = await response.Content.ReadAsStringAsync();
            Assert.IsNotEmpty(csvFileContent);
        }
    }

    [TearDown]
    public void Teardown()
    {
        _httpClient.Dispose();
    }
}