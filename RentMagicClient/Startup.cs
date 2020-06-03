using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace RentMagicClient
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

            services.AddAuthentication(config =>
            {
                // We check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                // When we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                //config.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // use this to check if we are allowed to do something
                config.DefaultChallengeScheme = "Exact";
            })
                .AddCookie("ClientCookie")
                .AddOAuth("Unit4", config =>
                {
                    config.ClientId = "f0f36c11-6dd4-425d-9abe-9a69df19989f";
                    config.ClientSecret = "2pYS7RWBY77pYDmRNBlIeV5KJZ505xfus66Nbczhxq7A12e7j9";
                    config.CallbackPath = "/login-unit4";
                    config.AuthorizationEndpoint = "https://sandbox.api.online.unit4.nl/V21/OAuth/Authorize";
                    config.TokenEndpoint = "https://sandbox.api.online.unit4.nl/V21/OAuth/Token";
                    config.Scope.Add("http://UNIT4.Multivers.API/Web/WebApi/*");

                    config.SaveTokens = true;
                })

                .AddOAuth("Exact", config =>
                {
                    config.ClientId = "59f52b9b-3d68-4ff2-870a-b170fe5574ab";
                    config.ClientSecret = "pbOUJND3kM4F";
                    config.CallbackPath = "/exact/login";
                    config.AuthorizationEndpoint = "https://start.exactonline.nl/api/oauth2/auth";
                    config.TokenEndpoint = "https://start.exactonline.nl/api/oauth2/token";

                    config.SaveTokens = true;
                });

                //.AddOAuth("Dynamics", config =>
                //{
                //    config.ClientId = "b8fe0fb4-9330-49c1-93f0-2cfcdad2a010";
                //    config.ClientSecret = "5_xAl6N6cnw_7s2W5-oSX9-52y4f~3cM0~";
                //    config.CallbackPath = "/login-dynamics";
                //    config.AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/authorize";

                //    config.SaveTokens = true;
                //});

            services.AddHttpClient();

            //services.AddControllersWithViews();

            //services.AddControllers()
            //     .AddNewtonsoftJson(options =>
            //     {
            //         options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //     });

            services.AddMvc().AddRazorPagesOptions(o =>
            {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
            });

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(next => context =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
                return next(context);
            });
            // else
            // {
            //     app.UseExceptionHandler("/Error");
            //     app.UseHsts();
            // }
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            //app.UseAntiforgeryToken();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
