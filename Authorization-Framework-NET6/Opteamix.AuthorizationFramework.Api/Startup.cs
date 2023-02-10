using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;
using Opteamix.AuthorizationFramework.Common.BusinessComponent;
using Opteamix.AuthorizationFramework.Common.DataManagers;
using Opteamix.AuthorizationFramework.Common.Interface;
using Opteamix.AuthorizationFramework.Common.Interfaces;
using Opteamix.AuthorizationFramework.Data.Model;
using Opteamix.AuthorizationFramework.Database.DTO;
using Opteamix.AuthorizationFramework.LogService.Interface;
using Opteamix.AuthorizationFramework.LogService.Logger;

namespace Opteamix.AuthorizationFramework.Api
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

            LogManager.Setup().LoadConfigurationFromSection(Configuration);

            //Data context DI
            services.AddDbContext<AuthFrameworkContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //Data manager DI
            services.AddScoped<IModulesDataManager<Module>, ModulesDataManager>();
            services.AddScoped<IClientDataManager<Client>, ClientDataManager>();
            services.AddScoped<IApplicationDataManager<Application>, ApplicationDataManager>();
            services.AddScoped<IPrivilegeDataManager<Privilege>, PrivilegeDataManager>();
            services.AddScoped<IAddRoleDataManager<Role>, AddRoleDataManager>();
            services.AddScoped<IUserDataManager<User>, UserDataManager>();


            //Business component DI
            services.AddScoped<IModulesBiz, ModulesBiz>();
            services.AddScoped<IClientsBiz, ClientBiz>();
            services.AddScoped<IApplicationBiz, ApplicationBiz>();
            services.AddScoped<IPrivilegeBiz, PrivilegeBiz>();
            services.AddScoped<IAddRoleBiz, AddRoleBiz>();
            services.AddScoped<ICommonDataManager, CommonDataManager>();
            services.AddScoped<IExcelBusiness, ExcelBusiness>();
            services.AddScoped<IUserBiz, UserBiz>();

            //Nlog DI
            services.AddSingleton<ILoggerManager, LoggerManager>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //swagger 
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //redirect to error handler
            //app.UseExceptionHandler(x => x.UseCustomError());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
