using System.Text;
using e_CommerceContinental.Server.Data;
using e_CommerceContinental.Server.Hubs;
using e_CommerceContinental.Server.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var key = Encoding.ASCII.GetBytes(builder.Configuration["App:Key"]);

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddRazorPages();


string ConnectionString = string.Empty;
#if DEBUG
    ConnectionString = builder.Configuration.GetConnectionString("Local");
#else
    ConnectionString = builder.Configuration.GetConnectionString("Production");
#endif

builder.Services.AddScoped<AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(ConnectionString));      

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddHttpClient();
builder.Services.AddScoped<IConverter, Converter>();
builder.Services.AddHostedService<BackgroundTasks>();
builder.Services.AddScoped<IDedicatedSMS, DedicatedSMS>()
                .AddHttpClient<IDedicatedSMS, DedicatedSMS>(client => 
                { 
                    client.BaseAddress = new Uri(builder.Configuration["App:Url"]); 
                });
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
SeedData.EnsureSeeded(app.Services);
// TaskSubsription.Subscribe(app.Services);

// Configure the HTTP request pipeline.
app.UseResponseCompression();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapHub<SignalRHub>("/hubs");
app.MapFallbackToFile("index.html");

app.Run();
