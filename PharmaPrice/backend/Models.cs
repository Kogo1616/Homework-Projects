namespace PharmaPrice;

/// <summary>ერთი შეთავაზება კონკრეტული აფთიაქიდან (provider-ის დონეზე).</summary>
public record PriceOffer
{
    public required string Pharmacy { get; init; }
    public required string ProductName { get; init; }
    public required decimal Price { get; init; }
    public decimal? OldPrice { get; init; }
    public bool InStock { get; init; } = true;
    public string? Url { get; init; }
    public string? ImageUrl { get; init; }
}

/// <summary>ერთი წამლის ფასი კონკრეტულ აფთიაქში (დაჯგუფებულ შედეგში).</summary>
public record PharmacyPrice
{
    public required string Pharmacy { get; init; }
    public required decimal Price { get; init; }
    public decimal? OldPrice { get; init; }
    public string? Url { get; init; }

    /// <summary>ნიშანი, რომ ეს არის ამ წამლის ყველაზე იაფი აფთიაქი.</summary>
    public bool IsCheapest { get; set; }
}

/// <summary>
/// ერთი წამალი (რამდენიმე აფთიაქიდან დაჯგუფებული) — API-ს პასუხის ერთეული.
/// </summary>
public record MedicineResult
{
    public required string Name { get; init; }
    public string? ImageUrl { get; init; }

    /// <summary>აფთიაქების ფასები, დალაგებული იაფიდან.</summary>
    public required IReadOnlyList<PharmacyPrice> Offers { get; init; }

    public decimal CheapestPrice { get; init; }
}
