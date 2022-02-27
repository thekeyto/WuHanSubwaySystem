using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileTest : MonoBehaviour
{
    /// <summary>
    /// 密钥(系统自动随机的密钥)
    /// </summary>
    string keys = "";
    /// <summary>
    /// 注册码(玩家输入的注册码)
    /// </summary>
    string inputLicense = "";

    string numChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    void Awake()
    {
        
    }

    // Use this for initialization
    public void encry()
    {
        List<string> arraydata = new List<string>();
        string line = "";
        using (StreamReader sr = new StreamReader("D:/myproject/subway_system/Assets/Resources/testAes.csv"))
        {
            while ((line = sr.ReadLine()) != null)
            {
                arraydata.Add( AESEncryption.myEncrypt(line));
            }
        }

        using (StreamWriter sw = new StreamWriter("D:/myproject/subway_system/Assets/Resources/testAes.csv"))
        {
            for(int i=0;i<arraydata.Count;i++)
            {
                sw.WriteLine(arraydata[i]);

            }
        }

    }

    public void decry()
    {
        List<string> arraydata = new List<string>();
        string line = "";
        using (StreamReader sr = new StreamReader("D:/myproject/subway_system/Assets/Resources/testAes.csv"))
        {
            while ((line = sr.ReadLine()) != null)
            {
                arraydata.Add(AESEncryption.myDecrypt(line));
            }
        }

        using (StreamWriter sw = new StreamWriter("D:/myproject/subway_system/Assets/Resources/testAes.csv"))
        {
            for (int i = 0; i < arraydata.Count; i++)
            {
                sw.WriteLine(arraydata[i]);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
