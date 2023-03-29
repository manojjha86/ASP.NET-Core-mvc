using Routing.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("alphanumeric", typeof(AlphaNumericConstraint));
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoint =>
{
    endpoint.MapGet("/products/{id:int:range(10,1000)}", async (context) =>
    {
        var id = context.Request.RouteValues["ID"];

        if(id != null)
        {
            id = Convert.ToInt32(id);
            await context.Response.WriteAsync("This is product with ID " + id);
        }
        else
        {
            await context.Response.WriteAsync("You are in the products page!");
        }
    });

    endpoint.MapGet("/books/author/{authorname:alpha:length(8)}/{bookid?}", async (context) =>
    {
        var BookId = context.Request.RouteValues["bookid"];
        var AuthorName = Convert.ToString(context.Request.RouteValues["authorname"]);

        if(BookId != null)
        {
            await context.Response.WriteAsync($"This is the book authored by {AuthorName} and book ID is {BookId}");
        }
        else
        {
            await context.Response.WriteAsync($"Foolowing are the list of books authored by {AuthorName}.");
        }
    });

    endpoint.MapGet("/quaterly-reports/{year:int:min(1999):minlength(4)}/{month}", async (context) =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        if(month == "mar" || month == "jun" || month == "sep" || month == "dec")
        {
            await context.Response.WriteAsync($"This is the quaterly report for {year}-{month}");
        }
        else
        {
            await context.Response.WriteAsync($"The month value {month} is not valid");
        }
        
    });

    endpoint.MapGet("/monthly-reports/{month:regex(^([1-9]|1[012])$)}", async (context) =>
    {
        int monthNumber = Convert.ToInt32(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"This is the monthly report for month number {monthNumber}");
    });

    //YYYY-MM-DD / YYYY.MM.DD / YYYY/MM/DD - 1900-01-01 to 2099-12-31
    endpoint.MapGet("/daily-reports/{date:regex(^(19|20)\\d\\d[- /.](0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])$)}", async (context) =>
    {
        string? date = Convert.ToString(context.Request.RouteValues["date"]);
        await context.Response.WriteAsync($"This is the daily report for {date}");
    });

    // - /user/manojjha10
    endpoint.MapGet("/user/{username:alphanumeric}", async (context) =>
    {
        string? username = Convert.ToString(context.Request.RouteValues["username"]);
        await context.Response.WriteAsync($"Welcome {username}");
    });
});

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("The URL which you are looking for is not found!");
});

app.Run();
