using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RespawnCoreApiExample.Api.Extensions;
using RespawnCoreApiExample.DataAccess.Contexts;
using RespawnCoreApiExample.DataAccess.Extensions;
using RespawnCoreApiExample.Domain.Constants;
using RespawnCoreApiExample.Domain.Models.Entities;

namespace RespawnCoreApiExample.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));
        services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseNpgsql(
                    Configuration.GetConnectionString(nameof(ApplicationDbContext)),
                    b => b.MigrationsAssembly("RespawnCoreApiExample.DataAccess")
                )
        );

        services.AddControllers();
        services.AddSwagger();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext applicationDbContext)
    {
        applicationDbContext.Database.Migrate();
        SeedGenres(applicationDbContext).Wait();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RespawnCoreApiExample.Api v1"));
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }

    private static async Task SeedGenres(ApplicationDbContext applicationDbContext)
    {
        foreach (var genre in BookGenres.GetAllGenres())
        {
            if (!await applicationDbContext.Genres.GetByName(genre).AnyAsync())
            {
                await applicationDbContext.Genres.AddAsync(new Genre { Name = genre });
            }
        }

        await applicationDbContext.SaveChangesAsync();
    }
}