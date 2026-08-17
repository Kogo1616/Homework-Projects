namespace PharmaPrice.Providers;

/// <summary>
/// თითოეული აფთიაქის მონაცემების წყარო ამ ინტერფეისს ახორციელებს.
/// ახალი აფთიაქის დასამატებლად საკმარისია ახალი კლასის დაწერა.
/// </summary>
public interface IPharmacyPriceProvider
{
    /// <summary>აფთიაქის სახელი (მაგ. "PSP").</summary>
    string PharmacyName { get; }

    /// <summary>ეძებს წამალს და აბრუნებს ფასების შეთავაზებებს.</summary>
    Task<IReadOnlyList<PriceOffer>> SearchAsync(string query, CancellationToken ct);
}
