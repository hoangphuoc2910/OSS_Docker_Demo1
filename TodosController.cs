using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Project2_TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        // Thay chuỗi kết nối Somee của bạn vào đây
        private readonly string connectionString = "Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult GetTodos()
        {
            var list = new List<Todo>();
            try {
                using (SqlConnection conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Todos ORDER BY CreatedAt DESC", conn)) {
                        using (SqlDataReader reader = cmd.ExecuteReader()) {
                            while (reader.Read()) {
                                list.Add(new Todo {
                                    Id = (int)reader["Id"],
                                    Title = reader["Title"].ToString(),
                                    IsCompleted = (bool)reader["IsCompleted"],
                                    CreatedAt = (DateTime)reader["CreatedAt"]
                                });
                            }
                        }
                    }
                }
                return Ok(list);
            } catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost]
        public IActionResult AddTodo([FromBody] Todo todo)
        {
            try {
                using (SqlConnection conn = new SqlConnection(connectionString)) {
                    conn.Open();
                    string query = "INSERT INTO Todos (Title) VALUES (@Title)";
                    using (SqlCommand cmd = new SqlCommand(query, conn)) {
                        cmd.Parameters.AddWithValue("@Title", todo.Title);
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Thêm thành công" });
            } catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}