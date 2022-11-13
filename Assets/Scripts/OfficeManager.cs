using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeManager : MonoBehaviour , IDataPersistence
{
    public static OfficeManager instance;
    [SerializeField] Office[] offices;
    public Office currentOffice;
    [SerializeField] private int _currIndex;
    public int currIndex {
        get => _currIndex;
        set
        {
            _currIndex = value;
            currentOffice = offices[_currIndex];
        }
    }
    
    private void Awake()
    {
        instance = this;
        offices = GetComponentsInChildren<Office>();
    }
    private void OnEnable()
    {
        
    }
    private void Start()
    {
        DataPersistenceManager.instance.LoadGame();
    }
    public void LoadData(GameData data)
    {
        currIndex = data.currentIndex;
    }
    public void SaveData(GameData data)
    {
        data.currentIndex = currIndex;
    }
    
}
