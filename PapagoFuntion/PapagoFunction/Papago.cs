using Microsoft.Extensions.Configuration;
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

        public string GetWhatIsLang(string query, string papagoID, string papagoPW)
        {
            string url = "https://openapi.naver.com/v1/papago/detectLangs";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", papagoID);
            request.Headers.Add("X-Naver-Client-Secret", papagoPW);
            request.Method = "POST";
            byte[] byteDataParams = Encoding.UTF8.GetBytes("query=" + query);
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
            Console.WriteLine(text);

            JObject jObject = JObject.Parse(text);

            return jObject["langCode"].ToString();
        }

        public string GetTransResult(string query, string papagoID, string papagoPW, string lanSource = "ko", string lanTarget = "ja")
        {
            string url = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("X-Naver-Client-Id", papagoID);
            request.Headers.Add("X-Naver-Client-Secret", papagoPW);
            request.Method = "POST";
            byte[] byteDataParams = Encoding.UTF8.GetBytes($"source={lanSource}&target={lanTarget}&text={query}");
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
            Console.WriteLine(text);

            JObject jObject = JObject.Parse(text);            
            return jObject["message"]["result"]["translatedText"].ToString();
        }
    }
}