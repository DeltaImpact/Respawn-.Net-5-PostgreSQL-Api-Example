using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RespawnCoreApiExample.DataAccess.Contexts;
using RespawnCoreApiExample.DataAccess.Extensions;
using RespawnCoreApiExample.Domain;
using RespawnCoreApiExample.Domain.Db.Entities;

namespace RespawnCoreApiExample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RespawnExampleDbContext>(options =>
                options
                    .UseNpgsql(
                        Configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("RespawnCoreApiExample.Api.DataAccess")
                    )
            );
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "RespawnCoreApiExample.Api", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RespawnExampleDbContext respawnExampleDbContext)
        {
            respawnExampleDbContext.Database.Migrate();
            SeedGenres(respawnExampleDbContext).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RespawnCoreApiExample.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static async Task SeedGenres(RespawnExampleDbContext respawnExampleDbContext)
        {
            foreach (var genre in BookGenres.GetAllGenres())
            {
                if (!await respawnExampleDbContext.Genres.GetByName(genre).AnyAsync())
                {
                    await respawnExampleDbContext.Genres.AddAsync(new Genre {Name = genre});
                }
            }

            await respawnExampleDbContext.SaveChangesAsync();
        }
    }
}