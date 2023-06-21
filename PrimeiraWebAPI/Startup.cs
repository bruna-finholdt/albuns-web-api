using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PrimeiraWebAPI.DAL;
using Microsoft.OpenApi.Models;
using PrimeiraWebAPI.Services;


namespace PrimeiraWebAPI
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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PrimeiraWebAPI", Version = "v1" });
            });
            //Indicar que AlbunsServices e AvaliacoesService serão usados com injeção de dependência
            services.AddTransient<AlbunsService>();
            services.AddTransient<AvaliacoesService>();

            string connectionString = "Server=.\\SQLExpress;Database=PrimeiraAPI2023;Trusted_Connection=True;TrustServerCertificate=True;";
            // se não estiver usando o SQLExpress tente
            //Server=localhost;Database=PrimeiraAPI;Trusted_Connection=True;
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrimeiraWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}