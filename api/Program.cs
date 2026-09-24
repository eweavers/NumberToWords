var builder = WebApplication.CreateBuilder(args);

// frontend is hosted on GitHub Pages, so it's a different origin - needs CORS open to call this
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowFrontend");

const decimal MaxSupported = 999_999_999_999_999_999m; // just under 10^18

app.MapGet("/api/convert", (string? number) =>
{
    if (string.IsNullOrWhiteSpace(number))
        return Results.BadRequest(new { error = "Enter a number to convert." });

    if (!decimal.TryParse(number, out var value))
        return Results.BadRequest(new { error = "That doesn't look like a valid number." });

    if (Math.Abs(value) > MaxSupported)
        return Results.BadRequest(new { error = "That number is too large to convert." });

    var words = NumberToWordsConverter.ConvertWithDecimals(value);

    return Results.Ok(new { input = number, words });
});

app.MapGet("/", () => Results.Text("NumberToWords API is running. Try /api/convert?number=1234.56"));

app.Run();