using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FESA.SCM.UserLoadJob.ConsultaUsuarios;
using Microsoft.Azure.WebJobs;
using RestSharp;
using FESA.SCM.Common;

namespace FESA.SCM.UserLoadJob {
    public class Functions {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log) {
            log.WriteLine(message);
        }


        [NoAutomaticTrigger]
        public static void LoadUsersFESA() {
            var salt = CreateSalt();
            var encPass = Encrypt($"{salt}password");

            var users = new List<dynamic>();
            using (var client = new ConsultaADClient()) {
                var fesaUSers = client.ListarTodos(encPass);
                foreach (var clsAdItem in fesaUSers) {
                    if (clsAdItem.CodEmp == null) {
                        continue;
                    }
                    var centroCosto = new List<string>();
                    bool isNumber;
                    int office;
                    if (string.IsNullOrEmpty(clsAdItem.CentroCosto)) {
                        continue;
                    } else {
                        centroCosto = clsAdItem.CentroCosto.Split('-').Reverse().ToList<string>();
                        if (centroCosto.Count == 1) {
                            continue;
                        }
                        isNumber = Int32.TryParse(centroCosto[1], out office);
                    }
                    int userType;
                    if (string.IsNullOrEmpty(clsAdItem.TipoTrabajador)) {
                        continue;
                    } else {
                        userType = (clsAdItem.TipoTrabajador == "P4" || clsAdItem.TipoTrabajador == "P5") ? 0 : 1;
                    }

                    //if (clsAdItem.EMail != null) {
                    //    if (clsAdItem.EMail.Contains("CARLOS.AGAMA")) {
                    //    }
                    //}


                    if (!users.Where(x => x.FesaUserId == clsAdItem.CodEmp).Any()) {
                        users.Add(new {
                            FesaUserId = clsAdItem.CodEmp,
                            UserName = clsAdItem.EMail == null ? "" : clsAdItem.EMail.Split('@').ToList()[0],
                            Name = $"{clsAdItem.Nombres} {clsAdItem.Apellidos}",
                            Email = clsAdItem.EMail,
                            Office = office,
                            CostCenter = centroCosto[0],
                            Phone = clsAdItem.Cell?.Replace("(51) ", string.Empty).Replace("-", string.Empty),
                            Photo = "http://bucare.io/wp-content/uploads/2015/09/boy-512.png",
                            Role = new {
                                Id = "6890da7b-ca7d-49cf-9faa-355f8de575b3"
                            },
                            UserType = userType,
                            UserStatus = 0,
                            Password = "Fesa@Azure",
                            Celullar = clsAdItem.Cell,
                            Rpm = clsAdItem.RPM
                        });
                    }
                }
            }

            //var ids = users.Select(x => x.FesaUserId).ToList();
            //var item = users.Where(x => x.Email != null && x.Email.Contains("CARLOS.AGAMA")).FirstOrDefault();

            //const string api = "http://localhost:37023/IdentityApi.svc/rest";
            const string api = "http://apiprod-identity.azurewebsites.net/IdentityApi.svc/rest";
            var restClient = new RestClient(api);
            var request = new RestRequest("syncusers");
            request.AddJsonBody(new {
                users
            });
            var response = restClient.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
            Console.WriteLine(response.StatusCode);
        }

        [NoAutomaticTrigger]
        public static void LoadUsers() {
            //try
            //{
            var salt = CreateSalt();
            var encPass = Encrypt($"{salt}password");

            var users = new List<dynamic>();
            using (var client = new ConsultaADClient()) {
                var fesaUSers = client.ListarTodos(encPass);
                foreach (var clsAdItem in fesaUSers) {
                    if (clsAdItem.CodEmp == null) {
                        continue;
                    }
                    var centroCosto = new List<string>();
                    bool isNumber;
                    int office;
                    if (string.IsNullOrEmpty(clsAdItem.CentroCosto)) {
                        continue;
                    } else {
                        centroCosto = clsAdItem.CentroCosto.Split('-').Reverse().ToList<string>();
                        if (centroCosto.Count == 1) {
                            continue;
                        }
                        isNumber = Int32.TryParse(centroCosto[1], out office);
                    }
                    int userType;
                    if (string.IsNullOrEmpty(clsAdItem.TipoTrabajador)) {
                        continue;
                    } else {
                        userType = (clsAdItem.TipoTrabajador == "P4" || clsAdItem.TipoTrabajador == "P5") ? 0 : 1;
                    }

                    if (clsAdItem.EMail != null) {
                        if (clsAdItem.EMail.Contains("CARLOS.AGAMA")) {
                        }
                    }


                    if (!users.Where(x => x.FesaUserId == clsAdItem.CodEmp).Any()) {
                        users.Add(new {
                            FesaUserId = clsAdItem.CodEmp,
                            UserName = clsAdItem.EMail == null ? "" : clsAdItem.EMail.Split('@').ToList()[0],
                            Name = $"{clsAdItem.Nombres} {clsAdItem.Apellidos}",
                            Email = clsAdItem.EMail,
                            Office = office,
                            CostCenter = centroCosto[0],
                            Phone = clsAdItem.Cell?.Replace("(51) ", string.Empty).Replace("-", string.Empty),
                            Photo = "http://bucare.io/wp-content/uploads/2015/09/boy-512.png",
                            Role = new {
                                Id = "6890da7b-ca7d-49cf-9faa-355f8de575b3"
                            },
                            UserType = userType,
                            UserStatus = 0,
                            Password = "Fesa@Azure",
                            Celullar = clsAdItem.Cell,
                            Rpm = clsAdItem.RPM
                        });
                    }
                }
            }

            var ids = users.Select(x => x.FesaUserId).ToList();
            var item = users.Where(x => x.Email != null && x.Email.Contains("CARLOS.AGAMA")).FirstOrDefault();
 
            const string api = "http://localhost:37023/IdentityApi.svc/rest";
            var restClient = new RestClient(api);
            var request = new RestRequest("syncusers");
            request.AddJsonBody(new {
                users
            });
            var response = restClient.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
            Console.WriteLine(response.StatusCode);
            //}
            //catch (Exception ex)
            //{
            //    Log.Write(ex);
            //    throw;
            //}

        }
        [NoAutomaticTrigger]
        public static void LoadUsersJob() {
            //try
            //{
            var salt = CreateSalt();
            var encPass = Encrypt($"{salt}password");

            var users = new List<dynamic>();
            using (var client = new ConsultaADClient()) {
                var fesaUSers = client.ListarTodos(encPass);
                foreach (var clsAdItem in fesaUSers) {
                    if (clsAdItem.CodEmp == null) {
                        continue;
                    }

                    //if (clsAdItem.EMail != null) {
                    //    if (clsAdItem.EMail.ToUpper().Contains("CARLOS.AGAMA")) {
                    //    }
                    //}


                    var centroCosto = new List<string>();
                    bool isNumber;
                    int office;
                    if (string.IsNullOrEmpty(clsAdItem.CentroCosto)) {
                        continue;
                    } else {
                        centroCosto = clsAdItem.CentroCosto.Split('-').Reverse().ToList<string>();
                        if (centroCosto.Count == 1) {
                            continue;
                        }
                        isNumber = Int32.TryParse(centroCosto[1], out office);
                    }
                    int userType;
                    if (string.IsNullOrEmpty(clsAdItem.TipoTrabajador)) {
                        continue;
                    } else {
                        userType = (clsAdItem.TipoTrabajador == "P4" || clsAdItem.TipoTrabajador == "P5") ? 0 : 1;
                    }



                    users.Add(new {
                        FesaUserId = clsAdItem.CodEmp,
                        //UserName = clsAdItem.Login,
                        Name = $"{clsAdItem.Nombres} {clsAdItem.Apellidos}",
                        Email = clsAdItem.EMail,
                        Office = office,
                        CostCenter = centroCosto[0],
                        Phone = clsAdItem.Cell?.Replace("(51) ", string.Empty).Replace("-", string.Empty),
                        Photo = "http://bucare.io/wp-content/uploads/2015/09/boy-512.png",
                        Role = new {
                            Id = "6890da7b-ca7d-49cf-9faa-355f8de575b3"
                        },
                        UserType = userType,
                        //UserStatus = 0,
                        //Password = "Fesa@Azure",
                        Celullar = clsAdItem.Cell,
                        Rpm = clsAdItem.RPM
                    });
                }
            }

            //var a = new List<dynamic>(users.Skip(10).Take(10));

            //users = new List<dynamic>(users.Skip(10).Take(10));
            const string api = "http://apiprod-identity.azurewebsites.net/IdentityApi.svc/rest";
            var restClient = new RestClient(api);
            var request = new RestRequest("syncusers");
            request.AddJsonBody(new {
                users
            });
            var response = restClient.ExecutePostTaskAsync(request).GetAwaiter().GetResult();
            Console.WriteLine(response.StatusCode);
            //}
            //catch (Exception ex)
            //{
            //    Log.Write(ex);
            //    throw;
            //}

        }

        private static string Encrypt(string pass) {
            var iv = Encoding.ASCII.GetBytes("CaTFerre");
            var key = Convert.FromBase64String("bgBkAHQAZQB5AHMANwAyAGEAbAByAGwA");
            var buffer = Encoding.UTF8.GetBytes(pass);
            var des = new TripleDESCryptoServiceProvider {
                Key = key,
                IV = iv
            };
            return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        }

        private static string CreateSalt() {
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[32];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

    }
}
