using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace PapagoFunction
{
    internal class Papago
    {
        public Papago()
        {

        }

        public string GetTransResult(string query)
        {
            string url = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "nF5lBrmaVoNmaaG4gU9r");
            request.Headers.Add("X-Naver-Client-Secret", "O6OyRU_9k_");
            request.Method = "POST";
            byte[] byteDataParams = Encoding.UTF8.GetBytes("source=ko&target=ja&text=" + query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();

            JObject jObject = JObject.Parse(text);

            Console.WriteLine(jObject["message"]["result"]["translatedText"].ToString());
            return jObject["message"]["result"]["translatedText"].ToString();
        }
    }
}