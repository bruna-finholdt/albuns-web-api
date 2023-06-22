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


//conceito code first

//comando para migrations:
//comando: Add-Migration Migration00 -Context AppDbContext -OutputDir DAL/Migrations
//esse comando irá gerar o arquivo da migration dentro da pasta DAL/Migrations
//até aqui só geramos a migration mas ainda não a executamos no banco de dados
//para executar, utilizar esse comando:
//comando: Update-Database  -Context AppDbContext

//comando para gerar a nova migration referente à classe Avaliações:
//comando: Add-Migration Migration01 -Context AppDbContext -OutputDir DAL/Migrations
//coments do curso desse momento: 
//"O projeto será compilado e a classe Migration01 que herda de Migration será criada, note que neste arquivo
//foram criados dois métodos : Up e Down. No método UP temos os comandos para criar a tabela e colunas com
//suas configurações. No método Down temos o comando para remover a tabela criada caso desejamos remover a
//migração". 
//"Assim, se você cometeu algum erro e deseja remover a migração basta digitar: Remove-Migration o método
//Down será executado. 
//Depois de criar os arquivos de migração com sucesso, precisamos aplicá-las que elas gere nossas tabelas e
//colunas no banco de dados. Para isso ainda no Package manager Console insira o comando update-database
//Ao final do processamento teremos as tabelas criadas refletindo nossa entidade.
//comando: update-database