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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace RentMagicClient
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(config =>
            {
                // We check the cookie to confirm that we are authenticated
                config.DefaultAuthenticateScheme = "ClientCookie";
                // When we sign in we will deal out a cookie
                config.DefaultSignInScheme = "ClientCookie";
                // use this to check if we are allowed to do something
                config.DefaultChallengeScheme = "Unit4";
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
                    config.ClientId = "f458b220-7c45-45fd-a352-97a09246426d";
                    config.ClientSecret = "yJF2o6BJEqKO";
                    config.CallbackPath = "/login-exact";
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

            services.AddControllersWithViews();
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
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
