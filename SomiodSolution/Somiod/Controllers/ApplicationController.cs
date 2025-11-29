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

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllApplications()
        {
            //return products;
            List<Application> apps = new List<Application>();
            SqlConnection conn = null;
            try
            {
                //conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DBProds.mdf;Integrated Security=True;Connect Timeout=30");
                //conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ProductsApp.Properties.Settings.ConnectionToDB"].ConnectionString);
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT * FROM Prods ORDER BY Id", conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Application a = new Application
                    {
                        ResourceName = (string)reader["ResoursceName"],
                        ResType = (reader["ResType"] == DBNull.Value) ? "application" : (string)reader["Restype"],
                        CreationDatetime = reader.GetDateTime(reader.GetOrdinal("CreationDatetime"))
                    };
                    apps.Add(a);
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
            return Ok(apps);
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
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