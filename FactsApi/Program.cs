
using FactsApi.Services;
using FactsApi.Services.CatFacts;
using FactsApi.Services.FactsAggregate;
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

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<ServiceUrls>(builder.Configuration.GetSection("ServiceUrls"));

            builder.Services.AddScoped<ICatFactsService, CatFactsService>();
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
