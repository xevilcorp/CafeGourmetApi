using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IdentityErrorDescriber, PortugueseIdentityErrorDescriber>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:5500", "https://xevilcorp.github.io")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
  var secretString = builder.Configuration["JwtConfig:Secret"];
  var secretkey = Encoding.ASCII.GetBytes(secretString!);
  o.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(secretkey),
    ValidateIssuer = false,
    ValidateAudience = false,
    ClockSkew = TimeSpan.Zero
  };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
  if (!roleManager.RoleExistsAsync("Admin").Result)
  {
    var role = new IdentityRole("Admin");
    var roleResult = roleManager.CreateAsync(role).Result;
  }
  if (!roleManager.RoleExistsAsync("User").Result)
  {
    var role = new IdentityRole("User");
    var roleResult = roleManager.CreateAsync(role).Result;
  }
}

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();