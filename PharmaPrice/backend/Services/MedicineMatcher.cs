using System.Text.RegularExpressions;

namespace PharmaPrice.Services;

/// <summary>
/// წამლების რელევანტობის შემოწმება და ერთი და იგივე წამლის დაჯგუფება
/// სხვადასხვა აფთიაქს შორის (ფასების შესადარებლად).
/// </summary>
public static partial class MedicineMatcher
{
    // "ხმაურა" სიტყვები, რომლებიც დაჯგუფებაში არ უნდა მონაწილეობდნენ
    // (ფორმა/შეფუთვა, არა თვით წამლის იდენტობა).
    private static readonly HashSet<string> Noise = new()
    {
        "ტაბლეტი", "ტაბლეტ", "კაფსულა", "კაფსული", "სიროფი", "ფლაკონი",
        "სუსპენზია", "ორალური", "რექტალური", "სანთელი", "სუპოზიტორია",
        "საბავშვო", "ბავშვის", "ბავშვებისთვის", "ფხვნილი", "ხსნარი", "გრანულა",
        "სპრეი", "ცალი", "დრაჟე", "მალამო", "გელი", "წვეთი", "წვეთები",
        "მგ", "მლ", "გ", "მკგ",
    };

    // ≥2 ასოიანი ლათინური = ტრანსლიტერაცია/ბრენდი (მაგ. "Paracetamol", "MIG) —
    // ვაცილებთ. ცალკეული ლათინური ასო (C, D, B) რჩება, რომ ვიტამინები არ აირიოს.
    [GeneratedRegex(@"[a-z]{2,}")]
    private static partial Regex LatinWords();

    [GeneratedRegex(@"[ა-ჰ]+|[a-z]|\d+")]
    private static partial Regex KeyTokens();

    [GeneratedRegex(@"[\s\-–—/]+")]
    private static partial Regex QuerySplit();

    /// <summary>
    /// რელევანტურია თუ არა პროდუქტი მოთხოვნისთვის: მოთხოვნის ყველა
    /// ტექსტური (ასოებიანი) სიტყვა უნდა გვხვდებოდეს პროდუქტის სახელში.
    /// ეს აშორებს ხმაურს (მაგ. "მიგ 400"-ზე შამპუნებს "400 მლ"-ით).
    /// </summary>
    public static bool IsRelevant(string query, string productName)
    {
        var tokens = QuerySplit().Split(query.ToLowerInvariant())
            .Where(t => t.Length > 0)
            .ToArray();

        // ტექსტური სიტყვები (შეიცავს ასოს), მაგ. "მიგ".
        var textTokens = tokens.Where(t => t.Any(char.IsLetter)).ToArray();

        // თუ მოთხოვნა მხოლოდ ციფრებია — ყველა ტოკენი უნდა ემთხვეოდეს.
        var required = textTokens.Length > 0 ? textTokens : tokens;

        var name = productName.ToLowerInvariant();
        return required.All(t => name.Contains(t, StringComparison.Ordinal));
    }

    /// <summary>
    /// დაჯგუფების გასაღები: ლათინური ტრანსლიტერაცია და ხმაურა სიტყვები
    /// მოცილებული, დარჩენილი ქართული სიტყვები + ციფრები დალაგებულად.
    /// ერთი და იგივე წამალი სხვადასხვა აფთიაქში იმავე გასაღებს იძლევა.
    /// </summary>
    public static string GroupKey(string productName)
    {
        var lower = productName.ToLowerInvariant();
        var noLatinWords = LatinWords().Replace(lower, " ");

        var tokens = KeyTokens().Matches(noLatinWords)
            .Select(m => m.Value)
            .Where(t => !Noise.Contains(t))
            .Distinct(StringComparer.Ordinal)          // დუბლიკატები (მაგ. სამი "400") ერთდება
            .OrderBy(t => t, StringComparer.Ordinal)
            .ToList();

        return string.Join("|", tokens);
    }
}
