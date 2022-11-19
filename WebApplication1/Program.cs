using NLog.Web;
using DistributedCache.Infrastructure;

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


builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSession();
app.Use(async (context, next) =>
{
    context.Session.SetString("SessionKey", "SessoinValue");
    await next.Invoke();
});
app.UseRouting();
app.UseCors(myCors);

app.Run();