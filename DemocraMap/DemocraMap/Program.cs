using DemocraMap.Client.Pages;
using DemocraMap.Components;
using DemocraMap.Services;
using Markdig;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

// Register AiService (server-side)
builder.Services.AddHttpClient<AiService>();

// Register GovDataService (server-side) — now uses IWebHostEnvironment, not HttpClient
builder.Services.AddSingleton<GovDataService>();

var app = builder.Build();

// Debug check — confirm API key is present
Console.WriteLine("OpenAI Key loaded? " +
    !string.IsNullOrEmpty(app.Configuration["OpenAI:ApiKey"]));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// AI endpoint now uses AiService
app.MapPost("/api/analyse", async (AiService aiService, HttpContext context) =>
{
    try
    {
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();

        var result = await aiService.AnalyseDataAsync(body);
        return Results.Content(result, "application/json");
    }
    catch (Exception ex)
    {
        Console.WriteLine("AI ERROR: " + ex.Message);
        return Results.Problem("AI call failed: " + ex.Message);
    }
});

// GovDataService endpoints (CSV-driven)

app.MapGet("/api/indigenous", async (GovDataService service) =>
{
    var data = await service.GetIndigenousRegionsAsync();
    return Results.Json(data);
});

app.MapGet("/api/migrants", async (GovDataService service) =>
{
    var data = await service.GetMigrantRegionsAsync();
    return Results.Json(data);
});

app.MapGet("/api/outreach", async (GovDataService service) =>
{
    var data = await service.GetProposedOutreachAsync();
    return Results.Json(data);
});

app.MapGet("/api/gaps", async (GovDataService service) =>
{
    var data = await service.GetIndigenousEnrolmentGapsAsync();
    return Results.Json(data);
});

app.MapGet("/api/div-enrol-rate", async (GovDataService service) =>
{
    var data = await service.GetDivisionEnrolRatesAsync();
    return Results.Json(data);
});

app.MapGet("/api/migrant-gaps", async (GovDataService service) =>
{
    var data = await service.GetMigrantParticipationGapsAsync();
    return Results.Json(data);
});

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DemocraMap.Client._Imports).Assembly);

app.Run();
