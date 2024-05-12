using Girteka.ElectricAggregate.Domain;

namespace Girteka.ElectricAggregate.Integrations;

public class CSVFileFromHTTTP : IContext<string, string, Stream>
{
    private readonly HttpClient _client;

    public CSVFileFromHTTTP(HttpClient client)
    {
        _client = client;
    }

    public Stream Do(string localPath, string path)
    {
        Uri uri = new Uri(path);
        var response = _client.GetAsync(uri).Result;

        response.EnsureSuccessStatusCode();

        using var fileStream = new FileStream(localPath + path.Split('/').Last(),
            FileMode.Create);
        var contentStream = response.Content.ReadAsStreamAsync().Result;
        contentStream.CopyToAsync(fileStream);
        return contentStream;
    }
}