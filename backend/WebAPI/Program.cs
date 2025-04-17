using Amazon;
using Microsoft.AspNetCore.DataProtection;
using Serilog;
using WebAPI.Extensions;
using WebAPI.Hubs;
using WebAPI.Installers.Core;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Required for SignalR
        });
});

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);

if (builder.Environment.IsProduction())
{
    try
    {
        builder.Configuration.AddSecretsManager(
            region: RegionEndpoint.APSoutheast1,
            configurator: options =>
            {
                options.SecretFilter = entry => entry.Name == "myamie/secrets";
                options.KeyGenerator = (secret, key) =>
                {
                    if (!key.StartsWith(secret.Name + ":")) return null;
                    var newKey = key[(secret.Name.Length + 1)..].Replace("__", ":");
                    return newKey;
                };
                options.PollingInterval = null;
            });

        Log.Information("AWS Secrets Manager loaded successfully.");
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Failed to load configuration from AWS Secrets Manager.");
        throw new InvalidOperationException("An error occurred while loading configuration from AWS Secrets Manager. See inner exception for details.", ex);
    }
}

builder.Services.InstallServicesInAssembly(builder.Configuration);

builder.Services.AddSignalR();

var keyDirectory = new DirectoryInfo("/app/DataProtection-Keys");
if (!keyDirectory.Exists)
{
    keyDirectory.Create();
    keyDirectory.Refresh();
}

// Register Data Protection with a persistent storage location
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(keyDirectory)
    .SetApplicationName("MYAmie");

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.ApplyMigration();
}

// Serve static files from wwwroot folder
app.UseStaticFiles(new StaticFileOptions
{
    // Optional: Set cache control headers
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", "public,max-age=604800");
    }
});

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat");

await app.RunAsync();

