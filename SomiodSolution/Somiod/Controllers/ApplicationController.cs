using Somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Somiod.Controllers
{
    [RoutePrefix("api/somiod")]
    public class ApplicationController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SomiodSolution.Properties.Settings.ConnStr"].ConnectionString;

        // GET api/somiod/{appName}
        [HttpGet]
        [Route("{appName}")]
        public IHttpActionResult GetApplication(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
                return BadRequest("Application resource-name is required.");

            Application app = null;
            SqlConnection conn = null;
            try
            {
                //conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DBProds.mdf;Integrated Security=True;Connect Timeout=30");
                //conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString);
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Application WHERE ResourceName = @name", conn);
                command.Parameters.AddWithValue("@name", appName);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    app = new Application
                    {
                        Id = (int)reader["Id"],
                        ResourceName = (string)reader["ResourceName"],
                        ResType = (string)reader["ResType"],
                        CreationDatetime = (DateTime)reader["CreationDateTime"]
                    };
                }
                reader.Close();
                conn.Close();
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
            }
            return Ok(app);
        }

        // POST api/somiod
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostApplication([FromBody] Application app)
        {
            if (app == null)
                return BadRequest("Invalid application payload.");

            if (string.IsNullOrWhiteSpace(app.ResourceName))
                return BadRequest("resource-name is required.");

            app.ResType = "application";
            app.CreationDatetime = DateTime.UtcNow;

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // verificar se já existe uma com o mesmo resource-name
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Application WHERE ResourceName = @name", conn);
                checkCmd.Parameters.AddWithValue("@name", app.ResourceName);
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    return Content(HttpStatusCode.Conflict,
                        $"Application with resource-name '{app.ResourceName}' already exists.");
                }

                string query = @"INSERT INTO Application (ResourceName, ResType, CreationDateTime)
                                 VALUES (@name, @resType, @creation);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@name", app.ResourceName);
                command.Parameters.AddWithValue("@resType", app.ResType);
                command.Parameters.AddWithValue("@creation", app.CreationDatetime);

                // Id gerado
                app.Id = Convert.ToInt32(command.ExecuteScalar());

                conn.Close();

                // no SOMIOD, ao criar um recurso devemos devolver o recurso completo
                return Content(HttpStatusCode.Created, app);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return BadRequest("Erro ao criar a aplicação");
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}