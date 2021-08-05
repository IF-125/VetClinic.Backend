using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VetClinic.BLL.Services;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Repositories.Base;
using VetClinic.Core.Interfaces.Services;
using VetClinic.DAL.Context;
using VetClinic.DAL.Repositories;
using VetClinic.DAL.Repositories.Base;
using VetClinic.WebApi.ExceptionHandling;
using VetClinic.WebApi.Validators;
using VetClinic.WebApi.Validators.EntityValidators;

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
            //Services
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<IEmployeePositionRepository, EmployeePositionRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProcedureRepository, ProcedureRepository>();
            services.AddScoped<IOrderProcedureRepository, OrderProcedureRepository>();
            services.AddScoped<IAnimalTypeRepository, AnimalTypeRepository>();

            //Services
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IPetService, PetServise>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISalaryService, SalaryService>();
            services.AddScoped<IEmployeePositionService, EmployeePositionService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProcedureService, ProcedureService>();
            services.AddScoped<IOrderProcedureService, OrderProcedureService>();
            services.AddScoped<IAnimalTypeService, AnimalTypeService>();

            //Validators
            services.AddScoped<AppointmentValidator>();
            services.AddScoped<EmployeePositionValidator>();
            services.AddScoped<EmployeeValidator>();
            services.AddScoped<OrderProcedureValidator>();
            services.AddScoped<OrderValidator>();
            services.AddScoped<PetValidator>();
            services.AddScoped<PositionValidator>();
            services.AddScoped<ProcedureValidator>();
            services.AddScoped<SalaryValidator>();
            services.AddScoped<ScheduleValidator>();
            services.AddScoped<UserValidator>();
            services.AddScoped<ScheduleCollectionValidator>();
            #endregion

            services.AddCors();
            services.AddDbContext<VetClinicDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            })
           .AddEntityFrameworkStores<VetClinicDbContext>()
           .AddDefaultTokenProviders();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers().AddNewtonsoftJson();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VetClinic.Backend"
                });

                var xmlFile = $"{Assembly.GetEntryAssembly()!.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:5101/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"api1","Api VetClinicBackend"}
                            },

                            
                        }
                    }
                });
            });
            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5101";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
           
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy =>
                {
                    policy.RequireClaim("RoleType", "Client");
                });
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            })
            .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VetClinic.Backend v1");
                    c.RoutePrefix = "swagger/ui";
                    c.OAuthClientId("swagger");
                    c.OAuthAppName("api1");
                    c.OAuthUsePkce();
                    c.OAuthClientSecret("secret");
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            );
                
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
