using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text.Json.Serialization;
using System;
using Microsoft.Extensions.FileProviders;
using DotNetSpa.Api.Routing;
using DotNetSpa.Api.MiddleWare;

namespace DotNetSpa.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var spaConfigurations = new List<SpaRouteConfiguration>()
            {
                new SpaRouteConfiguration("/app-1", "app-1", "http://[::1]:9000/"),
                new SpaRouteConfiguration("/app-2", "app-2", "http://[::1]:9001/"),
            };

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            foreach (var spaConfiguration in spaConfigurations)
                builder.Services.AddSpaStaticFiles(c =>
                {
                    c.RootPath = Path.Combine("wwwRoot", spaConfiguration.RootPath);
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseWhen(ctx => ctx.Request.Path.Value!.StartsWith("/api"), appBuilder =>
            {
                appBuilder.UseEndpoints(endpoints =>
                 {
                     endpoints.MapControllers();
                 });
            });

            foreach (var spaConfiguration in spaConfigurations.OrderByDescending(c => c.RootPath)) // configure root path "/" last
            {
                app.UseWhen(ctx => ctx.Request.Path.Value!.StartsWith(spaConfiguration.RoutePath),
                    spaBuilder =>
                    {
                        spaBuilder.UseSpa(spa =>
                        {
                            spa.Options.SourcePath = spaConfiguration.RootPath;
                            if (app.Environment.IsProduction())
                                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                                {
                                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwRoot", spaConfiguration.RootPath))
                                };

                            if (app.Environment.IsDevelopment())
                                spa.UseProxyToSpaDevelopmentServer(spaConfiguration.DevelopmentServerUrl);
                        });
                    });
            }

            app.Run();
        }
    }
}
