
using day1lab.Migrations;
using day1lab.Models;
using DemoAPI.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace day1lab
{
    public class Program
    {
        
        public static IConfiguration Configuration { get; set; }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddDbContext<APIcontext>(options =>
            //{
            //    options.UseSqlServer("Server=DESKTOP-2JDFSHG\\SQLEXPRESS;Database=APIDB;Trusted_Connection=True;TrustServerCertificate=true");
            //});
            
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new TokenValidationParameters()
                {

                   
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],

                };
            });

            builder.Services.AddDbContext<APIcontext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<APIcontext>();
            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(swagger =>
            {
            //ThisistogeneratetheDefaultUIofSwaggerDocumentation
            swagger.SwaggerDoc("v1", new OpenApiInfo
                                    {
                                                    Version = "v1",
                                    Title = "ASP.NET5WebAPI",
                                    Description = " ITI Projrcy"
                                    });
            //ToEnableauthorizationusingSwagger(JWT)
            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                                        {
                                                        Name = "Authorization",
                                        Type = SecuritySchemeType.ApiKey,
                                        Scheme = "Bearer",
                                        BearerFormat = "JWT",
                                        In = ParameterLocation.Header,
                                        Description = "Enter'Bearer'[space]andthenyourvalidtokeninthetextinputbelow.\r\n\r\nExample:\"BearereyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                                        });
            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                                            {
                                                            {
                                                                new OpenApiSecurityScheme
                                                            {
                                                                    Reference = new OpenApiReference
                                                            {
                                                                        Type = ReferenceType.SecurityScheme,
                                                                        Id = "Bearer"
                                                                                        }
                                                                                            },
                                                                        new string[]{ }
                                                            }
                                                        });
                                                    });

            builder.Services.AddCors(corsoptions =>
            {
                corsoptions.AddPolicy("mypolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    }
                    );
            }
                );
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("mypolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();
            app.MapControllers();

            app.Run();
        }
       

    }
}
