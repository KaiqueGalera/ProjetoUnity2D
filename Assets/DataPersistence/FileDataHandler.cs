using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq.Expressions;
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;  
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData LoadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {   
                // Carrega a informação serializada do arquivo 
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // Deserializa a informação do Json de volta para objeto C#endregion
                LoadedData = JsonUtility.FromJson<GameData>(dataToLoad); 
            }
            catch(Exception ex)
            {
                Debug.LogError("Error occured when trying to loaded data from file:" + fullPath + "\n" + ex);
            }
        }
        return LoadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        } 
        catch(Exception ex)
        {
            Debug.LogError("Error occured when trying to save data in file:" + fullPath + "\n" + ex);
        }
    }

    // Função para deletar o arquivo
    public void DeleteFile()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
                Debug.Log("File deleted successfully: " + fullPath);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error occurred when trying to delete file:" + fullPath + "\n" + ex);
            }
        }
        else
        {
            Debug.LogWarning("File does not exist: " + fullPath);
        }
    }
}


