using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndRoad : MonoBehaviour
{
    
    [SerializeField] List<GameObject> moneys;
    [SerializeField] int moneyAnimationIndex;
    int earnedMoneyPerFrame = 2;
    [SerializeField] private int collectedMoney;
    public List<Transform> officePath;
    [SerializeField] private GameObject officeRoads;
    private void Start()
    {
        collectedMoney = Collect.instance.GetCollecteMoney();
        for (int i = 0; i < officeRoads.transform.childCount; i++)
        {
            officePath.Add(officeRoads.transform.GetChild(i).transform);
        }
        StartCoroutine(Motorcycle.instance.Drive(OfficeManager.instance.currIndex, officePath));
        CameraFollow.instance.Finish(OfficeManager.instance.currentOffice.cameraPlace, OfficeManager.instance.currentOffice.transform);
        moneys = Collect.instance.CollectedMoneys();
    }
    public IEnumerator EarnMoney()
    {
        moneyAnimationIndex = moneys.Count;
        var temp = 0;
        Office currOffice = OfficeManager.instance.currentOffice;
        while (true)
        {
            yield return null;
            collectedMoney -= earnedMoneyPerFrame;
            temp+= earnedMoneyPerFrame;
            currOffice.currentMoney += earnedMoneyPerFrame;
            if(collectedMoney == 0)
            {
              
                Debug.Log("para býtti");
                break;
            }
            if(currOffice.levelMoney == currOffice.requiredMoneyForLevel)
            {   
                NextOffice();
                break;
            }
            if(temp == 10)
            {
                if (moneyAnimationIndex == 0)
                {
                    LevelManager.instance.finishCanvas.GetComponent<Button>().interactable = true;
                    LevelManager.instance.finishCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
                    LevelManager.instance.finishCanvas.GetComponent<Image>().enabled = true;
                    LevelManager.instance.NextLevelBtn();
                    DataPersistenceManager.instance.SaveGame();
                    break;
                }
                moneys[moneyAnimationIndex-1].GetComponent<MoneyAnimation>().Trigger();
                moneys.Remove(moneys[moneyAnimationIndex-1]);
                moneyAnimationIndex--;
                temp = 0;
            }
        }
    }   
    public void NextOffice()
    {
        OfficeManager.instance.currIndex++;
        CameraFollow.instance.Finish(OfficeManager.instance.currentOffice.cameraPlace, OfficeManager.instance.currentOffice.transform);
        StartCoroutine(Motorcycle.instance.Drive(OfficeManager.instance.currIndex, officePath));
    }
}
