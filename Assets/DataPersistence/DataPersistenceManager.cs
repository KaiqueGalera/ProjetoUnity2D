using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataPersistenceManager : MonoBehaviour
{   

    [Header("Reiniciando jogo")]
    public bool                             isRestarting;

    [Header("File Storage Config")]
    [SerializeField] private string         fileName;
    
    [Header("Debugging")]
    //[SerializeField] private bool           initializatingDataIfNull = false;
    private GameData                        _GameData;
    private FileDataHandler                 _FileDataHandler;

    public static DataPersistenceManager    Instance { get; private set; }

    private List<IDataPersistence>          dataPersistencesObjects;

    private void Awake()
    {
        if (Instance != null)
        {
            // Debug.LogError("Found more than one Data Persistence Manager in the scene.Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this._FileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        Debug.Log(Application.persistentDataPath);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded   += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded   -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isRestarting == false)
        {
            this.dataPersistencesObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

    }

    public void OnSceneUnloaded(Scene scene)
    {
        if (isRestarting == false)
        {
            SaveGame();
        }
    }

    public void NewGame()
    {
        _GameData = new GameData();
        Debug.Log("iNICIANDO NOVO JOGO");

    }

    public void LoadGame()
    {
        // - Carrega os dados salvos através do data handler utilizando um arquivo
        this._GameData = _FileDataHandler.Load();

        // - Caso o não tenha dados para carregar e há necessidade de inicializar os dados para propósito de depuração
        // if (this._GameData == null && initializatingDataIfNull)
        // {
        //     NewGame();
        // }

        if (this._GameData == null)
        {
            Debug.Log("No data was found. New game needs to be started before data can be loaded.");
            return;
        }

        // - Carrega os dados nos respectivos scripts
        foreach (IDataPersistence dataPersistence in dataPersistencesObjects)
        {
            dataPersistence.LoadData(_GameData);
        }
    }

    public void DeleteGame()
    {
        // if we don't have any data to save, log a warning here
        if (this._GameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
            return;
        }

        _FileDataHandler.DeleteFile();

    }
    public void SaveGame()
    {
         // if we don't have any data to save, log a warning here
        if (this._GameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
            return;
        }

        // - Passa os dados para outros scripts para que possam atualiza-los
        foreach (IDataPersistence dataPersistence in dataPersistencesObjects)
        {
            dataPersistence.SaveData(_GameData);
        }
        
        // - Salva todos os dados em um arquivo usando o data handler
        _FileDataHandler.Save(_GameData);

    }

    public void AutoDestroy()
    {
        Destroy(gameObject);
    }
    public void OnApplicationQuit()
    {
        SaveGame();
        // - Passa os dados para outros scripts para que possam atualiza-los
        // - Salva todos os dados em um arquivo usando o data handler
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistencesObjects); // Retorna uma lista de do tipo IDataPersistenceObjects
    }

    public bool HasGameData()
    {
        return _GameData != null;
    }
}
