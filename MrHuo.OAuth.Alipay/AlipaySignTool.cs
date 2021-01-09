using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.IO;

namespace MrHuo.OAuth.Alipay
{
    //1、获取所有请求参数，不包括字节类型参数，如文件、字节流，
    //   剔除 sign 字段，剔除值为空的参数，并按照第一个字符的键值 ASCII 码递增排序（字母升序排序），
    //   如果遇到相同字符则按照第二个字符的键值 ASCII 码递增排序，以此类推。
    //2、将排序后的参数与其对应值，组合成“参数=参数值”的格式，并且把这些参数用 & 字符连接起来，
    //   此时生成的字符串为待签名字符串。
    static class AlipaySignTool
    {
        /// <summary>
        /// https://opendocs.alipay.com/open/291/106118
        /// </summary>
        /// <param name="dict"></param>
        public static Dictionary<string, string> Sign(Dictionary<string, string> dict, string privateRSAKey)
        {
            var sortedDict = new SortedDictionary<string, string>();
            foreach (var item in dict)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    sortedDict.Add(item.Key, item.Value);
                }
            }
            var needSignStr = string.Join("&", sortedDict.Select(p => $"{p.Key}={p.Value}").ToArray());
            Console.WriteLine($"PrivateRSAKey: [{privateRSAKey}]");
            Console.WriteLine($"待签名: [{needSignStr}]");
            var sign = RSASign(needSignStr, privateRSAKey);
            Console.WriteLine($"最终签名: [{sign}]");
            sortedDict.Add("sign", sign);
            return new Dictionary<string, string>(sortedDict);
        }
        private static string RSASign(string data, string privatekey)
        {
            var rsaCsp = DecodeRSAPrivateKey(Convert.FromBase64String(privatekey), "RSA2");
            var signatureBytes = rsaCsp.SignData(Encoding.UTF8.GetBytes(data), "SHA256");
            return Convert.ToBase64String(signatureBytes);
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

                // linux 下签名时报错问题： https://www.cnblogs.com/zinan/p/11174731.html
                //CspParameters CspParameters = new CspParameters();
                //CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;
                //
                //int bitLen = 1024;
                //if ("RSA2".Equals(signType))
                //{
                //    bitLen = 2048;
                //}

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
