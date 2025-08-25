using System.Text.Json.Serialization;
using CarAuctionApi.Application.Interfaces;
using CarAuctionApi.Application.Services;
using CarAuctionApi.CrossCutting.Middleware;
using CarAuctionApi.Domain.Entities;
using CarAuctionApi.Domain.Interfaces;
using CarAuctionApi.Infrastructure.Data;
using CarAuctionApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace CarAuctionApi.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            await ConfigurePipeline(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Car Auction API",
                    Version = "v1",
                    Description = "A Car Auction Management System API"
                });
                c.MapType<VehicleType>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = Enum.GetNames(typeof(VehicleType))
                        .Select(n => (IOpenApiAny)new OpenApiString(n))
                        .ToList()
                });
            });

            services.AddDbContext<CarAuctionDbContext>(options =>
                options.UseInMemoryDatabase("CarAuctionDb"));

            services.AddScoped<IVehiclesInventoryRepository, VehiclesInventoryRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();

            services.AddScoped<IVehiclesInventoryService, VehiclesInventoryService>();
            services.AddScoped<IAuctionService, AuctionService>();

            services.AddLogging();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        private static async Task ConfigurePipeline(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Auction API V1");
                    c.RoutePrefix = string.Empty; 
                });
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors();
            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CarAuctionDbContext>();
            if (app.Environment.IsDevelopment())
            {
                await CarAuctionDbSeeder.SeedAsync(context);
            }
        }
    }
}
