using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Office : MonoBehaviour, IDataPersistence
{
    public Transform cameraPlace;
    [SerializeField] int index;
    public Image moneyImage;
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject [] childs;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int _requiredMoneyForLevel;
    public int requiredMoneyForLevel { get => _requiredMoneyForLevel; private set { }}
    private int requiredMoneyPerItem;
    [SerializeField] private int _levelMoney;

    public int levelMoney
    {
        get { return _levelMoney; }
        set {
            _levelMoney = value;
            slider.value = _levelMoney;
            moneyText.text = _levelMoney.ToString();
        }
    }

    [SerializeField] private int _currentMoney;
    public int currentMoney
    {
        get { return _currentMoney; }
        set {
            _currentMoney = value;
            
            levelMoney += 2;
            if(_currentMoney >= requiredMoneyPerItem)
            {
                if (!childs[itemIndex].TryGetComponent(out RectTransform rectTransform))
                {
                    childs[itemIndex].SetActive(true);
                    itemIndex++;
                }
                _currentMoney = 0;
            }
        }
    }
    
    private void Awake()
    {
        Transform[] canvasChilds = GetComponentsInChildren<Transform>();
        Transform[] transforms = GetComponentsInChildren<Transform>();
        childs = transforms.Select(x => x.gameObject).ToArray();
                    
        
        slider.maxValue = requiredMoneyForLevel;
        slider.value = levelMoney;
            
        requiredMoneyPerItem = requiredMoneyForLevel / childs.Length;
    }
    private void Start()
    {
        foreach (var item in childs)
        {
            if (item.TryGetComponent(out RectTransform rectTransform))
                continue;
            item.SetActive(false);
        }
        var itemCount = levelMoney / requiredMoneyPerItem;
        for (int i = 0; i < itemCount; i++)
        {
            if (!childs[itemIndex].TryGetComponent(out RectTransform rectTransform))
            {
                childs[itemIndex].SetActive(true);
                itemIndex++;
            }
        }
    }
    public void LoadData(GameData data)
    {
        Debug.Log(data.officeMoney[index] + " load " + index);
        levelMoney = data.officeMoney[index];

    }
    public void SaveData(ref GameData data)
    {
        Debug.Log(levelMoney + " save " + index);
        data.officeMoney[index] = levelMoney;
    }
}
