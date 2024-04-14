namespace Zadanie3.Controllers

using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Microsoft.AspNetCore.Mvc;

{
    [ApiController]
    [Route("api/animal")]

    public class AnimalController : ControllerBase
	{
		private readonly string _connectionString = "Nie mam jeszcze bazy";
		[HttpGet]
		public ActionResult<IEnumerable<Animal>> Get([FromQuery] string orderBy = "name")
		{
			var animals = new List<Animal>();

			string query = $"SELECT * FROM Animals ORDER BY {orderBy};";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                animals.Add(new Animal
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Category = reader.GetString(reader.GetOrdinal("Category")),
                    Area = reader.GetString(reader.GetOrdinal("Area"))
                });
            }

            reader.Close();
        }

        return Ok(animals);
    }

    [HttpPost]
		public IActionResult Post([FromBody] Animal animal)
		{
        if (string.IsNullOrEmpty(animal.Name) || string.IsNullOrEmpty(animal.Description))
        {
            return BadRequest("Name and description are required.");
        }

        string query = @"INSERT INTO Animals (Name, Description, Category, Area) 
                VALUES (@Name, @Description, @Category, @Area); SELECT SCOPE INDENTITY" ;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Name", animal.Name);
            command.Parameters.AddWithValue("@Description", animal.Description);
            command.Parameters.AddWithValue("@Category", animal.Category ?? (object)DBNull.Value); 
            command.Parameters.AddWithValue("@Area", animal.Area ?? (object)DBNull.Value); 

            connection.Open();

            int result = command.ExecuteNonQuery();

            if (result < 0)
            {
                return StatusCode(500, "Error inserting the record.");
            }
        }

        return Ok("Record added.");
    }
}