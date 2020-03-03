using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Persistence;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebApi
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
            services.Configure<AppConfiguration>(Configuration);
            AppConfiguration config = Configuration.Get<AppConfiguration>();

            services.AddDbContext<DemoDbContext>(opt =>
                opt.UseInMemoryDatabase("DemoList"));
            services.AddControllers();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.ForwardDefaultSelector = ctx => ctx.Request.Path.StartsWithSegments("/api")
                        ? JwtBearerDefaults.AuthenticationScheme
                        : CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = config.Authority;
                    options.Audience = config.Audience;
                    options.RequireHttpsMetadata = false;

                    IdentityModelEventSource.ShowPII = true;
                    options.TokenValidationParameters.ValidateIssuer = false;

                    options.BackchannelHttpHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                    };
                });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Web Api", Version = "v1", Description = "Web Api Swagger"
                });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "OAuth2 Auth code Grant",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{config.IdentityServer}/connect/authorize", UriKind.Absolute),
                                TokenUrl = new Uri($"{config.IdentityServer}/connect/token", UriKind.Absolute),
                                RefreshUrl = new Uri($"{config.IdentityServer}/connect/authorize", UriKind.Absolute),
                                Scopes = new Dictionary<string, string> {{config.Scope, "api scope"}},
                            }
                        }
                    }
                );

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "oauth2",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    }
                );
                string basePath = AppContext.BaseDirectory;
                string[] xmlFiles = Directory.GetFiles(basePath, "*.xml");
                foreach (var xmlFile in xmlFiles)
                {
                    options.IncludeXmlComments(xmlFile);
                }
            });

            services.AddAuthorization();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppConfiguration> appConfigurationOptions)
        {
            var config = appConfigurationOptions.Value;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
                c.OAuthConfigObject = new OAuthConfigObject
                {
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    AppName = "Swagger UI",
                    UsePkceWithAuthorizationCodeGrant = true
                };

                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Web Api v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
