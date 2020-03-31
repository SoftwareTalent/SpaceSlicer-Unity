using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// Use this for initialization

    public enum GameState
    {
        Wait,
        Playing,
        GameOver,
    };

    public GameState state;

    public GameObject HomeScreen;
    public GameObject PlayingMenu;
    public GameObject PopupNewBest;
    public GameObject PopupCong;
    public GameObject PopupNotsogood;
    public GameObject HowTo;

    public static GameManager Instance;

    public int _BestScore;
    public int Score;
    public int Lives;

    public Text txtScore;
    public GameObject[] LivesIcon;

    public Image PowerUPBar;
    public Text PowerUPTxt;
    public int PowerUP = 0;
    int powerupTime = 100;
    float _countTmPup;
    public int BestScore
    {
        get
        {
            _BestScore = PlayerPrefs.GetInt("BestScore", 0);

            return _BestScore;
        }

        set
        {
            _BestScore = PlayerPrefs.GetInt("BestScore", 0);
            if (value > _BestScore)
            {
                PlayerPrefs.SetInt("BestScore", value);
            }
        }
    }

    void Awake()
    {
        Instance = this;
    }

	void Start () {
        state = GameState.Wait;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (state == GameState.Playing)
        {
            txtScore.text ="SCORE: "+Score.ToString();

            if (Lives >= 0 && Lives < 3)
            {
                LivesIcon[Lives].SetActive(false);
            }
        }
        
        
	}

    void DisableMenus()
    {
        HowTo.SetActive(false);
        HomeScreen.SetActive(false);
        PlayingMenu.SetActive(false);
        PopupCong.SetActive(false);
        PopupNewBest.SetActive(false);
        PopupNotsogood.SetActive(false);
    }

    public void OpenHome()
    {
        DisableMenus();
        HomeScreen.SetActive(true);
    }

    public void OpenHowTo()
    {
        DisableMenus();
        HowTo.SetActive(true);
    }

    public void GameOver()
    {
        state = GameState.GameOver;
    }

    public void GameStart()
    {
        LivesIcon[0].SetActive(true);
        LivesIcon[1].SetActive(true);
        LivesIcon[2].SetActive(true);
        FinishedPowerUP();
        DisableMenus();
        PlayingMenu.SetActive(true);
        state = GameState.Playing;
        GameManager.Instance.Score = 0;
        Lives = 3;
        Spawn_items.Instance.StartSpawn();
    }

    public void GamePause()
    {
        if (state == GameState.Playing)
        {
            Time.timeScale = 0;
            Spawn_items.Instance.StopSpawn();
            state = GameState.Wait;
        }
        else
        {

            state = GameState.Playing;
            Spawn_items.Instance.StartSpawn();
            Time.timeScale = 1;
        }
        
    }


    public void OpenPopup(int d)
    {
        state = GameState.GameOver;
        DisableMenus();
        if (d == 0)
        {
            PopupCong.SetActive(true);
        }
        else if (d == 1)
        {
            PopupNewBest.SetActive(true);
        }
        else if (d == 2)
        {
            PopupNotsogood.SetActive(true);
        }
    }

    public void StartedPowerup(int pid)
    {
        PowerUP = pid; 
        PowerUPBar.gameObject.SetActive(true);
        PowerUPTxt.gameObject.SetActive(true);

        if (pid == 1) // double points
        {
            PowerUPTxt.text = "Double Points";
        }
        else if (pid == 2)
        {
            PowerUPTxt.text = "Immunity";
        }

        PowerUPBar.fillAmount = 1;
        _countTmPup = powerupTime;

        StartCoroutine(CountTimeForPowerUP(0.1f));
    }

    IEnumerator CountTimeForPowerUP(float dt)
    {
        yield return new WaitForSeconds(dt);
        _countTmPup--;
        PowerUPBar.fillAmount = _countTmPup / powerupTime;

        if (_countTmPup < 0 )
        {
            FinishedPowerUP();
        }
        else
        {
            StartCoroutine(CountTimeForPowerUP(0.1f));
        }
    }

    public void FinishedPowerUP()
    {
        PowerUPBar.gameObject.SetActive(false);
        PowerUPTxt.gameObject.SetActive(false);
        PowerUP = powerupTime;

        StopAllCoroutines();
    }

}
