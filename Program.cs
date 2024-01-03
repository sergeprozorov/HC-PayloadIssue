var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType()
    .AddMutationConventions()
    .AddTypeExtension<ProblematicMutation>();

var app = builder.Build();

app.MapGraphQL();

app.Run();

[MutationType]
public class ProblematicMutation
{
    public async Task<MutationResult<Author[], CustomError>> DoSomeWorkProperlyAsync(
        string[] authorNames,
        CancellationToken token) =>
        await Task.FromResult(
            authorNames.Select(n => new Author
            {
                Name = n
            }).ToArray());
}

public class CustomError
{
    public string Message { get; }
}

public class Book
{
    public string Title { get; set; }

    public Author Author { get; set; }
}

public class Author
{
    public string Name { get; set; }
}

public class Query
{
    public Book GetBook() =>
        new Book
        {
            Title = "C# in depth.",
            Author = new Author
            {
                Name = "Jon Skeet"
            }
        };
}