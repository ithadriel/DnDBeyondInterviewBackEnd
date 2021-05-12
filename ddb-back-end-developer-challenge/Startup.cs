using ddb_back_end_developer_challenge.Adapters.Persistence;
using ddb_back_end_developer_challenge.Adapters.Rest.Models;
using ddb_back_end_developer_challenge.Adapters.Utilities;
using ddb_back_end_developer_challenge.Core.Models;
using ddb_back_end_developer_challenge.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ddb_back_end_developer_challenge
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
            services.AddDbContext<CharacterDbContext>(options =>
                options.UseInMemoryDatabase("DomainCharacters")
            );
            services.AddScoped<ICharacterRepositoryAdapter, CharacterRepositoryAdapter>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title="DnD Beyond Back End Challenge", Version="1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ddb_back_end_developer_challenge v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedDatabase(app);
        }

        public static void SeedDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                ICharacterRepositoryAdapter adapter = serviceScope.ServiceProvider.GetService<ICharacterRepositoryAdapter>();
                using (StreamReader r = new StreamReader("briv.json"))
                {
                    string json = r.ReadToEnd();
                    Character character = JsonConvert.DeserializeObject<Character>(json);
                    DomainCharacter initialChar = CharacterConverter.ToDomain(character);
                    adapter.SaveCharacter(initialChar);
                }
            }
        }
    }
}
