using Newtonsoft.Json;
using Somiod.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Somiod.Controllers
{
    [RoutePrefix("api/somiod")]
    public class SomiodController : ApiController
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Somiod.Properties.Settings.ConnStr"].ConnectionString;

        #region APPLICATION

        // GET api/somiod/{appName}
        [HttpGet]
        [Route("{appName}")]
        public IHttpActionResult GetApplication(string appName)
        {
            //é discovery?
            var hasHeader = Request.Headers.Contains("somiod-discovery");
            var headerValue = hasHeader ? Request.Headers.GetValues("somiod-discovery").FirstOrDefault() : null;

            // discovery
            if (hasHeader && !string.IsNullOrWhiteSpace(headerValue))
            {
                if (headerValue.Equals("container", StringComparison.OrdinalIgnoreCase))
                    return DiscoverContainersInApp(appName);

                if (headerValue.Equals("content-instance", StringComparison.OrdinalIgnoreCase))
                    return DiscoverContentInstancesInApp(appName);

                if (headerValue.Equals("subscription", StringComparison.OrdinalIgnoreCase))
                    return DiscoverSubscriptionsInApp(appName);

                return BadRequest("somiod-discovery deve ser: container | content-instance | subscription");
            }

            // GET APPLICATION normal 
            Application app = null;
            SqlConnection connApp = null;

            try
            {
                connApp = new SqlConnection(connectionString);
                connApp.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT * FROM Application WHERE ResourceName = @name", connApp);
                command.Parameters.AddWithValue("@name", appName);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
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
                connApp.Close();
            }
            catch (Exception e)
            {
                if (connApp != null && connApp.State == System.Data.ConnectionState.Open)
                    connApp.Close();
                return InternalServerError(e);
            }

            if (app == null)
                return NotFound();

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

                SqlCommand checkCmd =
                    new SqlCommand("SELECT COUNT(*) FROM Application WHERE ResourceName = @name", conn);
                checkCmd.Parameters.AddWithValue("@name", app.ResourceName);

                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    conn.Close();
                    return Content(HttpStatusCode.Conflict,
                        $"Application with resource-name '{app.ResourceName}' already exists.");
                }

                SqlCommand command = new SqlCommand(
                    @"INSERT INTO Application (ResourceName, ResType, CreationDateTime)
                      VALUES (@name, @resType, @creation);
                      SELECT SCOPE_IDENTITY();", conn);

                command.Parameters.AddWithValue("@name", app.ResourceName);
                command.Parameters.AddWithValue("@resType", app.ResType);
                command.Parameters.AddWithValue("@creation", app.CreationDatetime);

                app.Id = Convert.ToInt32(command.ExecuteScalar());

                conn.Close();

                //return Ok("Aplicação inserida com sucesso!");
                var location = new Uri(Request.RequestUri, $"api/somiod/{app.ResourceName}");
                return Created(location, app);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        // PUT api/somiod/{appName}
        [HttpPut]
        [Route("{appName}")]
        public IHttpActionResult PutApplication(string appName, [FromBody] Application app)
        {
            if (app == null)
                return BadRequest("Invalid application payload.");

            if (string.IsNullOrWhiteSpace(app.ResourceName))
                return BadRequest("resource-name is required.");

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Application
                      SET ResourceName = @newName
                      WHERE ResourceName = @oldName", conn);

                cmd.Parameters.AddWithValue("@newName", app.ResourceName);
                cmd.Parameters.AddWithValue("@oldName", appName);

                int rows = cmd.ExecuteNonQuery();

                conn.Close();

                if (rows == 0)
                    return NotFound();

                //return Ok("Aplicação atualizada com sucesso!");
                app.ResType = "application"; // opcional
                return Ok(app);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Console.WriteLine(e.Message);
                return InternalServerError(e);
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

                SqlCommand command =
                    new SqlCommand("DELETE FROM Application WHERE ResourceName = @name", conn);
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
        

        #endregion

        #region CONTAINER   

        // GET api/somiod/{appName}/{contName}
        [HttpGet]
        [Route("{appName}/{contName}")]
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
                    return NotFound();

                return Ok(cont);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        // POST api/somiod/{appName}
        [HttpPost]
        [Route("{appName}")]
        public IHttpActionResult PostContainer(string appName, [FromBody] Containers c)
        {
            if (c == null)
                return BadRequest("Container inválido.");

            c.ResType = "container";
            c.CreationDatetime = DateTime.UtcNow;

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // obter Id da aplicação
                SqlCommand getAppIdCmd =
                    new SqlCommand("SELECT Id FROM Application WHERE ResourceName = @appName", conn);
                getAppIdCmd.Parameters.AddWithValue("@appName", appName);

                var appResult = getAppIdCmd.ExecuteScalar();
                if (appResult == null)
                {
                    conn.Close();
                    return NotFound();
                }

                int appId = (int)appResult;
                c.Application_ID = appId;

                // verificar duplicado
                SqlCommand checkCmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM Containers
                      WHERE Application_ID = @appId AND ResourceName = @name", conn);
                checkCmd.Parameters.AddWithValue("@appId", appId);
                checkCmd.Parameters.AddWithValue("@name", c.ResourceName);

                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    conn.Close();
                    return Content(HttpStatusCode.Conflict,
                        $"Já existe um container com o nome '{c.ResourceName}' na aplicação '{appName}'.");
                }

                // inserir
                SqlCommand insertCmd = new SqlCommand(
                    @"INSERT INTO Containers (ResourceName, ResType, CreationDateTime, Application_ID)
                      VALUES (@name, @resType, @creation, @appId);
                      SELECT SCOPE_IDENTITY();", conn);

                insertCmd.Parameters.AddWithValue("@name", c.ResourceName);
                insertCmd.Parameters.AddWithValue("@resType", c.ResType);
                insertCmd.Parameters.AddWithValue("@creation", c.CreationDatetime);
                insertCmd.Parameters.AddWithValue("@appId", appId);

                c.Id = Convert.ToInt32(insertCmd.ExecuteScalar());

                conn.Close();
                //return Ok("Container inserido com sucesso!");
                var location = new Uri(Request.RequestUri, $"api/somiod/{appName}/{c.ResourceName}");
                return Created(location, c);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        

        // PUT api/somiod/{appName}/{contName}
        [HttpPut]
        [Route("{appName}/{contName}")]
        public IHttpActionResult PutContainer(string appName, string contName, [FromBody] Containers c)
        {
            if (c == null)
                return BadRequest("Container inválido.");

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Containers
                      SET ResourceName = @newName
                      WHERE ResourceName = @oldName
                      AND Application_ID = (
                        SELECT Id FROM Application WHERE ResourceName = @appName
                      )", conn);

                cmd.Parameters.AddWithValue("@newName", c.ResourceName);
                cmd.Parameters.AddWithValue("@oldName", contName);
                cmd.Parameters.AddWithValue("@appName", appName);

                int rows = cmd.ExecuteNonQuery();

                conn.Close();

                if (rows == 0)
                    return NotFound();

                //return Ok("Container atualizado com sucesso!");
                c.ResType = "container"; 
                return Ok(c);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Console.WriteLine(e.Message);
                return BadRequest("Erro ao atualizar o container");
            }
        }

        // DELETE api/somiod/{appName}/{contName}
        [HttpDelete]
        [Route("{appName}/{contName}")]
        public IHttpActionResult DeleteContainer(string appName, string contName)
        {
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"DELETE FROM Containers
                      WHERE ResourceName = @contName
                      AND Application_ID = (
                        SELECT Id FROM Application WHERE ResourceName = @appName
                      )", conn);

                cmd.Parameters.AddWithValue("@appName", appName);
                cmd.Parameters.AddWithValue("@contName", contName);

                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                if (rows == 0)
                    return NotFound();

                return Ok($"Container '{contName}' eliminado com sucesso!");
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Console.WriteLine(e.Message);
                return InternalServerError(e);
            }
        }

        #endregion

        #region CONTENT INSTANCES

        // POST api/somiod/{appName}/{contName}
        [HttpPost]
        [Route("{appName}/{contName}")]
        public IHttpActionResult PostContentInstance(string appName, string contName, [FromBody] ContentInstances ci)
        {
            if (ci == null)
                return BadRequest("Content-instance inválida.");

            if (string.IsNullOrWhiteSpace(ci.ResourceName))
                return BadRequest("resource-name é obrigatório.");

            if (string.IsNullOrWhiteSpace(ci.ContentType))
                return BadRequest("content-type é obrigatório.");

            if (ci.Content == null)
                return BadRequest("content é obrigatório.");

            ci.ResType = "content-instance";
            ci.CreationDatetime = DateTime.UtcNow;

            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

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

                ci.Container_ID = (int)contResult;

                SqlCommand checkCmd = new SqlCommand(
                    @"SELECT COUNT(*) FROM ContentInstances
                      WHERE Container_ID = @cid AND ResourceName = @name", conn);

                checkCmd.Parameters.AddWithValue("@cid", ci.Container_ID);
                checkCmd.Parameters.AddWithValue("@name", ci.ResourceName);

                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    conn.Close();
                    return Content(HttpStatusCode.Conflict,
                        $"Já existe uma content-instance com o nome '{ci.ResourceName}'.");
                }

                SqlCommand insertCmd = new SqlCommand(
                    @"INSERT INTO ContentInstances
                      (ResourceName, ResType, CreationDateTime, ContentType, Content, Container_ID)
                      VALUES (@name,@res,@dt,@ctype,@content,@cid);
                      SELECT SCOPE_IDENTITY();", conn);

                insertCmd.Parameters.AddWithValue("@name", ci.ResourceName);
                insertCmd.Parameters.AddWithValue("@res", ci.ResType);
                insertCmd.Parameters.AddWithValue("@dt", ci.CreationDatetime);
                insertCmd.Parameters.AddWithValue("@ctype", ci.ContentType);
                insertCmd.Parameters.AddWithValue("@content", ci.Content);
                insertCmd.Parameters.AddWithValue("@cid", ci.Container_ID);

                var newIdObj = insertCmd.ExecuteScalar();
                if (newIdObj != null && newIdObj != DBNull.Value)
                {
                    ci.Id = Convert.ToInt32(newIdObj);
                }

                conn.Close();

                NotifySubscribers(appName, contName, ci, 1);
                //return Ok("Content-instance inserida com sucesso!");
                var location = new Uri(Request.RequestUri, $"api/somiod/{appName}/{contName}/{ci.ResourceName}");
                return Created(location, ci);
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
        [Route("{appName}/{contName}/{ciName}")]
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
                      WHERE a.ResourceName=@app AND c.ResourceName=@cont AND ci.ResourceName=@ci", conn);

                cmd.Parameters.AddWithValue("@app", appName);
                cmd.Parameters.AddWithValue("@cont", contName);
                cmd.Parameters.AddWithValue("@ci", ciName);

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
        [Route("{appName}/{contName}/{ciName}")]
        public IHttpActionResult DeleteContentInstance(string appName, string contName, string ciName)
        {
            ContentInstances ciDeleted = null;
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand selectCmd = new SqlCommand(
                    @"SELECT ci.Id, ci.ResourceName, ci.ResType, ci.CreationDateTime, ci.ContentType, ci.Content
                      FROM ContentInstances ci
                      INNER JOIN Containers c ON ci.Container_ID = c.Id
                      INNER JOIN Application a ON c.Application_ID = a.Id
                      WHERE a.ResourceName=@appName AND c.ResourceName=@contName AND ci.ResourceName=@ciName", conn);

                selectCmd.Parameters.AddWithValue("@appName", appName);
                selectCmd.Parameters.AddWithValue("@contName", contName);
                selectCmd.Parameters.AddWithValue("@ciName", ciName);

                SqlDataReader reader = selectCmd.ExecuteReader();


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

                SqlCommand deleteCmd = new SqlCommand(@"DELETE FROM ContentInstances WHERE Id = @id", conn);

                deleteCmd.Parameters.AddWithValue("@id", ciDeleted.Id);
                int rows = deleteCmd.ExecuteNonQuery();

                conn.Close();

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
            //para cada subscription:
            //      - se for MQTT → publish
            //      - se for HTTP → HTTP POST
            Debug.WriteLine($"[Notify] Entrei no NotifySubscribers para {appName}/{contName}, evt={evt}");
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                // ir buscar subscriptions do container com o evt correto
                SqlCommand cmd = new SqlCommand(
                            @"SELECT s.ResourceName, s.Evt, s.Endpoint
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
                Debug.WriteLine($"[Notify] Encontrei {subs.Count} subscriptions.");

                // decidir se é MQTT ou HTTP (para cada sub)
                foreach (var sub in subs)
                {
                    Debug.WriteLine($"[Notify] Endpoint recebido: '{sub.Endpoint}'");

                    var endpoint = sub.Endpoint.Trim();
                    Debug.WriteLine($"[Notify] StartsWith mqtt:// ? {endpoint.StartsWith("mqtt://", StringComparison.OrdinalIgnoreCase)}");

                    if (endpoint.StartsWith("mqtt://", StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.WriteLine("[Notify] Enviando notificação MQTT...");
                        SendMqttNotification(appName, contName, ci, evt, sub);   // podes até passar o endpoint limpo se quiseres
                    }
                    else if (endpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        SendHttpNotification(appName, contName, ci, evt, sub);
                    }
                }
            }
            catch (SqlException ex)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    conn.Close();

                Debug.WriteLine("[Notify][SQL ERROR] " + ex.Message);
                foreach (SqlError err in ex.Errors)
                {
                    Debug.WriteLine("  -> " + err.Message);
                }

                throw;
            }

        }
        private void SendMqttNotification(string appName, string contName, ContentInstances ci, int evt, Subscriptions sub)
        {
            Debug.WriteLine(">>> ENTROU NO MQTT-TEST <<<");

            var client = new MqttClient(IPAddress.Parse("54.36.178.49"));
            string clientId = Guid.NewGuid().ToString();
            string endpoint = sub.Endpoint.Trim();

            if (!endpoint.StartsWith("mqtt://", StringComparison.OrdinalIgnoreCase))
            {
                Debug.WriteLine("[MQTT] Endpoint NÃO é MQTT.");
                return;
            }

            // Extrair o tópico (retira mqtt://)
            string topic = endpoint.Substring("mqtt://".Length);
            client.Connect(clientId);
            Debug.WriteLine("MQTT Connected? " + client.IsConnected);

            if (!client.IsConnected)
                Debug.WriteLine("Não ligou ao broker");

            
            var payloadObj = new
            {
                resource = $"/api/somiod/{appName}/{contName}/{ci.ResourceName}",
                eventType = (evt == 1 ? "creation" : "deletion"),
                timestamp = DateTime.UtcNow.ToString("o"),
                content = ci.Content
            };

            string json = JsonConvert.SerializeObject(payloadObj);
            byte[] msg = Encoding.UTF8.GetBytes(json);
            //string msg = "WEBAPI DIZ OLÁ";

            client.Publish(
                topic,
                msg,
                MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                true
            );

            Debug.WriteLine($"MQTT Published to '{topic}': {msg}");



            Debug.WriteLine("MQTT TEST DONE");
            
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
                var response = http.PostAsync(sub.Endpoint, content).Result;
            }
        }

        #endregion

        #region SUBSCRIPTION    

        // POST api/somiod/{appName}/{contName}/subs
        [HttpPost]
        [Route("{appName}/{contName}/subs")]
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

                // Obter Id do container
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

                // Inserir subscription
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

                //return Ok("Subscription criada com sucesso!");
                var location = new Uri(Request.RequestUri, $"api/somiod/{appName}/{contName}/subs/{sub.ResourceName}");
                return Created(location, sub);
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
        [Route("{appName}/{contName}/subs/{subName}")]
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
        [Route("{appName}/{contName}/subs/{subName}")]
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

        #endregion

        #region DISCOVERY
        // GET api/somiod  (discovery)
        [HttpGet]
        [Route("")]
        public IHttpActionResult DiscoverRoot()
        {
            var hasHeader = Request.Headers.Contains("somiod-discovery");
            var headerValue = hasHeader ? Request.Headers.GetValues("somiod-discovery").FirstOrDefault() : null;
            if (!hasHeader || string.IsNullOrWhiteSpace(headerValue))
                return BadRequest("Header 'somiod-discovery' é obrigatório.");

            if (headerValue.Equals("application", StringComparison.OrdinalIgnoreCase))
                return DiscoverApplicationsInternal();

            if (headerValue.Equals("content-instance", StringComparison.OrdinalIgnoreCase))
                return DiscoverAllContentInstancesInternal();

            if (headerValue.Equals("subscription", StringComparison.OrdinalIgnoreCase))
                return DiscoverAllSubscriptionsInternal();

            return BadRequest("somiod-discovery deve ser: application | content-instance | subscription");
        }

        private IHttpActionResult DiscoverContainersInApp(string appName)
        {
            var paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                var cmd = new SqlCommand(@"
                    SELECT c.ResourceName
                    FROM Containers c
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    WHERE a.ResourceName = @appName
                    ORDER BY c.Id;", conn);

                cmd.Parameters.AddWithValue("@appName", appName);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var cont = (string)reader["ResourceName"];
                    paths.Add($"api/somiod/{appName}/{cont}");
                }

                reader.Close();
                conn.Close();

                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
                return InternalServerError(e);
            }
        }

        private IHttpActionResult DiscoverContentInstancesInApp(string appName)
        {
            var paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                var cmd = new SqlCommand(@"
                    SELECT c.ResourceName AS Cont, ci.ResourceName AS Ci
                    FROM ContentInstances ci
                    INNER JOIN Containers c ON ci.Container_ID = c.Id
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    WHERE a.ResourceName = @appName
                    ORDER BY c.Id, ci.Id;", conn);

                cmd.Parameters.AddWithValue("@appName", appName);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var cont = (string)reader["Cont"];
                    var ci = (string)reader["Ci"];
                    paths.Add($"api/somiod/{appName}/{cont}/{ci}");
                }

                reader.Close();
                conn.Close();

                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
                return InternalServerError(e);
            }
        }



        private IHttpActionResult DiscoverSubscriptionsInApp(string appName)
        {
            var paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                var cmd = new SqlCommand(@"
                    SELECT c.ResourceName AS Cont, s.ResourceName AS Sub
                    FROM Subscriptions s
                    INNER JOIN Containers c ON s.Container_ID = c.Id
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    WHERE a.ResourceName = @appName
                    ORDER BY c.Id, s.Id;", conn);

                cmd.Parameters.AddWithValue("@appName", appName);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var cont = (string)reader["Cont"];
                    var sub = (string)reader["Sub"];
                    paths.Add($"api/somiod/{appName}/{cont}/subs/{sub}");
                }

                reader.Close();
                conn.Close();

                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
                return InternalServerError(e);
            }
        }

        private IHttpActionResult DiscoverApplicationsInternal()
        {
            var paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                var cmd = new SqlCommand("SELECT ResourceName FROM Application ORDER BY Id", conn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    paths.Add($"api/somiod/{(string)reader["ResourceName"]}");

                reader.Close();
                conn.Close();
                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
                return InternalServerError(e);
            }
        }

        private IHttpActionResult DiscoverAllContentInstancesInternal()
        {
            var paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                var cmd = new SqlCommand(@"
                    SELECT a.ResourceName AS App, c.ResourceName AS Cont, ci.ResourceName AS Ci
                    FROM ContentInstances ci
                    INNER JOIN Containers c ON ci.Container_ID = c.Id
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    ORDER BY a.Id, c.Id, ci.Id;", conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var app = (string)reader["App"];
                    var cont = (string)reader["Cont"];
                    var ci = (string)reader["Ci"];
                    paths.Add($"api/somiod/{app}/{cont}/{ci}");
                }

                reader.Close();
                conn.Close();
                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
                return InternalServerError(e);
            }
        }

        private IHttpActionResult DiscoverAllSubscriptionsInternal()
        {
            var paths = new List<string>();
            SqlConnection conn = null;

            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();

                var cmd = new SqlCommand(@"
                    SELECT a.ResourceName AS App, c.ResourceName AS Cont, s.ResourceName AS Sub
                    FROM Subscriptions s
                    INNER JOIN Containers c ON s.Container_ID = c.Id
                    INNER JOIN Application a ON c.Application_ID = a.Id
                    ORDER BY a.Id, c.Id, s.Id;", conn);

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var app = (string)reader["App"];
                    var cont = (string)reader["Cont"];
                    var sub = (string)reader["Sub"];
                    paths.Add($"api/somiod/{app}/{cont}/subs/{sub}");
                }

                reader.Close();
                conn.Close();
                return Ok(paths);
            }
            catch (Exception e)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open) conn.Close();
                return InternalServerError(e);
            }
        }

        #endregion
    }
}