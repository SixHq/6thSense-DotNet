using AspNetCore.RouteAnalyzer;
using Sixth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();


builder.Services.AddRouteAnalyzer();

var app = builder.Build();
   app.UseRouting();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.MapControllers();
SixthSdk sixth = new SixthSdk("YVawS7tr1SaBmeG4NVZt3OniEw52",app);
await sixth.InitializeApp();
//app.UseEncryption_Middleware();
//app.UseSixth_Rate_Limiter_Express_Middleware();



app.Run();
