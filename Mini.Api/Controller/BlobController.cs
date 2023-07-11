using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;

namespace Mini.Api.Controller;

public static class BlobController
{
    public static void MapBlobApi(this IEndpointRouteBuilder app)
    {
        app.MapPost("/Tenant", async ([FromBody] TenantInfo tenant, IMultiTenantStore<TenantInfo> store) => await store.TryAddAsync(tenant));

        app.MapGet("/", () => "Hello World!");

        app.MapPost("/upload", async (IFormFile file) =>
        {
            string tempFile = CreateTempFilePath();
            using var stream = File.OpenWrite(tempFile);
            await file.CopyToAsync(stream);
            return tempFile;
            // dom more fancy stuff with the IFormFile
        });
        static string CreateTempFilePath()
        {
            var filename = $"{Guid.NewGuid()}.tmp";
            var directoryPath = Path.Combine("temp", "uploads");
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            return Path.Combine(directoryPath, filename);
        }
    }
}