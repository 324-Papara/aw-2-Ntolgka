using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Para.Data.Context;
using Para.Data.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Para.Api.Validators;
using Para.Data.Validators;
using Para.Base.Validators;
using Para.Data.GenericRepository;

namespace Para.Api;

public class Startup
{
    public IConfiguration Configuration;
    
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }
    
    
    public void ConfigureServices(IServiceCollection services)
    {
               
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddValidatorsFromAssemblyContaining<BookValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerAddressValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerDetailValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerPhoneValidator>();
        services.AddValidatorsFromAssemblyContaining<BaseEntityValidator>();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Para.Api", Version = "v1" });
        });

        var connectionStringSql = Configuration.GetConnectionString("MsSqlConnection");
        services.AddDbContext<ParaSqlDbContext>(options => options.UseSqlServer(connectionStringSql));
        
        var connectionStringPostgre = Configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<ParaPostgreDbContext>(options => options.UseNpgsql(connectionStringPostgre));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Para.Api v1"));
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