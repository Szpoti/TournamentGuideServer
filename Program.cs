using Microsoft.Extensions.DependencyInjection;
using System.Security.AccessControl;

namespace TournamentGuideServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Dependency injection.
            InjectDependencies(builder);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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

        private static void InjectDependencies(WebApplicationBuilder builder)
        {
            var datadir = Common.GetDataDir();
            PlayerManager playerManager = new(datadir);
            // Inject dependencies here.
            builder.Services.AddSingleton(playerManager);

            builder.Services.AddSingleton(serviceProvider =>
            {
                return new RoundManager(datadir, playerManager);
            });
        }
    }
}