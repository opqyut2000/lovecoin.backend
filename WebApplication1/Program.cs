using NLog.Web;
using DistributedCache.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using WebApplication1.Helpers;

var builder = WebApplication.CreateBuilder(args);
var myCors = "_myCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myCors,
    policy =>
    {
        policy.WithOrigins("http://localhost:5173", "*", "*")
        //.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


builder.Host.UseNLog();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
});

builder.Services.AddScoped<ICacheProvider, CacheProvider>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();//清除預設映射
builder.Services.AddSingleton<JwtHelpers>();//註冊JwtHelper
builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
                options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                        // 一般我們都會驗證 Issuer
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

                        // 通常不太需要驗證 Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                        // 一般我們都會驗證 Token 的有效期間
                        ValidateLifetime = true,

                        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                        ValidateIssuerSigningKey = false,

                        // "1234567890123456" 應該從 IConfiguration 取得
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
                    };

                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateIssuer = true,
                    //    ValidateAudience = true, // 不認證使用者
                    //    ValidateLifetime = true,
                    //    ValidateIssuerSigningKey = true,  // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                    //    ValidIssuer = builder.Configuration.GetSection("JWT")["ValidIssuer"],
                    //    ValidAudience = builder.Configuration.GetSection("JWT")["ValidAudience"],
                    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["Secret"]))
                    //};
                });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.UseSession();

app.UseRouting();
app.UseCors(myCors);
//認證
app.UseAuthentication();
//授權
app.UseAuthorization();

app.Run();