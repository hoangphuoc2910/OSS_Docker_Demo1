using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Project2_TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly string _connectionString;

        public TodosController(IConfiguration configuration)
        {
            // Lấy chuỗi kết nối từ appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // 1. GET: api/todos (Lấy danh sách)
        [HttpGet]
        public IActionResult GetTodos()
        {
            var todos = new List<object>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT Id, Title, DueDate, IsCompleted FROM Todos ORDER BY CreatedAt DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        todos.Add(new {
                            id = rdr["Id"],
                            title = rdr["Title"],
                            dueDate = rdr["DueDate"] == DBNull.Value ? null : rdr["DueDate"],
                            isCompleted = (bool)rdr["IsCompleted"]
                        });
                    }
                }
            }
            return Ok(todos);
        }

        // 2. POST: api/todos (Thêm mới)
        [HttpPost]
        public IActionResult AddTodo([FromBody] Todo item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Todos (Title, DueDate) VALUES (@title, @date)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@title", item.Title);
                cmd.Parameters.AddWithValue("@date", (object)item.DueDate ?? DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return Ok(new { success = true });
        }
    }

    public class Todo {
        public string Title { get; set; }
        public DateTime? DueDate { get; set; }
    }
}