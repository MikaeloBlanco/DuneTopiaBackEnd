using Microsoft.IdentityModel.Tokens;
using DunetopiaBackEnd.Models.Database;
using System.Text;

namespace DunetopiaBackEnd
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<MyDBContext>();
            builder.Services.AddTransient<DbSeeder>();

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                string Key = Environment.GetEnvironmentVariable("JWT_KEY");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
                };

            });

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                MyDBContext dbContext = scope.ServiceProvider.GetService<MyDBContext>();
                DbSeeder dbSeeder = scope.ServiceProvider.GetService<DbSeeder>();
                await dbSeeder.SeedAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
