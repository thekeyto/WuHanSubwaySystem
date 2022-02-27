using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// AES加密算法
/// </summary>
public class AESEncryption : MonoBehaviour
{

    public static string myEncrypt(string data)
    {
        byte[] bs = Encoding.UTF8.GetBytes(data);

        RijndaelManaged aes256 = new RijndaelManaged();
        aes256.Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        aes256.Mode = CipherMode.ECB;
        aes256.Padding = PaddingMode.PKCS7;

        return Convert.ToBase64String(aes256.CreateEncryptor().TransformFinalBlock(bs, 0, bs.Length));
    }

    public static string myDecrypt(string data)
    {
        byte[] bs = Convert.FromBase64String(data);

        RijndaelManaged aes256 = new RijndaelManaged();
        aes256.Key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        aes256.Mode = CipherMode.ECB;
        aes256.Padding = PaddingMode.PKCS7;

        return Encoding.UTF8.GetString(aes256.CreateDecryptor().TransformFinalBlock(bs, 0, bs.Length));
    }
}