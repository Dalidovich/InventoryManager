using InventoryManager.BLL.Hubs;
using InventoryManager.DAL;
using InventoryManager.Domain.Enums;
using InventoryManager.Midlaware;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddRepositores();
            builder.AddServices();
            builder.AddHostedService();
            builder.AddAuthPolicy();
            builder.AddJWT();
            builder.Services.AddDbContext<AppDBContext>(opt => opt.UseNpgsql(
                builder.Configuration.GetConnectionString(StandardConst.NameConnection)));

            builder.Services.AddSignalR();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(builder.Configuration.GetSection("CorsSettings:CorsPolicyName").Value,
                    CorsBuilder => CorsBuilder
                        .WithOrigins($"{builder.Configuration.GetSection("CorsSettings:URL").Value}")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        );
            });

            var app = builder.Build();

            app.UseCors(builder.Configuration.GetSection("CorsSettings:CorsPolicyName").Value);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.MapHub<InventoryHub>("/InventoryHub");

            app.Run();
        }
    }
}
