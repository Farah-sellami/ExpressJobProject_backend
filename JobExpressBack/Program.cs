using CloudinaryDotNet;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using JobExpressBack.Models;
using JobExpressBack.Models.DTOs;
using JobExpressBack.Models.Entities;
using JobExpressBack.Models.Repositories;
using JobExpressBack.Models.Repositories.Authentification;
using JobExpressBack.Models.Repositories.RepoDemandeService;
using JobExpressBack.Models.Repositories.RepoNotification;
using JobExpressBack.Models.Repositories.RepoServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Adding EF Core with SQL Server
// pour migration du base de données
builder.Services.AddDbContext<ExJobDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure()));

builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();


// ajouter les services de Identity Core
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ExJobDBContext>()
    .AddDefaultTokenProviders();

// Ajout des contrôleurs et gestion des JSON cycles
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);


//configurer l'injection de dépendances
//Un service scoped est instancié une fois par requête HTTP
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<PhotoService>();

// Enregistrement du repository générique et spécifique
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IServiceRepository, ServiceRpository>();
builder.Services.AddScoped<IDemandeServiceRepository, DemandeServiceRepository>();

//builder.Services.AddScoped<INotificationRepository,NotificationRepository>();

// Ajouter la configuration Cloudinary directement
var cloudinary = new Cloudinary(new Account(
    builder.Configuration["CloudinarySettings:CloudName"],
    builder.Configuration["CloudinarySettings:ApiKey"],
    builder.Configuration["CloudinarySettings:ApiSecret"]
));
/* Un singleton est un objet qui est instancié une seule fois pendant le cycle de vie de l'application. 
 * Cela signifie qu'il n'y aura qu'une seule instance de l'objet partagé entre tous les consommateurs.
 */
// Ajouter Cloudinary en tant que service singleton
builder.Services.AddSingleton(cloudinary);


// Adding Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = "uid"
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client"));
    options.AddPolicy("ProfessionnelOnly", policy => policy.RequireRole("Professionnel"));
});

// Ajouter CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5174", "http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configurer automapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Appliquez la politique CORS
app.UseCors("ReactAppPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
