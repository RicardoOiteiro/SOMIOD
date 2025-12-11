using Somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using Newtonsoft.Json;

namespace Somiod.Controllers
{
    //[RoutePrefix("api/somiod")]
    public class ResourcesController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Somiod.Properties.Settings.ConnStr"].ConnectionString;

        //   CONTENT-INSTANCES

        // POST api/somiod/{appName}/{contName}
        [HttpPost]
        [Route("api/somiod/{appName}/{contName}/contents")]
        public IHttpActionResult PostContentInstance(string appName, string contName, [FromBody] ContentInstances ci)
        {
            if (ci == null)
                return BadRequest("Content-instance inválida.");

            if (string.IsNullOrWhiteSpace(ci.ResourceName))
                return BadRequest("resource-name é obrigatório.");

            if (string.IsNullOrWhiteSpace(ci.ContentType))
                return BadRequest("content-type é obrigatório.");

            if (string.IsNullOrWhiteSpace(ci.Content))
                return BadRequest("content é obrigatório.");

            ci.ResType = "content-instance";
            ci.CreationDatetime = DateTime.UtcNow;

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // 1) Obter Id do container via appName + contName
                SqlCommand getContCmd = new SqlCommand(
                    @"SELECT c.Id
                      FROM Containers c
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName = @appName AND c.ResourceName = @contName", conn);

                getContCmd.Parameters.AddWithValue("@appName", appName);
                getContCmd.Parameters.AddWithValue("@contName", contName);

                var contResult = getContCmd.ExecuteScalar();
                if (contResult == null)
                {
                    conn.Close();
                    return NotFound(); // aplicação ou container não existem
                }

                int contId = (int)contResult;
                ci.Container_ID = contId;

                // 2) Verificar se já existe content-instance com o mesmo nome nesse container
                SqlCommand checkCmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM ContentInstances
                      WHERE Container_ID = @contId AND ResourceName = @name", conn);
                checkCmd.Parameters.AddWithValue("@contId", contId);
                checkCmd.Parameters.AddWithValue("@name", ci.ResourceName);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    conn.Close();
                    return Content(HttpStatusCode.Conflict,
                        $"Já existe uma content-instance com o nome '{ci.ResourceName}' no container '{contName}'.");
                }

                // 3) Inserir a content-instance
                SqlCommand insertCmd = new SqlCommand(
                    @"INSERT INTO ContentInstances 
                        (ResourceName, ResType, CreationDateTime, ContentType, Content, Container_ID)
                      VALUES (@name, @resType, @creation, @ctype, @content, @contId);
                      SELECT SCOPE_IDENTITY();", conn);

                insertCmd.Parameters.AddWithValue("@name", ci.ResourceName);
                insertCmd.Parameters.AddWithValue("@resType", ci.ResType);
                insertCmd.Parameters.AddWithValue("@creation", ci.CreationDatetime);
                insertCmd.Parameters.AddWithValue("@ctype", ci.ContentType);
                insertCmd.Parameters.AddWithValue("@content", ci.Content);
                insertCmd.Parameters.AddWithValue("@contId", contId);

                var newIdObj = insertCmd.ExecuteScalar();
                if (newIdObj != null && newIdObj != DBNull.Value)
                {
                    ci.Id = Convert.ToInt32(newIdObj);
                }

                conn.Close();

                // mais tarde aqui irás disparar notificações (evt = 1)
                return Ok("Content-instance inserida com sucesso!");
                NotifySubscribers(appName, contName, ci, 1);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }


        // GET api/somiod/{appName}/{contName}/{ciName}
        [HttpGet]
        [Route("api/somiod/{appName}/{contName}/{ciName}")]
        public IHttpActionResult GetContentInstance(string appName, string contName, string ciName)
        {
            SqlConnection conn = null;
            ContentInstances ci = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT ci.Id, ci.ResourceName, ci.ResType, ci.CreationDateTime,
                             ci.ContentType, ci.Content, ci.Container_ID
                      FROM ContentInstances ci
                      INNER JOIN Containers c ON ci.Container_ID = c.Id
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName = @appName
                        AND c.ResourceName = @contName
                        AND ci.ResourceName = @ciName", conn);

                cmd.Parameters.AddWithValue("@appName", appName);
                cmd.Parameters.AddWithValue("@contName", contName);
                cmd.Parameters.AddWithValue("@ciName", ciName);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    ci = new ContentInstances
                    {
                        Id = (int)reader["Id"],
                        ResourceName = (string)reader["ResourceName"],
                        ResType = (string)reader["ResType"],
                        CreationDatetime = (DateTime)reader["CreationDateTime"],
                        ContentType = (string)reader["ContentType"],
                        Content = (string)reader["Content"],
                        Container_ID = (int)reader["Container_ID"]
                    };
                }

                reader.Close();
                conn.Close();

                if (ci == null)
                    return NotFound();

                return Ok(ci);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }


        // DELETE api/somiod/{appName}/{contName}/{ciName}
        [HttpDelete]
        [Route("api/somiod/{appName}/{contName}/{ciName}")]
        public IHttpActionResult DeleteContentInstance(string appName, string contName, string ciName)
        {
            ContentInstances ciDeleted = null;
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // 1) Primeiro ir buscar a content-instance
                SqlCommand Cmd = new SqlCommand(
                    @"SELECT ci.Id, ci.ResourceName, ci.ResType, ci.CreationDateTime,
                     ci.ContentType, ci.Content
                     FROM ContentInstances ci
                     INNER JOIN Containers c ON ci.Container_ID = c.Id
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    WHERE a.ResourceName = @appName
                     AND c.ResourceName = @contName
                    AND ci.ResourceName = @ciName",conn);

                Cmd.Parameters.AddWithValue("@appName", appName);
                Cmd.Parameters.AddWithValue("@contName", contName);
                Cmd.Parameters.AddWithValue("@ciName", ciName);

                SqlDataReader reader = Cmd.ExecuteReader();

                if (reader.Read())
                {
                    ciDeleted = new ContentInstances
                    {
                        Id = (int)reader["Id"],
                        ResourceName = (string)reader["ResourceName"],
                        ResType = (string)reader["ResType"],
                        CreationDatetime = (DateTime)reader["CreationDateTime"],
                        ContentType = (string)reader["ContentType"],
                        Content = (string)reader["Content"]
                    };
                }

                reader.Close();

                if (ciDeleted == null)
                {
                    conn.Close();
                    return NotFound();
                }

                // 2) Agora sim: eliminar
                SqlCommand deleteCmd = new SqlCommand(
                    @"DELETE FROM ContentInstances WHERE Id = @id",
                    conn);

                deleteCmd.Parameters.AddWithValue("@id", ciDeleted.Id);
                int rows = deleteCmd.ExecuteNonQuery();

                conn.Close();

                // 3) Enviar notificações MQTT ou HTTP (evento = 2)
                NotifySubscribers(appName, contName, ciDeleted, 2);

                return Ok($"Content-instance '{ciName}' eliminada com sucesso.");
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }


        private void NotifySubscribers(string appName, string contName, ContentInstances ci, int evt)
        {
            // aqui fazes:
            // 1) SELECT às subscriptions daquele container
            // 2) para cada subscription:
            //      - se for MQTT → publish
            //      - se for HTTP → HTTP POST
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // 1) ir buscar subscriptions do container com o evt correto
                SqlCommand cmd = new SqlCommand(
                    @"SELECT ResourceName, Evt, Endpoint
                    FROM Subscriptions s
                    INNER JOIN Containers c ON s.Container_ID = c.Id
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    WHERE a.ResourceName = @appName
                    AND c.ResourceName = @contName
                    AND s.Evt = @evt", conn);

                cmd.Parameters.AddWithValue("@appName", appName);
                cmd.Parameters.AddWithValue("@contName", contName);
                cmd.Parameters.AddWithValue("@evt", evt);

                SqlDataReader reader = cmd.ExecuteReader();

                var subs = new List<Subscriptions>();
                while (reader.Read())
                {
                    subs.Add(new Subscriptions
                    {
                        ResourceName = (string)reader["ResourceName"],
                        Evt = (int)reader["Evt"],
                        Endpoint = (string)reader["Endpoint"]
                    });
                }

                reader.Close();
                conn.Close();

                // 2) para cada sub, decidir se é MQTT ou HTTP
                foreach (var sub in subs)
                {
                    if (sub.Endpoint.StartsWith("mqtt://", StringComparison.OrdinalIgnoreCase))
                    {
                        SendMqttNotification(appName, contName, ci, evt, sub);
                    }
                    else if (sub.Endpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        SendHttpNotification(appName, contName, ci, evt, sub);
                    }
                }
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                // aqui não rebentas a request só por falhar notificação
            }
        }
        private void SendMqttNotification(string appName, string contName, ContentInstances ci, int evt, Subscriptions sub)
        {
            // Endpoint vem tipo: mqtt://api/somiod/appX/contY
            string topic = sub.Endpoint.Substring("mqtt://".Length);

            string broker = System.Configuration.ConfigurationManager.AppSettings["MqttBrokerAddress"];
            int port = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MqttBrokerPort"]); //Ambos prefeitos na web config

            var client = new MqttClient(broker, port, false, null, null, MqttSslProtocols.None);
            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);

            var payloadObj = new
            {
                resource = $"/api/somiod/{appName}/{contName}/{ci.ResourceName}",
                eventType = (evt == 1 ? "creation" : "deletion"),
                timestamp = DateTime.UtcNow.ToString("o")
            };

            string json = JsonConvert.SerializeObject(payloadObj);
            byte[] message = Encoding.UTF8.GetBytes(json);

            client.Publish(topic, message, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            client.Disconnect();
        }
        private void SendHttpNotification(string appName, string contName, ContentInstances ci, int evt, Subscriptions sub)
        {
            var bodyObj = new
            {
                resource = $"/api/somiod/{appName}/{contName}/{ci.ResourceName}",
                eventType = (evt == 1 ? "creation" : "deletion"),
                timestamp = DateTime.UtcNow.ToString("o")
            };

            string json = JsonConvert.SerializeObject(bodyObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var http = new System.Net.Http.HttpClient())
            {
                // como os teus controllers não são async, uso .Result
                var response = http.PostAsync(sub.Endpoint, content).Result;
                // podes ignorar ou logar response.StatusCode
            }
        }


        //      SUBSCRIPTIONS

        // POST api/somiod/{appName}/{contName}/subs
        [HttpPost]
        [Route("api/somiod/{appName}/{contName}/subs")]
        public IHttpActionResult PostSubscription(string appName, string contName, [FromBody] Subscriptions sub)
        {
            if (sub == null)
                return BadRequest("Subscription inválida.");

            if (string.IsNullOrWhiteSpace(sub.ResourceName))
                return BadRequest("resource-name é obrigatório.");

            if (sub.Evt != 1 && sub.Evt != 2)
                return BadRequest("evt deve ser 1 (creation) ou 2 (deletion).");

            if (string.IsNullOrWhiteSpace(sub.Endpoint))
                return BadRequest("endpoint é obrigatório.");

            sub.ResType = "subscription";
            sub.CreationDatetime = DateTime.UtcNow;

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // 1) Obter Id do container
                SqlCommand getContCmd = new SqlCommand(
                    @"SELECT c.Id
                      FROM Containers c
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName = @appName AND c.ResourceName = @contName", conn);

                getContCmd.Parameters.AddWithValue("@appName", appName);
                getContCmd.Parameters.AddWithValue("@contName", contName);

                var contResult = getContCmd.ExecuteScalar();
                if (contResult == null)
                {
                    conn.Close();
                    return NotFound();
                }

                int contId = (int)contResult;
                sub.Container_ID = contId;

                // 2) Verificar duplicado
                SqlCommand checkCmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM Subscriptions
                      WHERE Container_ID = @contId AND ResourceName = @name", conn);
                checkCmd.Parameters.AddWithValue("@contId", contId);
                checkCmd.Parameters.AddWithValue("@name", sub.ResourceName);
                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    conn.Close();
                    return Content(HttpStatusCode.Conflict,
                        $"Já existe uma subscription com o nome '{sub.ResourceName}' no container '{contName}'.");
                }

                // 3) Inserir subscription
                SqlCommand insertCmd = new SqlCommand(
                    @"INSERT INTO Subscriptions 
                        (ResourceName, ResType, CreationDateTime, Evt, Endpoint, Container_ID)
                      VALUES (@name, @resType, @creation, @evt, @endpoint, @contId);
                      SELECT SCOPE_IDENTITY();", conn);

                insertCmd.Parameters.AddWithValue("@name", sub.ResourceName);
                insertCmd.Parameters.AddWithValue("@resType", sub.ResType);
                insertCmd.Parameters.AddWithValue("@creation", sub.CreationDatetime);
                insertCmd.Parameters.AddWithValue("@evt", sub.Evt);
                insertCmd.Parameters.AddWithValue("@endpoint", sub.Endpoint);
                insertCmd.Parameters.AddWithValue("@contId", contId);

                var newIdObj = insertCmd.ExecuteScalar();
                if (newIdObj != null && newIdObj != DBNull.Value)
                {
                    sub.Id = Convert.ToInt32(newIdObj);
                }

                conn.Close();

                return Ok("Subscription criada com sucesso!");
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        // GET api/somiod/{appName}/{contName}/subs/{subName}
        [HttpGet]
        [Route("api/somiod/{appName}/{contName}/subs/{subName}")]
        public IHttpActionResult GetSubscription(string appName, string contName, string subName)
        {
            SqlConnection conn = null;
            Subscriptions sub = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"SELECT s.Id, s.ResourceName, s.ResType, s.CreationDateTime,
                             s.Evt, s.Endpoint, s.Container_ID
                      FROM Subscriptions s
                      INNER JOIN Containers c ON s.Container_ID = c.Id
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName = @appName
                        AND c.ResourceName = @contName
                        AND s.ResourceName = @subName", conn);

                cmd.Parameters.AddWithValue("@appName", appName);
                cmd.Parameters.AddWithValue("@contName", contName);
                cmd.Parameters.AddWithValue("@subName", subName);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    sub = new Subscriptions
                    {
                        Id = (int)reader["Id"],
                        ResourceName = (string)reader["ResourceName"],
                        ResType = (string)reader["ResType"],
                        CreationDatetime = (DateTime)reader["CreationDateTime"],
                        Evt = (int)reader["Evt"],
                        Endpoint = (string)reader["Endpoint"],
                        Container_ID = (int)reader["Container_ID"]
                    };
                }

                reader.Close();
                conn.Close();

                if (sub == null)
                    return NotFound();

                return Ok(sub);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        // DELETE api/somiod/{appName}/{contName}/subs/{subName}
        [HttpDelete]
        [Route("api/somiod/{appName}/{contName}/subs/{subName}")]
        public IHttpActionResult DeleteSubscription(string appName, string contName, string subName)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM Subscriptions
                      WHERE Id IN (
                        SELECT s.Id
                        FROM Subscriptions s
                        INNER JOIN Containers c ON s.Container_ID = c.Id
                        INNER JOIN Application a ON c.Application_ID = a.Id
                        WHERE a.ResourceName = @appName
                          AND c.ResourceName = @contName
                          AND s.ResourceName = @subName
                      )", conn);

                cmd.Parameters.AddWithValue("@appName", appName);
                cmd.Parameters.AddWithValue("@contName", contName);
                cmd.Parameters.AddWithValue("@subName", subName);

                int rows = cmd.ExecuteNonQuery();

                conn.Close();

                if (rows == 0)
                    return NotFound();

                return Ok($"Subscription '{subName}' eliminada com sucesso.");
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