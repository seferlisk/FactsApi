
using FactsApi.Services;
using FactsApi.Services.CatFacts;
using FactsApi.Services.DogFacts;
using FactsApi.Services.FactsAggregate;
using FactsApi.Services.NinjaFacts;
using Serilog;

namespace FactsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Configuration.AddUserSecrets<Program>();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();            
          
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<ApiStatisticsService>();

            builder.Services.AddScoped<IDogFactsService, DogFactsService>();
            builder.Services.AddScoped<ICatFactsService, CatFactsService>();
            builder.Services.AddScoped<INinjaFactsService, NinjaFactsService>();
            builder.Services.AddScoped<IFactsAggregateService, FactsAggregateService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy => policy.WithOrigins("http://localhost:4200")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAngular");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
