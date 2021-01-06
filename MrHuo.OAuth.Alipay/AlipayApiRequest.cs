using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Text.Json;

namespace MrHuo.OAuth.Alipay
{
    public class AlipayApiRequest
    {
        private const string API_URL = "https://openapi.alipay.com/gateway.do";
        public string AppId { get; set; }
        public string PrivateRSAKey { get; set; }
        public string PublicRSAKey { get; set; }

        private T Invoke<T>(string httpMethod, string apiMethod, Dictionary<string, string> param = null)
        {
            if (param == null)
            {
                param = new Dictionary<string, string>();
            }
            param.Add("app_id", AppId);
            param.Add("method", apiMethod);
            param.Add("charset", "utf-8");
            param.Add("sign_type", "RSA2");
            param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("version", "1.0");
            param = Sign(param);
            var orignalQueryString = string.Join("&", param.Where(p => p.Key != "sign").Select(p => $"{p.Key}={p.Value}").ToArray());
            OAuthLog.Log("GET queryString=[{0}], sign=[{1}]", orignalQueryString, param["sign"]);
            using (var httpClient = API.CreateHttpClient())
            {
                OAuthLog.Log("GET [{0}]", API_URL);
                httpClient.DefaultRequestHeaders.Add("Referer", "https://alipay.com");
                httpClient.DefaultRequestHeaders.Add("User-Agent", API.DEFAULT_USER_AGENT);
                httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                HttpResponseMessage response = null;
                if (httpMethod == "get")
                {
                    var queryString = string.Join("&", param.Select(p => $"{p.Key}={p.Value}").ToArray());
                    var api = $"{API_URL}?{queryString}";
                    response = httpClient.GetAsync(api).Result;
                }
                else if (httpMethod == "post")
                {
                    response = httpClient.PostAsync(API_URL, new FormUrlEncodedContent(param)).Result;
                }
                response.EnsureSuccessStatusCode();
                var responseText = response.Content.ReadAsStringAsync().Result;
                OAuthLog.Log("GET status code={0}, response text=[{1}]", response.StatusCode, responseText);
                return JsonSerializer.Deserialize<T>(responseText);
            }
        }

        public T Get<T>(string apiMethod, Dictionary<string, string> param = null)
        {
            return Invoke<T>("get", apiMethod, param);
        }

        public T Post<T>(string apiMethod, Dictionary<string, string> param = null)
        {
            return Invoke<T>("post", apiMethod, param);
        }

        /// <summary>
        /// https://opendocs.alipay.com/open/291/106118
        /// </summary>
        /// <param name="dict"></param>
        private Dictionary<string, string> Sign(Dictionary<string, string> dict)
        {
            //1、获取所有请求参数，不包括字节类型参数，如文件、字节流，
            //   剔除 sign 字段，剔除值为空的参数，并按照第一个字符的键值 ASCII 码递增排序（字母升序排序），
            //   如果遇到相同字符则按照第二个字符的键值 ASCII 码递增排序，以此类推。

            //2、将排序后的参数与其对应值，组合成“参数=参数值”的格式，并且把这些参数用 & 字符连接起来，
            //   此时生成的字符串为待签名字符串。
            var sortedDict = new SortedDictionary<string, string>();
            foreach (var item in dict)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    sortedDict.Add(item.Key, item.Value);
                }
            }
            var needSignStr = string.Join("&", sortedDict.Select(p => $"{p.Key}={p.Value}").ToArray());
            var sign = RSASign(needSignStr, PrivateRSAKey);
            sortedDict.Add("sign", sign);
            return new Dictionary<string, string>(sortedDict);
        }
        private static string RSASign(string data, string privatekey)
        {
            RSACryptoServiceProvider rsaCsp = LoadCertificateString(privatekey);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var signatureBytes = rsaCsp.SignData(dataBytes, "SHA256");
            return Convert.ToBase64String(signatureBytes);
        }
        private static RSACryptoServiceProvider LoadCertificateString(string strKey)
        {
            try
            {
                var data = Convert.FromBase64String(strKey);
                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, "RSA2");
                return rsa;
            }
            catch
            {
            }
            return null;
        }
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch
            {
                return null;
            }
            finally
            {
                binr.Close();
            }
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
    }
}
