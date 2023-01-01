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


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();//�M���w�]�M�g
builder.Services.AddSingleton<JwtHelpers>();//���UJwtHelper
builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
                options.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // �z�L�o���ŧi�A�N�i�H�q "sub" ���Ȩó]�w�� User.Identity.Name
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // �z�L�o���ŧi�A�N�i�H�q "roles" ���ȡA�åi�� [Authorize] �P�_����
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

                        // �@��ڭ̳��|���� Issuer
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

                        // �q�`���ӻݭn���� Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // �����ҴN���ݭn��g

                        // �@��ڭ̳��|���� Token �����Ĵ���
                        ValidateLifetime = true,

                        // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
                        ValidateIssuerSigningKey = false,

                        // "1234567890123456" ���ӱq IConfiguration ���o
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
                    };

                    //options.TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    ValidateIssuer = true,
                    //    ValidateAudience = true, // ���{�ҨϥΪ�
                    //    ValidateLifetime = true,
                    //    ValidateIssuerSigningKey = true,  // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
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
//�{��
app.UseAuthentication();
//���v
app.UseAuthorization();

app.Run();