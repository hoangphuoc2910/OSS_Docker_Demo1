var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

// --- ĐOẠN NÀY GIÚP CHẠY FRONTEND CHUNG 1 HOST ---
app.UseDefaultFiles(); // Tự động tìm index.html
app.UseStaticFiles();  // Cho phép chạy html, css, js trong wwwroot
// ------------------------------------------------

app.UseAuthorization();
app.MapControllers();
app.Run();  