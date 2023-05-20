using Server.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options => options.AddDefaultPolicy(opt =>
{
    opt.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(opt => true);
}));

builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub");

app.Run();
