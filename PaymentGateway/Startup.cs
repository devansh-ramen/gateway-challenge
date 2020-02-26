using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentGateway.Models;

namespace PaymentGateway
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

            //services.AddDbContext<PaymentContext>(opt =>
            //  opt.UseInMemoryDatabase("PaymentItem"));

            //services.AddDbContext<MerchantContext>(opt =>
            //   opt.UseInMemoryDatabase("Merchant"));

            services.AddDbContext<PaymentContext>(options =>
                 options.UseSqlite(Configuration.GetConnectionString("GatewayDB")));

            services.AddDbContext<MerchantContext>(options =>
                 options.UseSqlite(Configuration.GetConnectionString("GatewayDB")));


            services.AddControllersWithViews();
            services.AddControllers();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSwaggerDocument(config => config.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "Payment Gateway Challenge";
                document.Info.Description = "REST APIs for communicating with Payment Gateway.";
                document.Info.TermsOfService = "None";
                document.Info.Contact = new NSwag.OpenApiContact
                {
                    Name = "Devansh Ramen",
                    Email = string.Empty,
                };
                document.Info.License = new NSwag.OpenApiLicense
                {
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
