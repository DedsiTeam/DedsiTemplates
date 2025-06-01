using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace ProjectNameCQRS;

[DependsOn(
    typeof(ProjectNameCQRSHttpApiModule),

    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAutofacModule)
)]
public class ProjectNameCQRSHostModule : AbpModule
{
    private readonly string[] _moduleNames =
    [
        "ProjectNameCQRS"
    ];

    private readonly string[] _useCaseModuleNames =
    [
        "ProjectNameCQRS.UseCase"
    ];

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostEnvironment = context.Services.GetAbpHostEnvironment();

        // 数据库
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(dbConfigContext =>
            {
                // 本地研发环境 - 输出到控制台
                if (hostEnvironment.EnvironmentName == "Development")
                {
                    dbConfigContext.DbContextOptions.LogTo(Serilog.Log.Information, new[] { DbLoggerCategory.Database.Command.Name }).EnableSensitiveDataLogging();
                }
                dbConfigContext.UseSqlServer();
            });
        });
        
        // 日志
        Configure<AbpAuditingOptions>(options =>
        {
            options.ApplicationName = ProjectNameCQRSDomainConsts.ApplicationName;
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
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, moduleName + ".UseCase.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, moduleName + ".Shared.xml"), true);
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
                options.SwaggerDoc(moduleName, new OpenApiInfo { Title = $"{moduleName} Module Api", Version = "v1" });
            }
            #endregion
        });
        
        // 添加JWT身份验证服务
        context.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JwtOptions:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:SecurityKey"]!))
                };
            });
        
        // MediatR
        context.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(_useCaseModuleNames.Select(Assembly.Load).ToArray()));
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
                options.SwaggerEndpoint($"/swagger/{moduleName}/swagger.json", $"{moduleName} Module Api v1");
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