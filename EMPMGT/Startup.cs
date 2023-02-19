using EMPMGT.DataContext;
using EMPMGT.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace EMPMGT
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages()
              .AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                  options.JsonSerializerOptions.PropertyNamingPolicy = null;
              });
            services.AddDbContext<EmpContext>(a => {
                a.UseInMemoryDatabase("EmpMgtDb").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
				a.EnableSensitiveDataLogging();
			});
			services.AddScoped<DbContext, EmpContext>();
			
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();

           
            services.AddSession();
			services.AddAutoMapper(typeof(Startup));

		}
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                //add endpoints for controller actions
                endpoints.MapControllers();

                endpoints.MapRazorPages();
            });
            app.Run();
        }
    }
}
