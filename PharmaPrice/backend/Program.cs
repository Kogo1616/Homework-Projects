using PharmaPrice;
using PharmaPrice.Providers;
using PharmaPrice.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

// React-ს (localhost:5173) ვაძლევთ უფლებას მოგვმართოს (dev-ში).
const string CorsPolicy = "frontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// --- აფთიაქების მონაცემების წყაროები ---
const string UserAgent =
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
    "(KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36";

void ConfigureClient(HttpClient c)
{
    c.Timeout = TimeSpan.FromSeconds(15);
    c.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
}

builder.Services.AddHttpClient("psp", ConfigureClient);
builder.Services.AddHttpClient("gepha", ConfigureClient);

builder.Services.AddSingleton<IPharmacyPriceProvider>(sp =>
    new PspProvider(sp.GetRequiredService<IHttpClientFactory>().CreateClient("psp")));
builder.Services.AddSingleton<IPharmacyPriceProvider>(sp =>
    new GephaProvider("GPC", "https://gpc.ge",
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("gepha")));
builder.Services.AddSingleton<IPharmacyPriceProvider>(sp =>
    new GephaProvider("ფარმადეპო", "https://pharmadepot.ge",
        sp.GetRequiredService<IHttpClientFactory>().CreateClient("gepha")));

builder.Services.AddSingleton<PriceSearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(CorsPolicy);

// React-ის აწყობილი ფაილების გასერვირება (production publish-ში wwwroot-ში ხვდება).
app.UseDefaultFiles();
app.UseStaticFiles();

// აფთიაქების სია (რომლებიც რეალურად ინტეგრირებულია).
app.MapGet("/api/pharmacies", (IEnumerable<IPharmacyPriceProvider> providers) =>
    providers.Select(p => p.PharmacyName));

// წამლის ძებნა — რეალური ფასები ყველა აფთიაქიდან, დალაგებული იაფიდან.
app.MapGet("/api/search", async (string? q, PriceSearchService svc, CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(q))
        return Results.Ok(Array.Empty<PriceOffer>());

    var offers = await svc.SearchAsync(q, ct);
    return Results.Ok(offers);
});

// ნებისმიერი სხვა მისამართი (API-ს გარდა) React-ის აპს გადაეცემა.
app.MapFallbackToFile("index.html");

app.Run();
