using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class WriteOnFile : MonoBehaviour
{
    private int id = 0;
    private bool firstTime = false;
    private StreamWriter sw;
    private String path;

    private GameObject logs;

    
    void Start()
    {
        path = Application.persistentDataPath + "MOJO_" + id + ".txt";

        while (checkIfFileExists() == true)
        {
            id += 1;
            path = Application.persistentDataPath + "MOJO_" + id + ".txt";
        }

        //path = Application.dataPath + "/DataFiles/test_" + id + ".txt";
        //path = Application.persistentDataPath + "test_" + id + ".txt";
        Debug.Log(path);
        logs = GameObject.Find("/Aux");
    }

 

    public bool checkIfFileExists()
    {
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CreateFile()
    {        
        sw = new StreamWriter(path);
        sw.WriteLine("Data from: " + id);
        sw.Flush();
        firstTime = true;
    }

    public void WriteToFile(String str)
    {

        if (firstTime == false)
        {
            CreateFile();
        }

        String area = logs.GetComponent<TriggerLog>().Area;

        sw.WriteLine(area + "   " + str + " " + Time.realtimeSinceStartup);
        sw.Flush();

        //closeFile();
    }

    public void closeFile()
    {
        sw.Close();
    }
}