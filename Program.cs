using Amazon.S3;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using QrCodeGenerator.Adapters;
using QrCodeGenerator.Ports;

using static QrCodeGenerator.Features.GenerateQRCode;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddScoped<IQRCodeGenerator, QrCodeGeneratorAdpter>();
builder.Services.AddScoped<IStorage, S3Storage>();
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = new AmazonS3Config
    {
        RegionEndpoint = Amazon.RegionEndpoint.USEast1
    };
    return new AmazonS3Client(config);
});

var app = builder.Build();

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/ping", () => Results.Ok("pong"));


app.MapPost("/qrcode", async (IMediator sender, [FromBody] string text) =>
{
    var fileUrl = await sender.Send(new Command(text));
    return Results.Ok(fileUrl);
})
.WithName("GenerateQRCode")
.Produces<string>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest);

app.Run();
