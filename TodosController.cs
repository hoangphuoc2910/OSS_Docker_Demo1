using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Project2_TodoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        
        private readonly string connectionString = "workstation id=DemoDBTodoList.mssql.somee.com;packet size=4096;user id=hoangdo2910_SQLLogin_1;pwd=h4l9pigc1s;data source=DemoDBTodoList.mssql.somee.com;persist security info=False;initial catalog=DemoDBTodoList;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult GetTodos()
        {
            var list = new List<Todo>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Lấy thêm cột DueDate
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Todos ORDER BY DueDate ASC, CreatedAt DESC", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new Todo
                                {
                                    Id = (int)reader["Id"],
                                    Title = reader["Title"].ToString(),
                                    IsCompleted = (bool)reader["IsCompleted"],
                                    CreatedAt = (DateTime)reader["CreatedAt"],
                                    // Kiểm tra xem có ngày không, nếu có thì lấy
                                    DueDate = reader["DueDate"] == DBNull.Value ? null : (DateTime?)reader["DueDate"]
                                });
                            }
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost]
        public IActionResult AddTodo([FromBody] Todo todo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Thêm @DueDate vào câu lệnh INSERT
                    string query = "INSERT INTO Todos (Title, DueDate) VALUES (@Title, @DueDate)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Title", todo.Title);
                        
                        // Nếu người dùng không chọn ngày thì lưu là NULL
                        if (todo.DueDate.HasValue)
                            cmd.Parameters.AddWithValue("@DueDate", todo.DueDate.Value);
                        else
                            cmd.Parameters.AddWithValue("@DueDate", DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new { message = "Thêm thành công" });
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}