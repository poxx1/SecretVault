using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace LocalKeyVault
{
    public class Utilities
    {
        #region MD5 Encryption
        public string MD5Gen(string secret)
        {
            var code = new UnicodeEncoding();
            var md5 = new MD5CryptoServiceProvider();
            byte[] ByteS = code.GetBytes(secret);
            byte[] Hash = md5.ComputeHash(ByteS);

            return Hash.ToString();
        }

        public string Encrypt(string secret)
        {
            string key = "hash";
            byte[] keyArray;
            byte[] encode = UTF8Encoding.UTF8.GetBytes(secret);

            try
            {
                var md5 = new MD5CryptoServiceProvider();
                var t = new TripleDESCryptoServiceProvider();

                keyArray = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                md5.Clear();

                t.Key = keyArray;
                t.Mode = CipherMode.ECB;
                t.Padding = PaddingMode.PKCS7;
                ICryptoTransform e = t.CreateEncryptor();
                byte[] result = e.TransformFinalBlock(encode, 0, encode.Length);
                t.Clear();

                secret = Convert.ToBase64String(result, 0, result.Length);
            }
            catch (Exception)
            {
                throw;
            }

            return secret;
        }

        public string Decrypt(string secret)
        {
            string key = "hash";

            try
            {
                byte[] keyArray;
                byte[] decode = Convert.FromBase64String(secret);

                var md5 = new MD5CryptoServiceProvider();
                var t = new TripleDESCryptoServiceProvider();

                keyArray = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                md5.Clear();

                t.Key = keyArray;
                t.Mode = CipherMode.ECB;
                t.Padding = PaddingMode.PKCS7;

                ICryptoTransform ict = t.CreateDecryptor();
                byte[] resultado = ict.TransformFinalBlock(decode, 0, decode.Length);
                t.Clear();

                secret = UTF8Encoding.UTF8.GetString(resultado);
            }
            catch (Exception)
            {
                throw;
            }

            return secret;
        }

        #endregion

        #region Document Hashes
        public bool AddSecret(string name, string hash)
        {
            List<SecretsModel> secretsList = new List<SecretsModel>();
            StreamReader sr = new StreamReader("secretList.json");
            var jObj = JObject.Parse(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();

            foreach (var item in jObj["Secrets"])
            {
                var obj = item.ToObject<SecretsModel>();
                secretsList.Add(obj);
            }

            secretsList.Add(new SecretsModel { name = name, secret = hash });

            var newJson = JsonConvert.SerializeObject(secretsList, Formatting.Indented);

            var fullJson = "{\"Secrets\": " + newJson +"}";
            
            StreamWriter sw = new StreamWriter("secretList.json");
            sw.Write(fullJson);
            sw.Close();

            return true;
        }

        public bool EditSecret(string name, string hash)
        {
            List<SecretsModel> secretsList = new List<SecretsModel>();
            StreamReader sr = new StreamReader("secretList.json");
            var jObj = JObject.Parse(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();

            foreach (var item in jObj["Secrets"])
            {
                var obj = item.ToObject<SecretsModel>();
                if (obj.name != name)
                    secretsList.Add(obj);
                else
                    secretsList.Add(new SecretsModel { name = name, secret = hash });
            }
            var newJson = JsonConvert.SerializeObject(secretsList, Formatting.Indented);

            var fullJson = "{\"Secrets\": " + newJson + "}";

            StreamWriter sw = new StreamWriter("secretList.json");
            sw.Write(fullJson);
            sw.Close();

            return true;
        }

        public bool RemoveSecret(string name, string hash)
        {
            List<SecretsModel> secretsList = new List<SecretsModel>();
            StreamReader sr = new StreamReader("secretList.json");
            var jObj = JObject.Parse(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();

            foreach (var item in jObj["Secrets"])
            {
                var obj = item.ToObject<SecretsModel>();
                if (obj.name != name)
                    secretsList.Add(obj);
                else
                    secretsList.Remove(new SecretsModel { name = name, secret = hash });
            }
            var newJson = JsonConvert.SerializeObject(secretsList, Formatting.Indented);

            var fullJson = "{\"Secrets\": " + newJson + "}";

            StreamWriter sw = new StreamWriter("secretList.json");
            sw.Write(fullJson);
            sw.Close();

            return true;
        }

        public List<SecretsModel> ListSecrets()
        {
            StreamReader sr = new StreamReader("secretList.json");
            SecretsModel secret = new SecretsModel();
            List<SecretsModel> secretsList = new List<SecretsModel>();

            var jObj = JObject.Parse(sr.ReadToEnd());
            sr.Close();
            sr.Dispose();

            foreach (var item in jObj["Secrets"])
            {
                var obj = item.ToObject<SecretsModel>();
                secretsList.Add(obj);
            }

            return secretsList;
        }

        public SecretsModel Deserializer(string json)
        {
            return JsonConvert.DeserializeObject<SecretsModel>(json);
        }

        public string Serializer(SecretsModel secret)
        {
            return JsonConvert.SerializeObject(secret);
        }
        #endregion

        #region Login
        public bool Check(string hash)
        {
            StreamReader sr = new StreamReader("secretList.json");
            SecretsModel secret = new SecretsModel();
            List<SecretsModel> secretsList = new List<SecretsModel>();

            var jObj = JObject.Parse(sr.ReadToEnd());

            foreach (var item in jObj["Login"])
            {
                if (hash == item.ToObject<SecretsModel>().secret)
                    return true;
            }
            return false;
        }
        #endregion
    }
}