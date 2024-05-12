using System.Globalization;
using HtmlAgilityPack;

namespace Girteka.ElectricAggregate.Integrations;

public class HTTPLink
{
    private const string FileUrlPrefix = "https://data.gov.lt";
    private const string WebsiteUrl =
        "https://data.gov.lt/dataset/siame-duomenu-rinkinyje-pateikiami-atsitiktinai-parinktu-1000-buitiniu-vartotoju-automatizuotos-apskaitos-elektriniu-valandiniai-duomenys";

    private readonly HttpClient _httpClient;

    public HTTPLink()
    {
        _httpClient = new HttpClient();
    }

    private string GetFileNameFromUrl(string fileUrl)
    {
        return Path.GetFileName(fileUrl);
    }

    public List<string> DownloadLastNPeriodFiles(int? numberOfYears = null, int? numberOfMonths = null)
    {
        // Get the HTML content of the website
        string htmlContent = _httpClient.GetStringAsync(WebsiteUrl).Result;

        // Parse the HTML content using HtmlAgilityPack
        HtmlDocument document = new HtmlDocument();
        document.LoadHtml(htmlContent);

        // Find the download links for the files
        var fileLinks = document.DocumentNode.Descendants("a")
            .Where(a => a.Attributes.Contains("href"))
            .Select(a => a.Attributes["href"].Value)
            .Where(link => link.EndsWith(".csv"))
            .ToList();

        // Filter and select the links for the last N period files
        var lastNPeriodFiles = GetLastNPeriodFiles(fileLinks, numberOfYears, numberOfMonths);
        lastNPeriodFiles = lastNPeriodFiles.Select(link => FileUrlPrefix + link).ToList();

        return lastNPeriodFiles;
    }

    private List<string> GetLastNPeriodFiles(List<string> fileLinks, int? numberOfYears, int? numberOfMonths)
    {
        // Get the current date
        DateTime currentDate = DateTime.Now;

        // Calculate the start date for the last N period
        DateTime startDate = currentDate;
        if (numberOfYears.HasValue)
            startDate = startDate.AddYears(-numberOfYears.Value);
        if (numberOfMonths.HasValue)
            startDate = startDate.AddMonths(-numberOfMonths.Value);

        // Filter and select the file links for the last N period files
        var lastNPeriodFiles = fileLinks.Skip(1)
            .Where(link =>
            {
                string fileName = GetFileNameFromUrl(link);
                var fileDate = link.Split('/').Last().Split('.').First();

                DateTime result = DateTime.ParseExact(fileDate.Substring(0, fileName.LastIndexOf(".")), "yyyy-MM",
                    CultureInfo.InvariantCulture);

                return (result >= startDate && result <= currentDate);
            })
            .ToList();

        return lastNPeriodFiles;
    }
}