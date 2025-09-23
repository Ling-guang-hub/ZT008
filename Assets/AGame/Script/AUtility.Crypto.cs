using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static partial class AUtility
{
    public static class Crypto
    {
        #region 纯A包AppLovin SdkKey加密工具

        /// <summary>
        /// DES加密解密密钥向量
        /// </summary>
        private static readonly byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="sdkKey"></param>
        /// <returns></returns>
        public static string DecryptDES(string sdkKey, string packageName)
        {
            // byte[] rgbKey = Encoding.UTF8.GetBytes(Application.identifier.Substring(0, 8));
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(packageName.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(sdkKey);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                cStream.Close();
                Debug.Log($"解密成功: {Encoding.UTF8.GetString(mStream.ToArray())}\nsource: {sdkKey}");
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                Debug.LogError($"解密失败！请检查SdkKey和packageName是否正确！\n{sdkKey}\n{packageName}");
                return sdkKey;
            }
        }
            
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptString, string packageName)
        {
            try
            {
                // byte[] rgbKey = Encoding.UTF8.GetBytes(Application.identifier.Substring(0, 8));
                byte[] rgbKey = Encoding.UTF8.GetBytes(packageName.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                cStream.Close();
                Debug.Log($"加密成功: {Convert.ToBase64String(mStream.ToArray())}\nsource: {encryptString}");
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                Debug.LogError($"加密失败！请检查SdkKey和packageName是否正确！\n{encryptString}\n{packageName}");
                return encryptString;
            }
        }
        #endregion
    }
}
