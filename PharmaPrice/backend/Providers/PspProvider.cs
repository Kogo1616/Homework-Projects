using System.Text;
using System.Text.Json;

namespace PharmaPrice.Providers;

/// <summary>
/// PSP — იყენებს Magento GraphQL API-ს (app.psp.ge/graphql).
/// ეს ყველაზე სუფთა წყაროა: სტრუქტურირებული JSON ფასებით.
/// </summary>
public class PspProvider : IPharmacyPriceProvider
{
    public string PharmacyName => "PSP";

    private const string Endpoint = "https://app.psp.ge/graphql";
    private const string Query =
        "query($s:String){products(search:$s,pageSize:20){items{" +
        "name sku stock_status url_key " +
        "price_range{minimum_price{final_price{value} regular_price{value}}} " +
        "thumbnail{url}}}}";

    private readonly HttpClient _http;

    public PspProvider(HttpClient http) => _http = http;

    public async Task<IReadOnlyList<PriceOffer>> SearchAsync(string query, CancellationToken ct)
    {
        var body = JsonSerializer.Serialize(new { query = Query, variables = new { s = query } });
        using var req = new HttpRequestMessage(HttpMethod.Post, Endpoint)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json"),
        };
        req.Headers.Add("Store", "ka");

        using var res = await _http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode();

        await using var stream = await res.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);

        var offers = new List<PriceOffer>();
        if (!doc.RootElement.TryGetProperty("data", out var data) ||
            !data.TryGetProperty("products", out var products) ||
            !products.TryGetProperty("items", out var items))
            return offers;

        foreach (var item in items.EnumerateArray())
        {
            var name = item.GetProperty("name").GetString();
            if (string.IsNullOrWhiteSpace(name)) continue;

            var minPrice = item.GetProperty("price_range").GetProperty("minimum_price");
            var final = minPrice.GetProperty("final_price").GetProperty("value").GetDecimal();
            var regular = minPrice.GetProperty("regular_price").GetProperty("value").GetDecimal();

            var inStock = item.TryGetProperty("stock_status", out var s) &&
                          s.GetString() == "IN_STOCK";
            var urlKey = item.TryGetProperty("url_key", out var uk) ? uk.GetString() : null;
            var thumb = item.TryGetProperty("thumbnail", out var t) &&
                        t.TryGetProperty("url", out var tu) ? tu.GetString() : null;

            offers.Add(new PriceOffer
            {
                Pharmacy = PharmacyName,
                ProductName = name,
                Price = final,
                OldPrice = regular > final ? regular : null,
                InStock = inStock,
                Url = urlKey is null ? "https://psp.ge" : $"https://psp.ge/{urlKey}",
                ImageUrl = thumb,
            });
        }

        return offers;
    }
}
