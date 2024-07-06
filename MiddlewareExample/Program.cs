var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.Use(async (context, next) =>
{ 
        await context.Response.WriteAsync("Hello World from the middle component! before next delegate");
        await next();
        await context.Response.WriteAsync("Hello World from the middle component! after next delegate");
});
app.Map("/usingmapbranch", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Map branch logic in the Use method before the next delegate"); 
        await next.Invoke();
        Console.WriteLine("Map branch logic in the Use method after the next delegate"); 
    });
    builder.Run(async context =>
    {
        Console.WriteLine($"Map branch response to the client in the Run method");
        await context.Response.WriteAsync("Hello from the map branch.");
    });
});
app.Run(async context =>
{
    await context.Response.WriteAsync("Writing the response to the client in the Run method of the application!");
    await context.Response.WriteAsync("Hello World from the middle component!");
});

app.MapControllers();

app.Run();
