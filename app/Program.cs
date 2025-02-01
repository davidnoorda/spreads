using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

app.MapGet("/", () => Results.Extensions.RazorSlice<Spends.App.Slices.Index, DateTime>(DateTime.Now));

app.MapPost("/upload", async (IFormFile file) =>
{
    using StreamReader reader = new(file.OpenReadStream());
    string csv = await reader.ReadToEndAsync();
    IEnumerable<string> lines = csv.Split('\n').Where(l => l.Contains("PURCHASE AUTHORISATION") == false);
    string cleared = string.Join('\n', lines);
    byte[] bytes = Encoding.UTF8.GetBytes(cleared);
    return Results.File(bytes, "text/csv", "cleared.csv");
}).DisableAntiforgery();

app.Run();
