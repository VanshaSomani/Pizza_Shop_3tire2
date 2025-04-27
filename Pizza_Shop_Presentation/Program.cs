using System.Text;
using System.Text.Json.Serialization;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pizza_Shop_Repository.Interfaces;
using Pizza_Shop_Repository.Models;
using Pizza_Shop_Repository.Repository;
using Pizza_Shop_Services.Interfaces;
using Pizza_Shop_Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserLoginService,UserLoginService>();
builder.Services.AddScoped<IUserLoginRepository,UserLoginRepository>();
builder.Services.AddScoped<IJWTService,JWTService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IRolePermissionService,RolePermissionService>();
builder.Services.AddScoped<IRolePermissionRepository,RolePermissionRepository>();
builder.Services.AddScoped<IMenuService,MenuService>();
builder.Services.AddScoped<IMenuRepository,MenuRepository>();
builder.Services.AddScoped<ITableAndSectionService,TableAndSectionService>();
builder.Services.AddScoped<ITableAndSectionRepository,TableAndSectionRepository>();
builder.Services.AddScoped<ITaxesAndFeesService,TaxesAndFeesService>();
builder.Services.AddScoped<ITaxesAndFeesRepository,TaxesAndFeesRepository>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IOrderExcelService,OrderExcelService>();
builder.Services.AddScoped<ICustomerService,CustomerService>();
builder.Services.AddScoped<ICustomerRepository,CustomerRepository>();
builder.Services.AddScoped<ICustomerExcelService,CustomerExcelService>();
builder.Services.AddScoped<IKOTService,KOTService>();
builder.Services.AddScoped<IKOTRepository,KOTRepository>();
builder.Services.AddScoped<IOrderAppTableService,OrderAppTableService>();
builder.Services.AddScoped<IOrderAppTableRepository,OrderAppTableRepository>();
builder.Services.AddScoped<IWaitingListService,WaitingListService>();
builder.Services.AddScoped<IWaitingListRepository,WaitingListRepository>();
builder.Services.AddScoped<IOrderAppMenuService,OrderAppMenuService>();
builder.Services.AddScoped<IOrderAppMenuRepository,OrderAppMenuRepository>();

//jwt token configration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
    options.Events = new JwtBearerEvents{
        //OnMessageReceived run when authentication request is recived
        OnMessageReceived = context => {
            var accessToken = context.Request.Cookies["jwtToken"];
            //check token axists
            if(!string.IsNullOrEmpty(accessToken)){
                //store token
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// builder.Services.AddJWTAuthentication(builder.Configuration);

//session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

//toast configure
builder.Services.AddNotyf(config=>
{
    config.DurationInSeconds = 1;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight; 
}
);

builder.Services.AddMvc().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//singeltone is used for using session veriables in view page
builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

//dbcontext configuration
var conn = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<RmsdemoContext>(q => q.UseNpgsql(conn));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// var env = app.Environment;
// Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

// app.UseRotativa();

app.UseStatusCodePagesWithRedirects("/ErrorPage/NotFound");

app.UseSession();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserLogin}/{action=Login}/{id?}");

app.Run();
