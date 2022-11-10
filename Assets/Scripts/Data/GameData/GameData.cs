using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int level;
    public int currentIndex;
    public int [] officeMoney;
    public GameData()
    {
        level = 0;
        officeMoney = new int[10];
        currentIndex = 0;
    }
}
