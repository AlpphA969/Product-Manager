
using Application.Abstraction;
using Application.Services;
using Infrastructure.AutoMapperProfiles;
using Persistence;
using Persistence.Tools;
using Persistence.Tools.Enums;

using System.Reflection;


namespace Web_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>(
               sp =>
               {
                   var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
                   
                   var provider = (Provider)Convert.ToInt32(builder.Configuration.GetSection("DbProvider").Value!);
                   var options = new Options(connectionString: connectionString, provider: provider);

                   

                   var unitOfWork = new UnitOfWork(options: options);

                   return unitOfWork;
               }
               );
           
            
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);






            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
