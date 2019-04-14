using System;
using Helpers;
using Jaeger;
using Jaeger.Samplers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTracing;
using OpenTracing.Util;

namespace JaegerASP.NETCore
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
            string serviceName = AppDomain.CurrentDomain.FriendlyName;
            var tracer = new Tracer.Builder(serviceName)
                .WithSampler(new ConstSampler(true))
                .Build();

            //加入 OpenTracing
            services.AddOpenTracing();
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                //註冊 Jaeger tracer
                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddSingleton<ResponseHelper>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}