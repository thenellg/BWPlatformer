using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuControl : MonoBehaviour
{
    [SerializeField] int level1Complete = 0;
    [SerializeField] int level2Complete = 0;
    [SerializeField] int level3Complete = 0;
    [SerializeField] int level4Complete = 0;
    [SerializeField] int level5Complete = 0;
    [SerializeField] int level6Complete = 0;

    public Sprite filledInStar;
    public Image level1Star;
    public Image level2Star;
    public Image level3Star;
    public Image level4Star;
    public Image level5Star;
    public Image level6Star;

    void Start()
    {


        level1Complete = PlayerPrefs.GetInt("Level1Status", 0);
        level2Complete = PlayerPrefs.GetInt("Level2Status", 0);
        level3Complete = PlayerPrefs.GetInt("Level3Status", 0);
        level4Complete = PlayerPrefs.GetInt("Level4Status", 0);
        level5Complete = PlayerPrefs.GetInt("Level5Status", 0);
        level6Complete = PlayerPrefs.GetInt("Level6Status", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (level1Complete > 0)
            level1Star.sprite = filledInStar;

        if (level2Complete > 0)
            level2Star.sprite = filledInStar;

        if (level3Complete > 0)
            level3Star.sprite = filledInStar;

        if (level4Complete > 0)
            level4Star.sprite = filledInStar;

        if (level5Complete > 0)
            level5Star.sprite = filledInStar;

        if (level6Complete > 0)
            level6Star.sprite = filledInStar;
    }

    public void changeScenes(string sceneName)
    {
        Debug.Log(sceneName);
        PlayerPrefs.SetInt("deathCount", 0);

        SceneManager.LoadScene(sceneName);
    }

    public void close()
    {
        Application.Quit();
    }
}
