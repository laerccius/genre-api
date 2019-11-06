using System;
using Consul;
using genre_api.Consul;
using genre_api.Models;
using GenresApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace genre_api
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
            services.AddControllers().AddNewtonsoftJson(options => options.UseCamelCasing(true));

            ConfigureMongoServices(services);

            ConfigureConsulServices(services);

            CondigureDomainService(services);

            ConfigureSwagger(services);
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Genre", Version = "v1" });
            });
        }

        private void CondigureDomainService(IServiceCollection services)
        {
            services.AddSingleton<GenreService>();
        }

        private void ConfigureMongoServices(IServiceCollection services)
        {
            services.Configure<GenresDatabaseSettings>(
                Configuration.GetSection(nameof(GenresDatabaseSettings)));

            services.AddSingleton<IGenresDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<GenresDatabaseSettings>>().Value);


        }

        private void ConfigureConsulServices(IServiceCollection services)
        {
            services.Configure<ConsulConfig>(Configuration.GetSection(nameof(ConsulConfig)));

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = Configuration[$"{nameof(ConsulConfig)}:address"];
                consulConfig.Address = new Uri(address);
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            BsonClassMap.RegisterClassMap<Genre>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Name).SetIsRequired(true).SetElementName("name");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
           {
               c.SwaggerEndpoint("/", "My API V1");
           });
        }
    }
}
