using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace API
{
    public static class Service
    {
        // http
        private const string Protocol = "http";

        // local
        // private const string IPAddress = "127.0.0.1";
        // LAN
        private const string IPAddress = "172.26.59.173";

        // port
        private const string Port = "2333";

        private const string BaseApi = Protocol + "://" + IPAddress + ":" + Port;

        // account 
        public const string Login = BaseApi + "/" + "login";
        public const string Register = BaseApi + "/" + "register";
        public const string UpdatePassword = BaseApi + "/" + "password";

        // game
        private const string BaseGame = BaseApi + "/" + "game";
        public const string Archive = BaseGame + "/" + "archive";
        public const string Play = BaseGame + "/" + "play";

        // player
        public const string Player = BaseApi + "/" + "player";

        // character
        public const string Character = BaseApi + "/" + "character";

        // structure
        public const string Structure = BaseApi + "/" + "structure";

        private static HttpClient Client()
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("token", PlayerPrefs.GetString("token", ""));
            return http;
        }

        public static Task<HttpResponseMessage> GET(string url, Dictionary<string, string> param)
        {
            if (param != null)
            {
                url += "?";
                foreach (var item in param)
                {
                    url += item.Key + "=" + item.Value + "&";
                }
            }

            return Client().GetAsync(url);
        }

        public static Task<HttpResponseMessage> POST(string url, Dictionary<string, string> param)
        {
            var http = Client();

            if (param != null)
            {
                var keyValuePairs = new List<KeyValuePair<string, string>>();
                foreach (var item in param)
                {
                    keyValuePairs.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                }

                return http.PostAsync(url, new FormUrlEncodedContent(keyValuePairs));
            }

            return http.PostAsync(url, null);
        }
        
        public static Task<HttpResponseMessage> PUT(string url, Dictionary<string, string> param)
        {
            var http = Client();

            if (param != null)
            {
                var keyValuePairs = new List<KeyValuePair<string, string>>();
                foreach (var item in param)
                {
                    keyValuePairs.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                }

                return http.PutAsync(url, new FormUrlEncodedContent(keyValuePairs));
            }

            return http.PutAsync(url, null);
        }
    }
}