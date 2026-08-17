using Microsoft.Extensions.Caching.Memory;
using PharmaPrice.Providers;

namespace PharmaPrice.Services;

/// <summary>
/// ყველა აფთიაქს ერთდროულად ეკითხება, ფილტრავს რელევანტობით, აჯგუფებს
/// ერთი და იგივე წამალს და ალაგებს იაფიდან. შედეგებს ხანმოკლედ ქეშავს.
/// </summary>
public class PriceSearchService
{
    private readonly IEnumerable<IPharmacyPriceProvider> _providers;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PriceSearchService> _logger;
    private static readonly TimeSpan CacheFor = TimeSpan.FromMinutes(10);

    public PriceSearchService(
        IEnumerable<IPharmacyPriceProvider> providers,
        IMemoryCache cache,
        ILogger<PriceSearchService> logger)
    {
        _providers = providers;
        _cache = cache;
        _logger = logger;
    }

    public async Task<IReadOnlyList<MedicineResult>> SearchAsync(string query, CancellationToken ct)
    {
        query = query.Trim();
        if (query.Length < 2) return Array.Empty<MedicineResult>();

        var cacheKey = "search:" + query.ToLowerInvariant();
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<MedicineResult>? cached) && cached is not null)
            return cached;

        // ყველა აფთიაქს პარალელურად ვეკითხებით; ერთის შეცდომა დანარჩენს არ აჩერებს.
        var tasks = _providers.Select(p => SafeSearchAsync(p, query, ct));
        var results = await Task.WhenAll(tasks);

        var relevant = results
            .SelectMany(r => r)
            .Where(o => MedicineMatcher.IsRelevant(query, o.ProductName))
            .ToList();

        var grouped = relevant
            .GroupBy(o => MedicineMatcher.GroupKey(o.ProductName))
            .Select(BuildResult)
            .OrderBy(r => r.CheapestPrice)
            .ToList();

        _cache.Set(cacheKey, (IReadOnlyList<MedicineResult>)grouped, CacheFor);
        return grouped;
    }

    /// <summary>ერთი ჯგუფიდან (ერთი წამალი) აწყობს შესადარებელ შედეგს.</summary>
    private static MedicineResult BuildResult(IGrouping<string, PriceOffer> group)
    {
        // თითო აფთიაქზე ვტოვებთ ყველაზე იაფ ვარიანტს.
        var offers = group
            .GroupBy(o => o.Pharmacy)
            .Select(byPharmacy => byPharmacy.OrderBy(o => o.Price).First())
            .Select(o => new PharmacyPrice
            {
                Pharmacy = o.Pharmacy,
                Price = o.Price,
                OldPrice = o.OldPrice,
                Url = o.Url,
            })
            .OrderBy(p => p.Price)
            .ToList();

        offers[0].IsCheapest = true;

        // საჩვენებელ სახელად ვირჩევთ ყველაზე მოკლეს (ჩვეულებრივ ყველაზე სუფთა ქართული).
        var displayName = group
            .OrderBy(o => o.ProductName.Length)
            .First().ProductName;

        var image = group
            .Select(o => o.ImageUrl)
            .FirstOrDefault(u => !string.IsNullOrEmpty(u));

        return new MedicineResult
        {
            Name = displayName,
            ImageUrl = image,
            Offers = offers,
            CheapestPrice = offers[0].Price,
        };
    }

    private async Task<IReadOnlyList<PriceOffer>> SafeSearchAsync(
        IPharmacyPriceProvider provider, string query, CancellationToken ct)
    {
        try
        {
            return await provider.SearchAsync(query, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "აფთიაქიდან {Pharmacy} ვერ მოვიტანე მონაცემები", provider.PharmacyName);
            return Array.Empty<PriceOffer>();
        }
    }
}
