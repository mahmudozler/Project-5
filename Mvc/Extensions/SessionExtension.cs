/*using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace MVC.Extension
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static bool IfExists(this ISession session, string key)
        {
            return session.Get(key) != null;
        }
    }

    public class UserSession
    {
        public UserSession(int _ID,string _Username){
            this.ID = _ID;
            this.Username = _Username;
        }

        public int ID { get; set; }
        public string Username { get; set; }
    }*/
