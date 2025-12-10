using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Somiod.Models;

namespace Somiod.Controllers
{
    //[RoutePrefix("api/somiod")]
    public class ContainersController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Somiod.Properties.Settings.ConnStr"].ConnectionString;


        // GET api/somiod/{appName}/{contName}
        [HttpGet]
        [Route("api/somiod/{appName}/{contName}")]
        public IHttpActionResult GetContainer(string appName, string contName)
        {
            SqlConnection conn = null;
            Containers cont = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand command = new SqlCommand(
                    @"SELECT c.Id, c.ResourceName, c.ResType, c.CreationDateTime, c.Application_ID
                      FROM Containers c
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName = @appName AND c.ResourceName = @contName", conn);

                command.Parameters.AddWithValue("@appName", appName);
                command.Parameters.AddWithValue("@contName", contName);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    cont = new Containers
                    {
                        Id = (int)reader["Id"],
                        ResourceName = (string)reader["ResourceName"],
                        ResType = (string)reader["ResType"],
                        CreationDatetime = (DateTime)reader["CreationDateTime"],
                        Application_ID = (int)reader["Application_ID"]
                    };
                }

                reader.Close();
                conn.Close();

                if (cont == null)
                {
                    return NotFound();
                }
                return Ok(cont);
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }
        

        // POST api/somiod/{appName}
        [HttpPost]
        [Route("api/somiod/{appName}/containers")]
        public IHttpActionResult PostContainer(string appName, [FromBody] Containers c)
        {
            SqlConnection conn = null;
            if (c == null)
            {
                return BadRequest("Container inválido.");
            }
            c.ResType = "container";
            c.CreationDatetime = DateTime.UtcNow;
            try
            {
                //conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DBProds.mdf;Integrated Security=True;Connect Timeout=30");
                //conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString);
                conn = new SqlConnection(connectionString);
                conn.Open();

                //Ir buscar o Id da aplicação
                SqlCommand getAppIdCmd = new SqlCommand("SELECT Id FROM Application WHERE ResourceName = @appName", conn);
                getAppIdCmd.Parameters.AddWithValue("@appName", appName);
                var appResult = getAppIdCmd.ExecuteScalar();

                if (appResult == null)
                {
                    conn.Close();
                    return NotFound(); // aplicação não existe
                }
                int appId = (int)appResult;
                c.Application_ID = appId;


                //Verificar se já existe container com este nome nessa app
                SqlCommand Checkcontainercmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM Containers 
                      WHERE Application_ID = @appId AND ResourceName = @resourcename", conn);
                Checkcontainercmd.Parameters.AddWithValue("@appId", c.Application_ID);
                Checkcontainercmd.Parameters.AddWithValue("@resourcename", c.ResourceName);
                int count = (int)Checkcontainercmd.ExecuteScalar();

                if (count > 0)
                {
                    conn.Close();
                    return Content(HttpStatusCode.Conflict,
                        $"Já existe um container com o nome '{c.ResourceName}' na aplicação '{appName}'.");
                }

                //Inserir o container na app
                SqlCommand insertContainerCmd = new SqlCommand(
                    @"INSERT INTO Containers (ResourceName, ResType, CreationDateTime, Application_ID)
                      VALUES (@name, @resType, @creation, @appId);
                      SELECT SCOPE_IDENTITY();", conn);

                insertContainerCmd.Parameters.AddWithValue("@name", c.ResourceName);
                insertContainerCmd.Parameters.AddWithValue("@resType", c.ResType);
                insertContainerCmd.Parameters.AddWithValue("@creation", c.CreationDatetime);
                insertContainerCmd.Parameters.AddWithValue("@appId", appId);

                // obter o Id gerado
                c.Id = Convert.ToInt32(insertContainerCmd.ExecuteScalar());

                conn.Close();

                // se chegou aqui sem exceção, consideramos sucesso
                return Ok("Container inserida com sucesso!");
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        // GET api/somiod/{appName}/containers  (DISCOVERY)
        [HttpGet]
        [Route("api/somiod/{appName}/containers")]
        public IHttpActionResult DiscoverContainers(string appName)
        {
            var hasHeader = Request.Headers.Contains("somiod-discovery");
            var headerValue = hasHeader
                ? Request.Headers.GetValues("somiod-discovery").FirstOrDefault()
                : null;

            if (!hasHeader || !string.Equals(headerValue, "container", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Use header 'somiod-discovery: container' para discovery de containers.");
            }

            List<string> paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT c.ResourceName
                      FROM Containers c
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName = @appName
                      ORDER BY c.Id", conn);

                cmd.Parameters.AddWithValue("@appName", appName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string contName = (string)reader["ResourceName"];
                    paths.Add($"/api/somiod/{appName}/{contName}");
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

        // DELETE api/somiod/{appName}/{contName}
        [HttpDelete]
        [Route("api/somiod/{appName}/{contName}")]
        public IHttpActionResult DeleteContainer(string appName, string contName)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand command = new SqlCommand(
                    @"DELETE FROM Containers
                      WHERE Id IN (
                        SELECT c.Id
                        FROM Containers c
                        INNER JOIN Application a ON c.Application_ID = a.Id
                        WHERE a.ResourceName = @appName AND c.ResourceName = @contName
                      )", conn);

                command.Parameters.AddWithValue("@appName", appName);
                command.Parameters.AddWithValue("@contName", contName);

                int rows = command.ExecuteNonQuery();
                conn.Close();

                if (rows > 0)
                    return Ok($"Container '{contName}' da aplicação '{appName}' eliminado com sucesso!");
                else
                    return NotFound();
            }
            catch (Exception e)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }
    }
}