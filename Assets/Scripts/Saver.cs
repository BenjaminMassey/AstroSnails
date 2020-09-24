using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Saver : MonoBehaviour
{
    // Attached to name text input field
    
    private InputField name_input;

    void Start()
    {
        SaveData.file_path = Application.persistentDataPath + "/battlebaos.save";
        name_input = GetComponent<InputField>();

        string result = SaveData.Load();
        Debug.Log("Saver got result: " + result);
        if (!result.Equals("<FAILURE>"))
        {
            name_input.text = result;
        }
    }

    public void Save()
    {
        SaveData.data = name_input.text;
        SaveData.Save();
    }
}

public static class SaveData
{
    public static bool initialized = false;
    public static string file_path;
    public static string data;

    public static void Save()
    {
        Debug.Log("Attempting save data (" + file_path + ")");
        // https://support.unity3d.com/hc/en-us/articles/115000341143/comments/360000857792
        try
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                sw.Write(data);
                sw.Close();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public static string Load()
    {
        Debug.Log("Attempting load data (" + file_path + ")");
        if (File.Exists(file_path))
        {
            Debug.Log("Succeeded file open: data collecting");
            try
            {
                string result = "";
                using (StreamReader sr = new StreamReader(new FileStream(file_path, FileMode.Open, FileAccess.Read)))
                {
                    int i = 0;
                    while (sr.Peek() >= 0)
                    {
                        try
                        {
                            char c = (char)sr.Read();
                            result += c.ToString();
                        }
                        catch (Exception e)
                        {
                            Debug.Log(e.ToString());
                            return "<FAILURE>";
                        }
                        i++;
                    }
                    sr.Close();
                }
                Debug.Log("SaveData got result: " + result);
                return result;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return "<FAILURE>";
            }
        }
        else
        {
            Debug.Log("Failed file open");
            return "<FAILURE>";
        }
    }
}
