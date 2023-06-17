using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Encryption
{

    public static class Encryptio_Utilis
    {
        public static async Task PostOrderEncrypt(string publicKey, string key, dynamic data, dynamic copyText, string listKey = null)
        {
            if (data is List<dynamic>)
            {
                var newList = new List<dynamic>();
                foreach (var i in data)
                {
                    if (i is Dictionary<string, dynamic>)
                    {
                        var newDeepCopy = new Dictionary<string, dynamic>(i);
                        await PostOrderEncrypt(publicKey, null, i, newDeepCopy);
                        newList.Add(newDeepCopy);
                    }
                    else if (i is List<dynamic>)
                    {
                        await PostOrderEncrypt(publicKey, key, i, copyText, listKey);
                    }
                    else
                    {
                        newList.Add(await Encrypt(publicKey, i.ToString() + ":::bob_::_johan::sixer" + i.GetType()));
                    }
                }
                copyText.Remove(listKey);
                copyText.Add(listKey, newList);
                return;
            }
            if (!(data is Dictionary<string, dynamic>))
            {
                copyText[await Encrypt(publicKey, key)] = await Encrypt(publicKey, data.ToString() + ":::bob_::_johan::sixer" + data.GetType());
            }

            foreach (var i in data.Keys)
            {
                var encrypted = await Encrypt(publicKey, i);
                copyText[encrypted] = copyText[i];
                copyText.Remove(i);
                if (copyText[encrypted] is string || copyText[encrypted] is int || copyText[encrypted] is float || copyText[encrypted] is bool)
                {
                    if (copyText[encrypted].ToString().Length < 40)
                    {
                        copyText[encrypted] = await Encrypt(publicKey, copyText[encrypted].ToString() + ":::bob_::_johan::sixer" + copyText[encrypted].GetType());
                    }
                    else
                    {
                        copyText[encrypted] = copyText[encrypted].ToString();
                    }
                }
                else if (copyText[encrypted] is List<dynamic>)
                {
                    await PostOrderEncrypt(publicKey, i, data[i], copyText, encrypted);
                }
                else
                {
                    await PostOrderEncrypt(publicKey, i, data[i], copyText[encrypted]);
                }
            }
        }

        public static  async Task PostOrderDecrypt(string privateKey, string key, dynamic data, Dictionary<string, dynamic> copyText, string listKey = null)
        {
            if (data is List<dynamic>)
            {
                var newList = new List<dynamic>();
                foreach (var i in data)
                {
                    if (i is Dictionary<string, dynamic>)
                    {
                        var newDeepCopy = new Dictionary<string, dynamic>(i);
                        await PostOrderDecrypt(privateKey, null, i, newDeepCopy);
                        newList.Add(newDeepCopy);
                    }
                    else if (i is List<dynamic>)
                    {
                        await PostOrderDecrypt(privateKey, key, i, copyText, listKey);
                    }
                    else
                    {
                        newList.Add(await ParseWord(await Decrypt(privateKey, i)));
                    }
                }
                copyText.Remove(listKey);
                copyText.Add(listKey, newList);
                return;
            }

            if (!(data is Dictionary<string, dynamic>))
            {
                copyText[await Decrypt(privateKey, key)] = await Decrypt(privateKey, data.ToString());
            }

            foreach (var i in data.Keys)
            {
                var encrypted = await Decrypt(privateKey, i.ToString());
                copyText[encrypted] = copyText[i.ToString()];
                if (copyText[encrypted] is string || copyText[encrypted] is int || copyText[encrypted] is float)
                {
                    try
                    {
                        copyText[encrypted] = await ParseWord(await Decrypt(privateKey, copyText[encrypted].ToString()));
                    }
                    catch
                    {
                        copyText[encrypted] = copyText[encrypted].ToString();
                    }
                }
                else if (copyText[encrypted] is List<dynamic>)
                {
                    await PostOrderDecrypt(privateKey, i.ToString(), data[i], copyText, encrypted);
                }
                else
                {
                    await PostOrderDecrypt(privateKey, i.ToString(), data[i], copyText[encrypted]);
                }
            }
        }

        public static async Task<dynamic> ParseWord(string inputString)
        {
            var addedWord = ":::bob_::_johan::sixer";
            var index = inputString.IndexOf(addedWord);
            var add = inputString.Substring(index);
            var real = inputString.Replace(add, "");
            if (add.Contains("int"))
            {
                return int.Parse(real);
            }
            if (add.Contains("float"))
            {
                return float.Parse(real);
            }
            if (add.Contains("str"))
            {
                return real;
            }
            if (add.Contains("bool"))
            {
                return bool.Parse(add);
            }
            return null;
        }

        public static async Task<string> Encrypt(string publicKey, string data)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);

            var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(encryptedData);
        }

        public static async Task<string> Decrypt(string privateKey, string data)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(privateKey);

            var decryptedData = rsa.Decrypt(Convert.FromBase64String(data), RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(decryptedData);
        }

    }

}