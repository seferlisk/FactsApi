
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

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("ServiceSettings"));

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IDogFactsService, DogFactsService>();
            builder.Services.AddScoped<ICatFactsService, CatFactsService>();
            builder.Services.AddScoped<INinjaFactsService, NinjaFactsService>();
            builder.Services.AddScoped<IFactsAggregateService, FactsAggregateService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
