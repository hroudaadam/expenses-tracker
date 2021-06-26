using ExpensesTracker.Api.Data;
using ExpensesTracker.Api.Services;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ExpensesTracker.Api.Helpers;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExpensesTracker.Api
{
    // TODO: healthchecks
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            // DB context configuration
            services.AddDbContext<DataContext>(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("Dev"));
            });

            services.AddControllers()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                })
                .AddXmlSerializerFormatters()
                .ConfigureApiBehaviorOptions(o =>
                {
                    // empty 404 erorrs
                    o.SuppressMapClientErrors = true;
                });

            services.AddApiVersioning(o => {
                // custom api version errors
                o.ErrorResponses = new VersioningErrorResponseProvider();     
            });

            services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();
                });

            // logic services
            services.AddScoped<IExpensesService, ExpensesService>();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseHttpsRedirection();

            app.UseExceptionHandler("/error");

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
