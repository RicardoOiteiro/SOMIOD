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

                // verificar se já existe com o mesmo resource-name
                SqlCommand checkCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Application WHERE ResourceName = @name", conn);
                checkCmd.Parameters.AddWithValue("@name", app.ResourceName);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    conn.Close();
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

                // obter o Id gerado
                app.Id = Convert.ToInt32(command.ExecuteScalar());

                conn.Close();

                int rows = command.ExecuteNonQuery();

                conn.Close();
                if (rows > 0)
                    return Ok("Aplicação inserida com sucesso!");
                else
                    return BadRequest("Erro ao inserir a Aplicação.");

            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return BadRequest("Erro ao criar a aplicação");
            }
        }

        // DELETE api/somiod/{appName}
        [HttpDelete]
        [Route("{appName}")]
        public IHttpActionResult DeleteApplication(string appName)
        {
            if (string.IsNullOrWhiteSpace(appName))
                return BadRequest("Application resource-name is required.");

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                string query = "DELETE FROM Application WHERE ResourceName = @name";
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@name", appName);

                int rowsAffected = command.ExecuteNonQuery();

                conn.Close();

                if (rowsAffected > 0)
                    return Ok($"A aplicação '{appName}' foi eliminada com sucesso!");
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return BadRequest("Erro ao excluir a aplicação");
            }
        }


        // GET api/somiod   (DISCOVERY de applications)
        // Usa header: somiod-discovery: application
        [HttpGet]
        [Route("")]
        public IHttpActionResult DiscoverApplications()
        {
            var hasHeader = Request.Headers.Contains("somiod-discovery");
            var headerValue = hasHeader
                ? Request.Headers.GetValues("somiod-discovery").FirstOrDefault()
                : null;

            if (!hasHeader || !string.Equals(headerValue, "application", StringComparison.OrdinalIgnoreCase))
            {
                // tal como manda o enunciado: nada de GET-all normal
                return BadRequest("Use header 'somiod-discovery: application' para discovery de applications.");
            }

            List<string> paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT ResourceName FROM Application ORDER BY Id", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string name = (string)reader["ResourceName"];
                    paths.Add($"/api/somiod/{name}");
                }

                reader.Close();
                conn.Close();

                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

    }
}