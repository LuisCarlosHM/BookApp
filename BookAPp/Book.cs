#nullable disable
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

public static class BookGenerator
{
    public static IEnumerable<Book> GenerateBooks(int seed, int page, int count, double avgLikes, double avgReviews, string locale = "en")
    {
        // Combine user seed with page number for reproducibility
        var fakerSeed = seed + page;

        // Configure Faker with locale and seed
        Randomizer.Seed = new Random(fakerSeed);
        var faker = new Faker(locale);

        // Generate books
        return Enumerable.Range(1, count).Select(index =>
        {
            var generatedLikes = GenerateFakeReviews(faker, avgLikes);
            var reviews = GenerateFakeReviews(faker, avgReviews);

            return new Book
            {
                Index = (page - 1) * count + index,
                ISBN = faker.Random.ReplaceNumbers("##########"), // Generate 10-digit ISBN
                Title = faker.Lorem.Sentence(1, 2),               // Generate realistic book titles
                Authors = faker.Name.FullName(),                  // Generate author names
                Publisher = faker.Company.CompanyName(),          // Generate publisher names
                Likes = generatedLikes,
                Reviews = reviews,                                // Store the number of fake reviews (as a decimal)
                FakeReviews = GenerateFakeReviewsList(faker, reviews), // Generate the actual fake review list
                Description = faker.Lorem.Paragraph(3),           // Generate a short description for the book
                ReleaseYear = faker.Date.Past(20).Year,           // Generate a release year from the past 20 years
                CoverImageUrl = faker.Image.PicsumUrl()           // Generate a random image URL
            };
        });
    }

    // Generate a random number of reviews based on avgReviews, handling fractional reviews
    private static double GenerateFakeReviews(Faker faker, double avgReviews)
    {
        // Separate integer and fractional parts of avgReviews
        double fractionalPart = avgReviews % 1;
        int baseReviews = (int)Math.Floor(avgReviews);

        // Add an extra review with probability = fractional part
        if (faker.Random.Double() < fractionalPart)
            baseReviews++;  // Increment base reviews by 1 with probability equal to fractional part

        return baseReviews;
    }

    // Generate the list of fake reviews (based on the integer part of reviews)
    private static List<string> GenerateFakeReviewsList(Faker faker, double reviews)
    {
        var reviewCount = (int)Math.Floor(reviews); // Take the integer part of the review value

        // Generate a list of fake reviews based on the review count
        return Enumerable.Range(1, reviewCount).Select(_ => faker.Lorem.Sentence()).ToList();
    }
}

// Define the Book model with additional details
public class Book
{
    public int Index { get; set; }
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Authors { get; set; }
    public string Publisher { get; set; }
    public double Likes { get; set; }  // Likes as a decimal between 0 and 10
    public double Reviews { get; set; }  // Reviews as a decimal between 0 and 5
    public List<string> FakeReviews { get; set; }  // Array of fake reviews
    public string Description { get; set; }
    public int ReleaseYear { get; set; }
    public string CoverImageUrl { get; set; } // URL for book cover image
}
