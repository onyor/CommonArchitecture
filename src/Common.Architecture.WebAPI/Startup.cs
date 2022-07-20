using Common.Architecture.Core.DependencyResolvers;
using Common.Architecture.Core.Extensions;
using Common.Architecture.Core.Utilities.IoC;
using Common.Architecture.Persistance;
using HRM.WebAPI.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Common.Architecture.WebAPI
{
    public class Startup
    {
        private string ConnectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddAuthenticationWithJwt(Configuration);

            services.AddDependencyResolvers(new ICoreModule[]{
                new CoreModule()
            });

            ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CommonDBContext>(options =>
                options.UseSqlServer(ConnectionString, x => x.MigrationsAssembly("Common.Architecture.Persistance")));

            services.AddIdentityServices(Configuration);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Common.Architecture.WebAPI", Version = "v1" });
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Common.Architecture.WebAPI v1"));
            }

            using (var context = new CommonDBContext(Configuration.GetConnectionString("DefaultConnection")))
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowMVCApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //if (Configuration.GetValue<bool>("MigrateDatabase"))
            //MigrationHelper.MigrateRentACarDBContext(dataContext);
        }
    }
}