using System.Reflection;
using System.Text;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
builder.Services.ConfigureCors();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
//^this tells HOW to authenticate.. I also need to add the middlware to tell it to authenticate



var app = builder.Build();


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
//order is v important here...
app.UseAuthentication();
//^asks "do you have a valid token
app.UseAuthorization();
//^ says okay... you have a valid token, now what are you allowed to do

app.MapControllers();

app.Run();
