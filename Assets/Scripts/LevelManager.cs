using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    public GameObject finishCanvas;
    
    [SerializeField] Button btn;
    public static LevelManager instance;
    [SerializeField] private Image fader;
    [SerializeField] private int level;
    public LevelManager()
    {
        level = 0;
    }
    void Awake()
    {
        LevelManager[] objs = GameObject.FindObjectsOfType<LevelManager>();
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        if (instance == null)
            instance = this;
        DontDestroyOnLoad(this.gameObject);
        finishCanvas = transform.GetChild(1).gameObject;
    }
    public void LoadData(GameData data)
    {
        level = data.level;
    }
    public void SaveData(GameData data)
    {
        data.level = level;
        //StartCoroutine(FadeScene(level, 0f, 0f));
    }
    public void NextLevelBtn()
    {
        btn.gameObject.GetComponent<Button>().interactable= false;
        level++;
        Debug.Log(level+" level");
        if (level >= SceneManager.sceneCountInBuildSettings-1)
        {
            level = 0;
        }
        StartCoroutine(FadeScene(level, 1f, .5f));
    }

    public IEnumerator FadeScene(int level, float duration, float waitTime)
    {
        yield return new WaitForSeconds(1);
        //fader.transform.position = Vector3.zero;
        btn.gameObject.GetComponent<Image>().enabled = false;
        btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
        float passed = 0f;
        fader = transform.GetChild(0).GetComponent<Image>();
        fader.gameObject.SetActive(true);
        while (passed < duration)
        {
            passed += Time.deltaTime;
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, passed));
            yield return null;
        }
        AsyncOperation ao = SceneManager.LoadSceneAsync(level);
        while (!ao.isDone)
        {
            yield return null;
        }
        SceneManager.LoadScene(level);
        yield return new WaitForSeconds(waitTime);
        passed = 0;
        fader = transform.GetChild(0).GetComponent<Image>();
        fader.color = new Color(0, 0, 0, 1);
        fader.gameObject.SetActive(false);
        while (passed < duration)
        {
            passed += Time.deltaTime;
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, passed));
            yield return null;
        }

    }
}
