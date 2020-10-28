using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Medicale_Updater
{

    class properties
    {
        private String propertyLocation = "";
        private List<KeyValuePair<String, String>> pairs;

        public properties()
        {
            StringBuilder sb = new StringBuilder();
       
                sb.Append(Environment.GetEnvironmentVariable("localappdata")).Append(Path.DirectorySeparatorChar)
                .Append("Cabinet Medical").Append(Path.DirectorySeparatorChar).Append("config")
                        .Append(Path.DirectorySeparatorChar).Append("medical.properties");
            propertyLocation = sb.ToString();
           
            pairs = new List<KeyValuePair<string, string>>();

            load();
        }
      
        private void load()
        {
            String tm = System.IO.File.ReadAllText(propertyLocation);
            String[] df = tm.Split("\r");




            for (int i = 0; i < df.Length ; i++)
            {
                String[] h = df[i].Split("=");
                if (h.Length == 2)
                {
                    if (h[0].StartsWith('\n'))
                    {
                        h[0] = h[0].Substring(1);
                    }
                   
                    pairs.Add(new KeyValuePair<String, String>(h[0], h[1]));
                }
            }

        }

        public String getPropertie(String key)
        {
            foreach (var i in pairs)
            {
                if (i.Key.ToLower().Equals(key.ToLower()))
                {
                    return i.Value;
                }
            }
            return "";
        }
        public void addProperty(String key, String value)
        {
            pairs.Add(new KeyValuePair<String, String>(key, value));
        }
        public bool deleteProperty(String key)
        {
            foreach (var i in pairs)
            {
                if (i.Key.ToLower().Equals(key.ToLower()))
                {
                    pairs.Remove(i);
                    return true;
                }
            }
            return false;
        }
        public bool update(String key, String value)
        {
            if (deleteProperty(key))
            {
                addProperty(key, value);
                return true;
            }
            return false;



        }
        public void save()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var pair in pairs)
            {
               
                    sb.Append(pair.Key + "=" + pair.Value + '\r');
                
            }
            if (sb.Length > 0)
            {
                File.WriteAllText(propertyLocation, sb.ToString());
            }
        }
    }
}
