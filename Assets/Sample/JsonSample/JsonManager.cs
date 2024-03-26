using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    private SaveArray Target1;
    private SaveArray Target2;
    private int b = 0;
    private string content1;
    private string content2;
    private string file = "JsonTest.txt";
    private void Start()
    {
        Target1 = new SaveArray();
        Target2 = new SaveArray();
    }
    public void InCrease()
    {
        b++;
        Target1.a.Add(b);
        Target1.name = "Nam";
        Target2.a.Add(b);
        Target2.name = "Huyen";
        Debug.Log("increase" + b);
    }
    public void save()
    {
        content1 = JsonUtility.ToJson(Target1);
        content2 = JsonUtility.ToJson(Target2);
        WriteToFile(file, content1);
        WriteToFile(file,content2);
    }
    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
    }
    private string ReadFromFIle(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
        }

        return "Success";
    }
    public void DeleteSaveFile()
    {
        string filePath = GetFilePath(file);

        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath,string.Empty);
            Target1 = new SaveArray();
            save();
            Debug.Log("File deleted: " + filePath);
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
    private string GetFilePath(string fileName)
    {
        Debug.Log(Application.persistentDataPath + "/" + fileName);
        return Application.persistentDataPath + "/" + fileName;
    }

    public void load()
    {
        string load = ReadFromFIle(file);
        JsonUtility.FromJsonOverwrite(load, Target1);
        JsonUtility.FromJsonOverwrite(load,Target2);
        foreach (var mb in Target1.a)
        {
            Debug.Log(Target1.name);
            Debug.Log(mb);
        }
        foreach (var mb in Target2.a)
        {
            Debug.Log(Target2.name);
            Debug.Log(mb);
        }
    }
}
[Serializable]
public class SaveArray
{
    public List<int> a = new List<int>();
    public string name;
    public Vector3 pos;
}
