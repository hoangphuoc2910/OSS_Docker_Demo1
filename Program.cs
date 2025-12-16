var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình CORS (Phải nằm TRƯỚC builder.Build)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowInfinityFree",
        policy => policy.WithOrigins("http://demogit.infinityfreeapp.com") // Domain InfinityFree của bạn
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();

var app = builder.Build();

// 2. Kích hoạt CORS (Nằm trước MapControllers)
app.UseCors("AllowInfinityFree");

app.UseAuthorization();
app.MapControllers();

app.Run();