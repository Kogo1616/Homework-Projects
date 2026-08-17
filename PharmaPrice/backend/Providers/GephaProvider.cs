using System.Globalization;
using System.Text.Json;

namespace PharmaPrice.Providers;

/// <summary>
/// Gepha-ს პლატფორმა (GPC და ფარმადეპო). ორივე Next.js-ია და პროდუქტების
/// მონაცემები გვერდის HTML-ში embedded JSON-ად ("search_result") ზის.
/// ერთი კლასი ორივე აფთიაქს ემსახურება — მხოლოდ baseUrl განსხვავდება.
/// </summary>
public class GephaProvider : IPharmacyPriceProvider
{
    public string PharmacyName { get; }
    private readonly string _baseUrl;
    private readonly HttpClient _http;

    public GephaProvider(string pharmacyName, string baseUrl, HttpClient http)
    {
        PharmacyName = pharmacyName;
        _baseUrl = baseUrl.TrimEnd('/');
        _http = http;
    }

    public async Task<IReadOnlyList<PriceOffer>> SearchAsync(string query, CancellationToken ct)
    {
        var url = $"{_baseUrl}/ka/search?keyword={Uri.EscapeDataString(query)}";
        var html = await _http.GetStringAsync(url, ct);

        var json = ExtractSearchResultJson(html);
        if (json is null) return Array.Empty<PriceOffer>();

        using var doc = JsonDocument.Parse(json);
        var offers = new List<PriceOffer>();

        foreach (var item in doc.RootElement.EnumerateArray())
        {
            var name = GetString(item, "full_name") ?? GetString(item, "name");
            if (string.IsNullOrWhiteSpace(name)) continue;
            if (!TryGetDecimal(item, "price", out var price)) continue;

            decimal? oldPrice = null;
            if (TryGetDecimal(item, "initial_price", out var initial) && initial > price)
                oldPrice = initial;

            offers.Add(new PriceOffer
            {
                Pharmacy = PharmacyName,
                ProductName = name!,
                Price = price,
                OldPrice = oldPrice,
                InStock = true,
                Url = $"{_baseUrl}/ka/search?keyword={Uri.EscapeDataString(name!)}",
                ImageUrl = GetString(item, "image_url"),
            });
        }

        return offers;
    }

    /// <summary>
    /// HTML-ში პოულობს Next.js-ის embedded "search_result" მასივს და
    /// აბრუნებს გასუფთავებულ (unescaped) JSON-ს. null — თუ ვერ იპოვა.
    /// </summary>
    private static string? ExtractSearchResultJson(string html)
    {
        // RSC-ში JSON JS-სტრიქონშია, ამიტომ ბრჭყალები ასეა: \"search_result\":
        const string marker = "\\\"search_result\\\":";
        var start = html.IndexOf(marker, StringComparison.Ordinal);
        if (start < 0) return null;

        start += marker.Length;
        while (start < html.Length && html[start] != '[') start++;
        if (start >= html.Length) return null;

        // მასივის დახურვის პოვნა \" -ს (სტრიქონის საზღვრის) გათვალისწინებით.
        int depth = 0, end = -1;
        bool inStr = false;
        for (var i = start; i < html.Length; i++)
        {
            var c = html[i];
            if (c == '\\' && i + 1 < html.Length && html[i + 1] == '"')
            {
                inStr = !inStr;
                i++;
                continue;
            }
            if (inStr) continue;
            if (c == '[') depth++;
            else if (c == ']')
            {
                depth--;
                if (depth == 0) { end = i + 1; break; }
            }
        }
        if (end < 0) return null;

        var esc = html.Substring(start, end - start);
        return esc.Replace("\\\"", "\"").Replace("\\\\", "\\").Replace("\\n", " ");
    }

    private static string? GetString(JsonElement el, string prop) =>
        el.TryGetProperty(prop, out var v) && v.ValueKind == JsonValueKind.String
            ? v.GetString()
            : null;

    private static bool TryGetDecimal(JsonElement el, string prop, out decimal value)
    {
        value = 0;
        if (!el.TryGetProperty(prop, out var v)) return false;
        if (v.ValueKind == JsonValueKind.Number) { value = v.GetDecimal(); return true; }
        if (v.ValueKind == JsonValueKind.String)
            return decimal.TryParse(v.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        return false;
    }
}
