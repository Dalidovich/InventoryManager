using InventoryManager.AuthPolicy;
using InventoryManager.BLL.Interfaces;
using InventoryManager.BLL.Services;
using InventoryManager.DAL.Repositories.Implements;
using InventoryManager.DAL.Repositories.Interfaces;
using InventoryManager.Domain.Entities;
using InventoryManager.Domain.Enums;
using InventoryManager.Domain.JWT;
using InventoryManager.HostedServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace InventoryManager
{
    public static class DIManger
    {
        public static void AddRepositores(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<IRepository<AccessAccountToInventory>, BaseRepository<AccessAccountToInventory>>();
            webApplicationBuilder.Services.AddScoped<IRepository<Account>, BaseRepository<Account>>();
            webApplicationBuilder.Services.AddScoped<IRepository<Comment>, BaseRepository<Comment>>();
            webApplicationBuilder.Services.AddScoped<IRepository<Inventory>, BaseRepository<Inventory>>();
            webApplicationBuilder.Services.AddScoped<IRepository<InventoryCategory>, BaseRepository<InventoryCategory>>();
            webApplicationBuilder.Services.AddScoped<IRepository<InventoryObject>, BaseRepository<InventoryObject>>();
            webApplicationBuilder.Services.AddScoped<IRepository<Like>, BaseRepository<Like>>();
            webApplicationBuilder.Services.AddScoped<IRepository<ObjectField>, BaseRepository<ObjectField>>();
            webApplicationBuilder.Services.AddScoped<IRepository<Tag>, BaseRepository<Tag>>();
            webApplicationBuilder.Services.AddScoped<IRepository<TokenData>, BaseRepository<TokenData>>();

            webApplicationBuilder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<IAccessAccountToInventoryService, AccessAccountToInventoryService>();
            webApplicationBuilder.Services.AddScoped<IService<Account>, BaseService<Account>>();
            webApplicationBuilder.Services.AddScoped<IService<Comment>, BaseService<Comment>>();
            webApplicationBuilder.Services.AddScoped<IInventoryService, InventoryService>();
            webApplicationBuilder.Services.AddScoped<IService<InventoryCategory>, BaseService<InventoryCategory>>();
            webApplicationBuilder.Services.AddScoped<IService<InventoryObject>, BaseService<InventoryObject>>();
            webApplicationBuilder.Services.AddScoped<IService<Like>, BaseService<Like>>();
            webApplicationBuilder.Services.AddScoped<IObjectFieldService, ObjectFieldService>();
            webApplicationBuilder.Services.AddScoped<IService<Tag>, BaseService<Tag>>();
            webApplicationBuilder.Services.AddScoped<IService<Account>, BaseService<Account>>();

            webApplicationBuilder.Services.AddScoped<IRegistrationService, RegistrationService>();
            webApplicationBuilder.Services.AddScoped<ITokenService, TokenService>();
        }

        public static void AddAuthPolicy(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddAuthorization(options =>
             {
                 options.AddPolicy(AuthPolicyName.ActiveStatusPolicyRequire, policy =>
                     policy.RequireAssertion(context =>
                         context.User.HasClaim(c =>
                             c.Type == CustomClaimType.Status &&
                             c.Value == ((int)AccountStatus.Active).ToString()
                         )
                     ));
             });
        }

        public static void AddHostedService(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddHostedService<CheckDBHostedService>();
        }

        public static void AddJWT(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.Configure<JWTSettings>(webApplicationBuilder.Configuration.GetSection("JWTSettings"));
            var secretKey = webApplicationBuilder.Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = webApplicationBuilder.Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = webApplicationBuilder.Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            webApplicationBuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = JwtHelper.CustomLifeTimeValidator
                };
            });
        }
    }
}