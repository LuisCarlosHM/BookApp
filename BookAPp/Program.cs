var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Serve default files like index.html
app.UseDefaultFiles();

// Enable serving static files from wwwroot
app.UseStaticFiles();

// Map the API route
app.MapGet("/books",(int seed, int page, int count, double avgLikes, double avgReviews, string locale) =>
{
    if (page < 1 || count < 1) return Results.BadRequest("Page and count must be positive.");
    if (avgLikes < 0 || avgReviews < 0) return Results.BadRequest("Average likes and reviews must be non-negative.");

    var books = BookGenerator.GenerateBooks(seed, page, count, avgLikes , avgReviews, locale);
    return Results.Ok(books);
});

app.Run();
