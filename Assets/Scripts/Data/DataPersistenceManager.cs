using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Stroage Config")]
    [SerializeField] private string fileName;
    private FileDataHandler dataHandler;
    GameData gameData;
    public static DataPersistenceManager instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObjects;
    private void Awake()
    {
        instance = this;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllIDataPersistenceObjects();
    }
    
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (IDataPersistence dataPersistanceObject in dataPersistenceObjects)
        {
            dataPersistanceObject.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistanceObject in dataPersistenceObjects)
        {
            dataPersistanceObject.SaveData(gameData);
        }
        dataHandler.Save(gameData);
    }
    
    List<IDataPersistence> FindAllIDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
