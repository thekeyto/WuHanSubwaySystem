using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
public class ExeCall : MonoBehaviour
{
    public string startline, startstation;
    public string endline, endstation;
    public string standNumber;
    public string hour, min;/*
    private void Start()
    {
        string s = Application.dataPath + "/Resources/";
        string exeFilePath = s + "system_of_subway.exe";
        UnityEngine.Debug.Log(exeFilePath);
        //设置参数-多参数使用空格键进行分隔
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = exeFilePath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Arguments = "1 1 10 2 6 7 50";
            process.EnableRaisingEvents = true;
            process.Start();
            process.WaitForExit();
            UnityEngine.Debug.Log("exe已经运行关闭了");
            int ExitCode = process.ExitCode;
            //print(ExitCode);
        }
        catch (Exception e)
        {
            print(e);
        }
    }*/
    public void StartExe()
    {
        string s = Application.dataPath + "/Resources/";
        string exeFilePath = s + "system_of_subway.exe";
        UnityEngine.Debug.Log(exeFilePath);
        //设置参数-多参数使用空格键进行分隔
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = exeFilePath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Arguments = standNumber+ " " + startline + " " + startstation + " " + endline + " " + endstation+" "+hour+" "+min;
            process.EnableRaisingEvents = true;
            process.Start();
            process.WaitForExit();
            UnityEngine.Debug.Log("exe已经运行关闭了");
            int ExitCode = process.ExitCode;
            //print(ExitCode);
        }
        catch (Exception e)
        {
            print(e);
        }
    }

}
