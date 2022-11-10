using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour ,IDataPersistence
{
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

        DontDestroyOnLoad(this.gameObject);
    }
    public void LoadData(GameData data)
    {
        level = data.level;
    }
    public void SaveData(ref GameData data)
    {
        data.level = level;
        StartCoroutine(FadeScene(level, 0f, 0f));
    }
    public void NextLevelBtn()
    {
        btn.gameObject.SetActive(false);
        level++;
        StartCoroutine(FadeScene(level, 1f, .5f));
    }

    public IEnumerator FadeScene(int level, float duration, float waitTime)
    {
        //fader.transform.position = Vector3.zero;
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
        fader.gameObject.SetActive(false);
        while (passed < duration)
        {
            passed += Time.deltaTime;
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, passed));
            yield return null;
        }
        

        
    }
}
