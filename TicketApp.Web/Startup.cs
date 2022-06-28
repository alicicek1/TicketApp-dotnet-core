using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketApp.Application.Interfaces;
using TicketApp.Application.Interfaces.Base;
using TicketApp.Application.Mapper;
using TicketApp.Application.Services;
using TicketApp.Core.Configuration;
using TicketApp.Core.Entities;
using TicketApp.Core.Repositories.Base;
using TicketApp.Infrastructure.Repositories;
using TicketApp.Web.MvcFilters;

namespace TicketApp.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureRunServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureRunServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<UserDocument>), typeof(UserRepository));
            services.AddScoped<IUserService, UserService>();

            services.AddAutoMapper(cfg => { cfg.AddProfile<RequestToDocumentMapping>(); });
        }
    }
}