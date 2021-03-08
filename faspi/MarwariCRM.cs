using DeviceId;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace faspi
{
   static class MarwariCRM
    {
        public static string errorMessage { get; set; }

        public static async Task<double> APIBalanceAsync()
        {
            string USID = File.ReadAllText("ClientUSID.txt");
            string deviceId = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().AddUserName().ToString();
            string ComputerName = System.Environment.MachineName;
            RestClient newClient = new RestClient("http://crm.faspi.in/api/");
            RestRequest request = new RestRequest("MarwariAPI");
            request.AddHeader("Authorization", "TWFyd2FyU290d2FyZTpNYXJ3YXJpQCMxMjM=");
            //request.AddParameter("USID", "nalandaxpress@gmail.com");
            request.AddParameter("USID", USID);
            request.AddParameter("DeviceId", deviceId);
            request.AddParameter("ComputerName", ComputerName);


            var response = await newClient.ExecuteGetTaskAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return double.Parse(response.Content);
            }
            else
            {
                errorMessage = response.StatusCode + " " + response.Content;
                //MessageBox.Show(response.StatusCode + " " + response.Content);
                return 0;
            }
        }

        public static double APIBalance()
        {
            string USID = File.ReadAllText("ClientUSID.txt");
            string deviceId = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().AddUserName().ToString();
            string ComputerName = System.Environment.MachineName;
            RestClient newClient = new RestClient("http://crm.faspi.in/api/");
            RestRequest request = new RestRequest("MarwariAPI");
            request.AddHeader("Authorization", "TWFyd2FyU290d2FyZTpNYXJ3YXJpQCMxMjM=");
            //request.AddParameter("USID", "nalandaxpress@gmail.com");
            request.AddParameter("USID", USID);
            request.AddParameter("DeviceId", deviceId);
            request.AddParameter("ComputerName", ComputerName);
            var response = newClient.Get(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return double.Parse(response.Content);
            }
            else
            {
                errorMessage = response.StatusCode + " " + response.Content;
                //MessageBox.Show(response.StatusCode + " " + response.Content);
                return 0;
            }
        }


        public static void DeductAPI(string remark, double quantity)
        {
            string USID = File.ReadAllText("ClientUSID.txt");
            string deviceId = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().AddUserName().ToString();
            string ComputerName = System.Environment.MachineName;
            RestClient newClient = new RestClient("http://crm.faspi.in/api/");
            RestRequest request = new RestRequest("MarwariAPI", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;

            request.AddHeader("Authorization", "TWFyd2FyU290d2FyZTpNYXJ3YXJpQCMxMjM=");
            request.AddHeader("USID", USID);
            request.AddHeader("DeviceId", deviceId);
            request.AddHeader("ComputerName", ComputerName);

            Dictionary<string, string> obj = new Dictionary<string, string>();
            obj.Add("Remark", remark);
            obj.Add("Quantity", quantity.ToString());

            request.AddJsonBody(obj);

            IRestResponse response = newClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
            }
            else
            {
                throw new Exception(response.ErrorMessage);
            }



        }

        public static int Validate()
        {
            string deviceId = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().AddUserName().ToString();
            if (!Utility.CheckForInternetConnection())
            {
                //off line
                if (File.Exists("token\\useless.dat"))
                {
                    string tokenJson = File.ReadAllText("token\\useless.dat", Encoding.UTF8).Base64Decode();


                    MyToken myToken = new MyToken();

                    var stringmyToken = JsonConvert.DeserializeObject(tokenJson).ToString();

                    MyToken result = JsonConvert.DeserializeObject<MyToken>(stringmyToken);

                    if (deviceId != result.DeviceId)
                    {
                        return 0;
                    }

                    if (long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) < result.ExpireOn)
                    {
                        if (result.LastOffLineRun == 0)
                        {
                            if (long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) < result.TokenOn)
                            {
                                return 0;
                            }
                        }
                        else if (long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) < result.LastOffLineRun)
                        {
                            return 0;
                        }

                        result.LastOffLineRun = long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss"));
                        
                        string json = JsonConvert.SerializeObject(result);
                        
                        File.WriteAllText("token\\useless.dat", json.Base64Encode());

                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }


            string USID = File.ReadAllText("ClientUSID.txt");

            string ComputerName = System.Environment.MachineName;
            //RestClient newClient = new RestClient("http://localhost:4311/api/");
            RestClient newClient = new RestClient("http://crm.faspi.in/api/");
            RestRequest request = new RestRequest("Licence");
            request.AddHeader("Authorization", "TWFyd2FyU290d2FyZTpNYXJ3YXJpQCMxMjM=");
           request.AddParameter("USID", "nalandaxpress@gmail.com");
            request.AddHeader("USID", USID);
            request.AddHeader("DeviceId", deviceId);
            request.AddHeader("ComputerName", ComputerName);


            var response = newClient.Get(request);

            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!Directory.Exists("token"))
                {
                    System.IO.Directory.CreateDirectory("token");
                }

                File.WriteAllText("token\\useless.dat", response.Content.Base64Encode());

                //MessageBox.Show(response.Content);
                //bool status = (response.Content == "1");
                //if (status == false) { errorMessage = "STOP"; }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int Surrender()
        {
            string USID = File.ReadAllText("ClientUSID.txt");
            string deviceId = new DeviceIdBuilder().AddProcessorId().AddMotherboardSerialNumber().AddUserName().ToString();
            string ComputerName = System.Environment.MachineName;
            RestClient newClient = new RestClient("http://crm.faspi.in//api/");
            RestRequest request = new RestRequest("Surrender");
            request.AddHeader("Authorization", "TWFyd2FyU290d2FyZTpNYXJ3YXJpQCMxMjM=");
            //request.AddParameter("USID", "nalandaxpress@gmail.com");
            request.AddParameter("USID", USID);
            request.AddParameter("DeviceId", deviceId);
            request.AddParameter("ComputerName", ComputerName);
            var response = newClient.Get(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MessageBox.Show(response.Content);
                //bool status = (response.Content == "1");
                //if (status == false) { errorMessage = "STOP"; }
                return 1;
            }
            return 0;
        }
    }

    class MyToken
    {
        public string DeviceId { get; set; }
        public long RegistionTime { get; set; }
        public long LastPingTime { get; set; }
        public long TokenOn { get; set; }
        public long ExpireOn { get; set; }
        public long LastOffLineRun { get; set; } 

    }
}
