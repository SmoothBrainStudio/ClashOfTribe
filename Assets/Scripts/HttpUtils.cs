using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HttpUtils
{
    public enum HttpVerb
    {
        GET, POST, PUT, DELETE
    }

    public class ContentType
    {
        public static string
            JSon = "application/json; charset=utf-8",
            XML = "application/xml; charset=utf-8",
            XMLText = "text/xml; charset=utf-8",
            Text = "text; charset=utf-8",
            PlainText = "text/plain; charset=utf-8",
            HTML = "text/html; charset=utf-8";
    }

    [Serializable]
    public class Settings
    {
        public string DomainName { get; set; }
        public int Port { get; set; }
        public bool Secure { get; set; }
    }

    public class RestClient
    {
        public static Settings ClientSettings { get; set; }

        //URL appelée
        public string EndPoint { get; set; }

        //le type de méthode désirée
        public HttpVerb Method { get; set; }

        //le type de contenu de la requête HTTP
        public string CurrentContentType { get; set; }

        //le contenu lui même
        public string Content { get; set; }

        public static bool Init()
        {
            //Si le fichier de conf n'existe pas
            if (!File.Exists("Rest.config"))
            {
                //on le crée
                using (FileStream fs = new FileStream("Rest.config", FileMode.Create))
                {
                    //on l'ouvre en écriture
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        //on écrit une structure vide de fichier XML que l'on pourra valorisé ultérieurement
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                        xmlSerializer.Serialize(sw, new Settings());
                    }
                }
                return false;
            }

            using (FileStream fs = new FileStream("Rest.config", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                    ClientSettings = (Settings)xmlSerializer.Deserialize(sr);
                    return true;
                }
            }
        }

        public void SetbaseClient(string serviceName, string parameters = "")
        {
            string protocol = "http";
            if (ClientSettings.Secure)
            {
                protocol += "s";
            }
            this.EndPoint = protocol + "://" + ClientSettings.DomainName + ":" + ClientSettings.Port + "/" + serviceName + parameters;
            this.CurrentContentType = ContentType.JSon;
        }


        public string SendRequest(string parameters = "")
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.EndPoint + parameters);
            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = CurrentContentType;


            if (!string.IsNullOrEmpty(Content) && Method != HttpVerb.GET)
            {
                request.AllowWriteStreamBuffering = false;
                request.ContentLength = Content.Length;

                Stream stream = request.GetRequestStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(Content);
                writer.Flush();
                writer.Close();
                stream.Close();

            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                string responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    string message = string.Format("Request failed. Received http {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                using (Stream stream = response.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }
                return responseValue;
            }
        }

        public string SendToWebService(string serviceName, HttpVerb method, Object? jsonSerializableObject, string parameters = "")
        {
            SetbaseClient(serviceName, parameters);
            this.Method = method;
            if (jsonSerializableObject != null)
            {
                this.Content = Newtonsoft.Json.JsonConvert.SerializeObject(jsonSerializableObject);
            }
            return SendRequest(parameters);
        }
    }
}
