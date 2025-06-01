using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace ProjectName;

[DependsOn(
    // ProjectName
    typeof(ProjectNameCoreModule),
    typeof(ProjectNameInfrastructureModule),
    typeof(ProjectNameHttpApiModule),
    
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule)
)]
public class ProjectNameHttpApiHostModule : AbpModule
{
    private readonly string[] _moduleNames =
    [
        "ProjectName"
    ];
    
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // 日志
        Configure<AbpAuditingOptions>(options =>
        {
            options.ApplicationName = ProjectNameCoreConsts.ApplicationName;
            options.IsEnabledForGetRequests = true;
        });
        
        // 时间格式 
        Configure<AbpJsonOptions>(options =>
        {
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        });
        
        // 跨域
        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(
                        configuration["App:CorsOrigins"]?
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(o => o.RemovePostFix("/"))
                            .ToArray() ?? []
                    )
                    .WithAbpExposedHeaders()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        // Swagger
        context.Services.AddSwaggerGen(options =>
        {
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
            
            foreach (var moduleName in _moduleNames)
            {
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, moduleName + ".HttpApi.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, moduleName + ".Core.xml"), true);
            }

            #region Bearer Token
            options.AddSecurityDefinition("BearerToken", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "请输入Token!"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerToken" }
                    },
                    []
                }
            });
            #endregion
            
            #region swagger 分组
            options.DocInclusionPredicate((doc, api) =>
            {
                if (api.GroupName == "*") { return true; }
                return doc == api.GroupName;
            });
        
            foreach (var moduleName in _moduleNames)
            {
                options.SwaggerDoc(moduleName, new OpenApiInfo { Title = moduleName + " Module Api", Version = "v1" });
            }
            #endregion
        });
        
        // 添加JWT身份验证服务
        context.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var secretByte = Encoding.UTF8.GetBytes(configuration["JwtOptions:SecretKey"]!);
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JwtOptions:Audience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretByte)
                };
            });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelExpandDepth(-1);
            foreach (var moduleName in _moduleNames)
            {
                options.SwaggerEndpoint($"/swagger/{moduleName}/swagger.json",moduleName + " Module Api v1");
            }
        });
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAuditing();

        app.UseConfiguredEndpoints(endpoints =>
        {
            // AuthorizeAttribute
            endpoints.MapControllers().RequireAuthorization();
        });

    }
}