using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VetClinic.BLL.Services;
using VetClinic.BLL.Services.Base;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Repositories.Base;
using VetClinic.Core.Interfaces.Services;
using VetClinic.Core.Interfaces.Services.Base;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories;
using VetClinic.DAL.Repositories.Base;

namespace VetClinic.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
                #region DI
            //Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IPetRepository, PetRepository>();



            //Services
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPetService, PetServise>();
            #endregion

            services.AddDbContext<VetClinicDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();
        }
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
        }
    }
}
