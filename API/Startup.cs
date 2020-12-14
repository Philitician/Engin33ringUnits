using System;
using ConversionGrpcService;
using DimensionalClassGrpcService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Prometheus;
using QuantityTypeGrpcService;

namespace API
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
            services.AddGrpcClient<Conversion.ConversionClient>(options =>
                options.Address = new Uri(Configuration.GetConnectionString("EngineeringUnits")));
            services.AddGrpcClient<DimensionalClass.DimensionalClassClient>(options =>
                options.Address = new Uri(Configuration.GetConnectionString("EngineeringUnits")));
            services.AddGrpcClient<QuantityType.QuantityTypeClient>(options =>
                options.Address = new Uri(Configuration.GetConnectionString("EngineeringUnits")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API", Version = "v1"});
                c.CustomSchemaIds(type => type.ToString());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            // Http Context
            var counter = Metrics.CreateCounter("api_path_counter", "Count request to the API",
                new CounterConfiguration {LabelNames = new[] {"method", "endpoint"}});

            app.Use((context, next) =>
            {
                // method: GET, POST etc.
                // endpoint: Requested path
                counter.WithLabels(context.Request.Method, context.Request.Path).Inc();
                return next();
            });
            
            app.UseMetricServer();

            app.UseHttpMetrics();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}