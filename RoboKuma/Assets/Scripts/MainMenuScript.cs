﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Assets.SimpleAndroidNotifications;

public class MainMenuScript : MonoBehaviour {
   
    public Canvas MainScreen;
    public GameObject PetScreen;
	public GameObject MinigameScreen;
    public GameObject CustomizationScreen;
    public GameObject AchievementScreen;
    public GameObject AttributeScreen;
    public GameObject SettingsScreen;
    public GameObject ResultsPanel;
    public GameObject RoboKuma, RKAnimation;
    public GameObject Q1, Q2, Q3;
    public GameObject SpeechBubble;
    public GameObject popup;
    public GameObject dailyPanel;
    public GameObject rewards;

    public Slider Memory, Response, Speed, Accuracy, MemoryR, ResponseR, SpeedR, AccuracyR, MemoryS, ResponseS, SpeedS, AccuracyS;
    public Text  MemoryRT, ResponseRT, SpeedRT, AccuracyRT, MemoryT, ResponseT, SpeedT, AccuracyT;
    public Text[] testsAverage;
    public Text[] testsBest;
    public GameObject generalStatPanel, minigameStatPanel;
    public Image generalStatBtn, minigameStatBtn, gBtnBottom, mBtnBottom;
    public Text generalData;

    public Text popupText;
    public Text petStatus;
    public Text petMessage;
    public Text experience, bearya, resultLevel, resultText;
    public Text tLevel, tBearya, tXP;
    public Text incompleteDaily;
    public Image resultI;
    public Slider tExperience;

    public Slider achievementsGoNoGo, achievementsEriksen, achievementsCorsi, achievementsNback;
    public Text achievementsGoNoGoText, achievementsEriksenText, achievementsCorsiText, achievementsNbackText;
    public GameObject[] achievementRewards;
    public GameObject[] achievementComplete;

	public Slider dailyObjectivesGoNoGo, dailyObjectivesEriksen, dailyObjectivesCorsi, dailyObjectivesNback;
	public Text dailyObjectivesGoNoGoText, dailyObjectivesEriksenText, dailyObjectivesCorsiText, dailyObjectivesNbackText;
    public GameObject[] dailyRewards;
    public GameObject[] dailyComplete;

    public Image leg, body, head, accessory;

    public Button back;

    public int log_id { get; set; }

    public String status;

    public Boolean leveledup = false;

    public SQLiteDatabase sn;

    public Sprite body_1, body_2, body_3, body_4, body_5;
    public Sprite leg_1, leg_2, leg_3;
    public Sprite head_1, head_2;
    public Sprite accessory_1;
    public Sprite trophy;

    public String dailyReward = "";
    public String achievementReward = "";
    //for sounds SFX
    public AudioSource audioHandler;
    public AudioSource audioBackground;
    public AudioClip soundStable;
    public AudioClip soundSlow;
    public AudioClip soundFidgety;
    public AudioClip soundForgetful;
    public AudioClip soundLevelup;
    public AudioClip soundAchievement;
    public AudioClip backGround;

    // Use this for initialization
    void Start () {

        NotificationManager.CancelAll();
		MainScreen = MainScreen.GetComponent<Canvas>();
        MinigameScreen = GameObject.Find ("MinigameScreen");
		PetScreen = GameObject.Find ("PetScreen");
        CustomizationScreen = GameObject.Find("CustomizationScreen");
        AchievementScreen = GameObject.Find("AchievementScreen");
        AttributeScreen = GameObject.Find("AttributeScreen");
        MinigameScreen.gameObject.SetActive (false);
        CustomizationScreen.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        Q1.gameObject.SetActive(false);
        Q2.gameObject.SetActive(false);
        Q3.gameObject.SetActive(false);

        back.gameObject.SetActive(false);

        SpeechBubble.SetActive(false);
        popup.SetActive(false);
        dailyPanel.SetActive(false);

        sn = new SQLiteDatabase();
        status = "STABLE";

        //PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetInt("DB"));

        audioHandler.clip = backGround;
        audioHandler.Play();
        audioHandler.loop = true;


        if (PlayerPrefs.GetInt("DB", -1) == -1)
        {
            sn.Start();
            PlayerPrefs.SetInt("DB", 1);
            Debug.Log("DB");
        }

        updateAttributes();
        updateAchievements();
        updateAssets();
        updateDailyObjectives();

        if (PlayerPrefs.GetInt("Score", -1) != 1)
            ResultsPanel.gameObject.SetActive(false);
        else
        {
            if(achievementReward.Length != 0)
            {
                int exp = PlayerPrefs.GetInt("TExperience", 0) + PlayerPrefs.GetInt("Experience", 0);
                int bear = PlayerPrefs.GetInt("TBearya", 0) + PlayerPrefs.GetInt("Bearya", 0);
                exp += 500;
                bear += 500;
                PlayerPrefs.SetInt("TBearya", bear);
                PlayerPrefs.SetInt("TExperience", exp);
            }

            if(dailyReward.Length != 0)
            {
                int exp = PlayerPrefs.GetInt("TExperience", 0) + PlayerPrefs.GetInt("Experience", 0);
                int bear = PlayerPrefs.GetInt("TBearya", 0) + PlayerPrefs.GetInt("Bearya", 0);
                exp += 50;
                bear += 50;
                PlayerPrefs.SetInt("TBearya", bear);
                PlayerPrefs.SetInt("TExperience", exp);
            }
            
            resultI.overrideSprite = null;
            resultLevel.text = "";
            rewards.SetActive(false);
            
            //experience.text = PlayerPrefs.GetInt("Experience", 0) + "";
            //bearya.text = PlayerPrefs.GetInt("Bearya", 0) + "";

            int tExp = PlayerPrefs.GetInt("TExperience", 0) + PlayerPrefs.GetInt("Experience", 0);
            int tBear = PlayerPrefs.GetInt("TBearya", 0) + PlayerPrefs.GetInt("Bearya", 0);

            while (tExp >= Math.Pow((PlayerPrefs.GetInt("Level", 0) + 1), 3))
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
                leveledup = true;
            }

            
            if (achievementReward.Length != 0)
            {
                rewards.SetActive(true);
                audioHandler.PlayOneShot(soundAchievement);
                experience.text = "500";
                bearya.text = "500";
                resultText.text = "\nYou have unlocked\n\n\n\n\n" + achievementReward;
                resultI.overrideSprite = trophy;

                achievementReward = "";

            }
            else if (dailyReward.Length != 0)
            {
                rewards.SetActive(true);
                experience.text = "50";
                bearya.text = "50";
                audioHandler.PlayOneShot(soundAchievement);
                resultText.text = "\nYou have unlocked\n\n\n\n\n" + dailyReward;
                resultI.overrideSprite = trophy;

                dailyReward = "";
            }
            else if (leveledup)
            {
                audioHandler.PlayOneShot(soundLevelup);
                leveledup = false;
                resultText.text = "\nYou have reached level\n\n\n\n\nCongratulations!";
                resultLevel.text = PlayerPrefs.GetInt("Level") + "";
                resultI.overrideSprite = trophy;
            }
            else
                resultText.text = "\nThanks for the help!\n\n\n\n\nHere are the results";
            


            PlayerPrefs.SetInt("TExperience", tExp);
            PlayerPrefs.SetInt("TBearya", tBear);

            PlayerPrefs.SetInt("Experience", 0);
            PlayerPrefs.SetInt("Bearya", 0);

            ResultsPanel.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Score", 0);
        }

        tLevel.text = "Lvl. " + PlayerPrefs.GetInt("Level", 0);
        tBearya.text = PlayerPrefs.GetInt("TBearya", 0) + "";
        tXP.text = "" + (PlayerPrefs.GetInt("TExperience", 0) - Math.Pow((PlayerPrefs.GetInt("Level", 0)), 3)) + "/" + (Math.Pow((PlayerPrefs.GetInt("Level", 0) + 1), 3) - Math.Pow((PlayerPrefs.GetInt("Level", 0)), 3));


        Debug.Log(PlayerPrefs.GetInt("TExperience", 0));
        Debug.Log(Math.Pow((PlayerPrefs.GetInt("Level", 0)), 3));

        tExperience.value = (float) (PlayerPrefs.GetInt("TExperience", 0) - Math.Pow((PlayerPrefs.GetInt("Level", 0)), 3));
        tExperience.maxValue = (float) (Math.Pow((PlayerPrefs.GetInt("Level", 0) + 1), 3) - Math.Pow(PlayerPrefs.GetInt("Level", 0), 3));
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationPause(bool pauseStatus)
    {
        if(pauseStatus)
        {
            //NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(1), "Test Notification", "Should appear when the app closes.", new Color(1, 0.3f, 0.15f), NotificationIcon.Message);
            NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(64800), "RoboKuma", "Robokuma misses you.", new Color(1, 0.3f, 0.15f), NotificationIcon.Message);
        }
    }

    public void updateAchievements()
    {
        int gonogoCount = sn.count("gonogo");
        int eriksenCount = sn.count("eriksen");
        int nbackCount = sn.count("nback");
        int corsiCount = sn.count("corsi");
        
        if(gonogoCount == 20 && PlayerPrefs.GetInt("A1", 0) == 0)
        {
            achievementReward = "Go/No-Go Veteran";
            achievementRewards[0].SetActive(false);
            achievementComplete[0].SetActive(true);
            PlayerPrefs.SetInt("A1", 1); 
        }
        else if (PlayerPrefs.GetInt("A1", 0) == 0)
        {
            achievementComplete[0].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A1", 0) == 1)
        {
            achievementRewards[0].SetActive(false);
        }

        if (nbackCount == 20 && PlayerPrefs.GetInt("A2", 0) == 0)
        {
            achievementReward = "N-Back Veteran";
            achievementRewards[1].SetActive(false);
            achievementComplete[1].SetActive(true);
            PlayerPrefs.SetInt("A2", 1);
        }
        else if (PlayerPrefs.GetInt("A2", 0) == 0)
        {
            achievementComplete[1].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A2", 0) == 1)
        {
            achievementRewards[1].SetActive(false);
        }

        if (corsiCount == 20 && PlayerPrefs.GetInt("A3", 0) == 0)
        {
            achievementReward = "Corsi Block-Tapping Veteran";
            achievementRewards[2].SetActive(false);
            achievementComplete[2].SetActive(true);
            PlayerPrefs.SetInt("A3", 1);
        }
        else if (PlayerPrefs.GetInt("A3", 0) == 0)
        {
            achievementComplete[2].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A3", 0) == 1)
        {
            achievementRewards[2].SetActive(false);
        }

        if (eriksenCount == 20 && PlayerPrefs.GetInt("A4", 0) == 0)
        {
            achievementReward = "Eriksen Flanker Veteran";
            achievementRewards[3].SetActive(false);
            achievementComplete[3].SetActive(true);
            PlayerPrefs.SetInt("A4", 1);
        }
        else if (PlayerPrefs.GetInt("A4", 0) == 0)
        {
            achievementComplete[3].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A4", 0) == 1)
        {
            achievementRewards[3].SetActive(false);
        }





        achievementsGoNoGo.value = gonogoCount;
        achievementsCorsi.value = corsiCount;
        achievementsNback.value = nbackCount;
        achievementsEriksen.value = eriksenCount;

        achievementsCorsiText.text = corsiCount + "/20";
        achievementsGoNoGoText.text = gonogoCount + "/20";
        achievementsEriksenText.text = eriksenCount + "/20";
        achievementsNbackText.text = nbackCount + "/20";

        //Notify the user when an achievement is unlocked
        //resultText.text = "\n\nCongratulations!\nYou've completed the task\n\n\n\n\nPlay Go/No-Go";


    }

	public void updateDailyObjectives()
	{
		int gonogoCount = sn.countToday ("gonogo");
		int eriksenCount = sn.countToday("eriksen");
		int nbackCount = sn.countToday("nback");
		int corsiCount = sn.countToday("corsi");
        int incompleteCnt = 0;

		if (gonogoCount > 1)
		{
			gonogoCount = 1;
		}
		else if (eriksenCount > 1)
		{
			eriksenCount = 1;
		}
		else if (nbackCount > 1)
		{
			nbackCount = 1;
		}
		else if (corsiCount > 1)
		{
			corsiCount = 1;
		}

        if(gonogoCount == 1 && !PlayerPrefs.GetString("D1").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyReward = "Play Go/No-Go";
            dailyRewards[0].SetActive(false);
            dailyComplete[0].SetActive(true);
            PlayerPrefs.SetString("D1", System.DateTime.Now.Date.ToString());
        }
        else if (!PlayerPrefs.GetString("D1").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyComplete[0].SetActive(false);
            incompleteCnt++;
        }
        else if (PlayerPrefs.GetString("D1").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyRewards[0].SetActive(false);
        }

        if (nbackCount == 1 && !PlayerPrefs.GetString("D2").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyReward = "Play N-Back";
            dailyRewards[1].SetActive(false);
            dailyComplete[1].SetActive(true);
            PlayerPrefs.SetString("D2", System.DateTime.Now.Date.ToString());
        }
        else if (!PlayerPrefs.GetString("D2").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyComplete[1].SetActive(false);
            incompleteCnt++;
        }
        else if (PlayerPrefs.GetString("D2").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyRewards[1].SetActive(false);
        }

        if (corsiCount == 1 && !PlayerPrefs.GetString("D3").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyReward = "Play Corsi Block-Tapping";
            dailyRewards[2].SetActive(false);
            dailyComplete[2].SetActive(true);
            PlayerPrefs.SetString("D3", System.DateTime.Now.Date.ToString());
        }
        else if (!PlayerPrefs.GetString("D3").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyComplete[2].SetActive(false);
            incompleteCnt++;
        }
        else if (PlayerPrefs.GetString("D3").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyRewards[2].SetActive(false);
        }

        if (eriksenCount == 1 && !PlayerPrefs.GetString("D4").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyReward = "Play Eriksen Flanker";
            dailyRewards[3].SetActive(false);
            dailyComplete[3].SetActive(true);
            PlayerPrefs.SetString("D4", System.DateTime.Now.Date.ToString());
        }
        else if (!PlayerPrefs.GetString("D4").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyComplete[3].SetActive(false);
            incompleteCnt++;
        }
        else if (PlayerPrefs.GetString("D4").Equals(System.DateTime.Now.Date.ToString()))
        {
            dailyRewards[3].SetActive(false);
        }

        incompleteDaily.text = incompleteCnt + "";

        dailyObjectivesGoNoGo.value = gonogoCount;
		dailyObjectivesCorsi.value = corsiCount;
		dailyObjectivesNback.value = nbackCount;
		dailyObjectivesEriksen.value = eriksenCount;

		dailyObjectivesCorsiText.text = corsiCount + "/1";
		dailyObjectivesGoNoGoText.text = gonogoCount + "/1";
		dailyObjectivesEriksenText.text = eriksenCount + "/1";
		dailyObjectivesNbackText.text = nbackCount + "/1";
	}

    public void updateAssets()
    {
        if (PlayerPrefs.GetInt("accessory") == 11)
            accessory.overrideSprite = accessory_1;
        else
            accessory.overrideSprite = null;

        switch (PlayerPrefs.GetInt("head"))
        {
            case 21:
                head.overrideSprite = head_1;
                break;
            case 22:
                head.overrideSprite = head_2;
                break;
            case 0:
                head.overrideSprite = null;
                break;
        }

        switch (PlayerPrefs.GetInt("leg"))
        {
            case 41:
                leg.overrideSprite = leg_1;
                break;
            case 42:
                leg.overrideSprite = leg_2;
                break;
            case 43:
                leg.overrideSprite = leg_3;
                break;
            case 0:
                leg.overrideSprite = null;
                break;
        }

        switch (PlayerPrefs.GetInt("body"))
        {
            case 31:
                body.overrideSprite = body_1;
                break;
            case 32:
                body.overrideSprite = body_2;
                break;
            case 33:
                body.overrideSprite = body_3;
                break;
            case 34:
                body.overrideSprite = body_4;
                break;
            case 35:
                body.overrideSprite = body_5;
                break;
            case 0:
                body.overrideSprite = null;
                break;
        }
    }

    public void tryAsset(int itemNum)
    {
        if(itemNum == PlayerPrefs.GetInt("accessory"))
        {
            accessory.overrideSprite = null;
        }

        if (itemNum == PlayerPrefs.GetInt("head"))
        {
            head.overrideSprite = null;
        }

        if (itemNum == PlayerPrefs.GetInt("body"))
        {
            body.overrideSprite = null;
        }

        if (itemNum == PlayerPrefs.GetInt("leg"))
        {
            leg.overrideSprite = null;
        }

        switch (itemNum)
        {
            case 11:
                accessory.overrideSprite = accessory_1;
                break;
            case 21:
                head.overrideSprite = head_1;
                break;
            case 22:
                head.overrideSprite = head_2;
                break;
            case 31:
                body.overrideSprite = body_1;
                break;
            case 32:
                body.overrideSprite = body_2;
                break;
            case 33:
                body.overrideSprite = body_3;
                break;
            case 34:
                body.overrideSprite = body_4;
                break;
            case 35:
                body.overrideSprite = body_5;
                break;
            case 41:
                leg.overrideSprite = leg_1;
                break;
            case 42:
                leg.overrideSprite = leg_2;
                break;
            case 43:
                leg.overrideSprite = leg_3;
                break;
        }
    }

	public void mainPress()
	{

		PetScreen.gameObject.SetActive(false);
		MinigameScreen.gameObject.SetActive (false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        PetScreen.gameObject.SetActive(true);
        ResultsPanel.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);
        back.gameObject.SetActive(false);

        updateAttributes();
    }

	public void minigamePress()
	{
		PetScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(!MinigameScreen.gameObject.activeSelf);
        ResultsPanel.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);

        if (MinigameScreen.gameObject.activeSelf)
            back.gameObject.SetActive(true);
        else
        {
            back.gameObject.SetActive(false);
            PetScreen.gameObject.SetActive(true);
            updateAttributes();
        }
    }

    public void achievementPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(!AchievementScreen.gameObject.activeSelf);
        ResultsPanel.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);

        if (AchievementScreen.gameObject.activeSelf)
            back.gameObject.SetActive(true);
        else
        {
            back.gameObject.SetActive(false);
            PetScreen.gameObject.SetActive(true);
            updateAttributes();
        }
    }

    public void statisticsPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(!AttributeScreen.gameObject.activeSelf);
        ResultsPanel.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);
        if (AttributeScreen.gameObject.activeSelf)
        {
            back.gameObject.SetActive(true);
            generalStatPress();
            updateStatistics();
        }
        else
        {
            back.gameObject.SetActive(false);
            PetScreen.gameObject.SetActive(true);
            updateAttributes();
        }
    }

    public void customizationPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(!CustomizationScreen.gameObject.activeSelf);
        ResultsPanel.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(false);

        if (CustomizationScreen.gameObject.activeSelf)
            back.gameObject.SetActive(true);
        else
        {
            back.gameObject.SetActive(false);
            PetScreen.gameObject.SetActive(true);
            updateAttributes();
        }

    }

    public void settingsPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        ResultsPanel.gameObject.SetActive(false);
        SettingsScreen.gameObject.SetActive(!SettingsScreen.gameObject.activeSelf);

        if (SettingsScreen.gameObject.activeSelf)
            back.gameObject.SetActive(true);
        else
        {
            back.gameObject.SetActive(false);
            PetScreen.gameObject.SetActive(true);
            updateAttributes();
        }

    }

    public void resultsPress()
    {
        if (achievementReward.Length != 0)
        {
            rewards.SetActive(true);
            experience.text = "500";
            bearya.text = "500";
            resultText.text = "\nYou have unlocked\n\n\n\n\n" + achievementReward;
            resultI.overrideSprite = trophy;

            achievementReward = "";

        }
        else if (dailyReward.Length != 0)
        {
            rewards.SetActive(true);
            experience.text = "50";
            bearya.text = "50";
            resultText.text = "\nYou have unlocked\n\n\n\n\n" + dailyReward;
            resultI.overrideSprite = trophy;

            dailyReward = "";
        }
        else if (leveledup)
        {
            leveledup = false;
            resultText.text = "\nYou have reached level\n\n\n\n\nCongratulations!";
            resultLevel.text = PlayerPrefs.GetInt("Level") + "";
            resultI.overrideSprite = trophy;
        }
        else
            ResultsPanel.gameObject.SetActive(false);
    }

    public void gonogoPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene("Go No-Go");
    }

    public void nbackPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene("NBack");
    }

    public void corsiPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene("Corsi Block Tapping");
    }

    public void eriksenPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene("Eriksen Flanker");
    }

    public void updateAttributes()
    {
        int[] stats = sn.getPlayerStatistics(1);
        Debug.Log("STATS " + stats[0] + " " + stats[1] + " " + stats[2] + " " + stats[3] + "");
        Debug.Log("PSTATS " + Manager.Instance.pStats[0] + " " + Manager.Instance.pStats[1] + " " + Manager.Instance.pStats[2] + " " + Manager.Instance.pStats[3] + "");
        Memory.value = stats[0];
        MemoryS.value = stats[0];
        Accuracy.value = stats[1];
        AccuracyS.value = stats[1];
        Speed.value = stats[2];
        SpeedS.value = stats[2];
        Response.value = stats[3];
        ResponseS.value = stats[3];
        MemoryR.value = Manager.Instance.pStats[0];
        AccuracyR.value = Manager.Instance.pStats[1];
        SpeedR.value = Manager.Instance.pStats[2];
        ResponseR.value = Manager.Instance.pStats[3];

        MemoryT.text = stats[0] + "";
        AccuracyT.text = stats[1] + "";
        SpeedT.text = stats[2] + "";
        ResponseT.text = stats[3] + "";


        MemoryRT.text = "" + (stats[0] - Manager.Instance.pStats[0]);
        if (stats[0] - Manager.Instance.pStats[0] >= 0)
            MemoryRT.text = "+" + MemoryRT.text;
        AccuracyRT.text = "" + (stats[1] - Manager.Instance.pStats[1]);
        if (stats[1] - Manager.Instance.pStats[1] >= 0)
            AccuracyRT.text = "+" + AccuracyRT.text;
        SpeedRT.text = "" + (stats[2] - Manager.Instance.pStats[2]);
        if (stats[2] - Manager.Instance.pStats[2] >= 0)
            SpeedRT.text = "+" + SpeedRT.text;
        ResponseRT.text = "" + (stats[3] - Manager.Instance.pStats[3]);
        if (stats[3] - Manager.Instance.pStats[3] >= 0)
            ResponseRT.text = "+" + ResponseRT.text;

        int statAve = 0;

        for (int i = 0; i < stats.Length; i++)
        {
            statAve += stats[i];
        }

        statAve /= stats.Length;

        if ((stats[1] * 1.7) < stats[3])
        {
            status = "FIDGETY";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", true);
        }
        else if ((statAve * 0.6) > stats[0])
        {
            status = "FORGETFUL";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else if ((statAve * 0.6) > stats[2])
        {
            status = "SLOW";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else
        {
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
            status = "STABLE";

        }

        petStatus.text = status;

    }

    public void updateStatistics()
    {
        if (sn.getAvgGoNoGo()[2] == null)
            testsAverage[0].text = "Average Reaction Time: No Data";
        else
            testsAverage[0].text = "Average Reaction Time: " + ((Int32)float.Parse(sn.getAvgGoNoGo()[2].ToString())) + " ms";
        
        GoNoGoData g = sn.getBestGoNoGo();
        if(g == null)
        {
            testsBest[0].text = "Correct Go: No Data\nCorrect No-Go: No Data\nScore: No Data\nMean Reaction Time: No Data";

        }
        else
        {
            testsBest[0].text = "Correct Go: " + g.correct_go_count + "\nCorrect No-Go: " + g.correct_nogo_count + "\nScore: " + (g.correct_nogo_count + g.correct_go_count) + "/" + g.trial_count + "\nMean Reaction Time: " + ((Int32)g.mean_time) + " ms";
        }

        if (sn.getAvgNBack()[2] == null)
            testsAverage[1].text = "Accuracy: No Data";
        else
            testsAverage[1].text = "Accuracy: " + ((Int32)float.Parse(sn.getAvgNBack()[2].ToString()) * 100) + "%";
        
        NBackData n = sn.getBestNBack();
        if(n == null)
        {
            testsBest[1].text = "Score: No Data\nN Count: No Data";
        }
        else
            testsBest[1].text = "Score: " + n.correct_count + "/" + n.element_count + "\nN Count: " + n.n_count;

        if (sn.getAvgCorsi()[2] == null)
            testsAverage[2].text = "Accuracy: No Data\nCurrent Difficulty: " + (PlayerPrefs.GetInt("Corsi_Difficulty", 0) + 4) + " items";
        else
            testsAverage[2].text = "Accuracy: " + ((Int32)float.Parse(sn.getAvgCorsi()[2].ToString()) * 100) + "%" + "\nCurrent Difficulty: " + (PlayerPrefs.GetInt("Corsi_Difficulty", 0) + 4) + " items";
        
        CorsiData c = sn.getBestCorsi();
        if(c == null)
            testsBest[2].text = "Score: No Data\nSequence Length: No Data";
        else
            testsBest[2].text = "Score: " + c.correct_length + "/" + c.seq_length + "\nSequence Length: " + c.seq_length;


        testsAverage[3].text = "Ave. Reaction Time (Congruent): " + ((Int32)float.Parse(sn.getAvgEriksen()[2].ToString())) + " ms" + "\nAve. Reaction Time (Incongruent): " + ((Int32) float.Parse(sn.getAvgEriksen()[3].ToString())) + " ms" + "\nAverage Score: " + sn.getAvgEriksen()[4];
        if(sn.getAvgEriksen()[2] == null)
            testsAverage[3].text = "Ave. Reaction Time (Congruent): No Data\nAve. Reaction Time (Incongruent): No Data\nAverage Score: No Data";

        EriksenData e = sn.getBestEriksen();
        if(e == null)
            testsBest[3].text = "Score: No Data\nAve.Reaction Time (Congruent): No Data\nAve.Reaction Time (Incongruent): No Data\nCorrect Congruent: No Data\nCorrect Inconguent: No Data";
        else
        {
            testsBest[3].text = "Score: " + (e.correct_congruent + e.correct_incongruent) + "/" + e.trial_count
                    + "\nAve.Reaction Time (Congruent): " + ((Int32) e.time_congruent) + " ms" 
                    + "\nAve.Reaction Time (Incongruent): " + ((Int32) e.time_incongruent) + " ms"
                    + "\nCorrect Congruent: " + e.correct_congruent
                    + "\nCorrect Inconguent: " + e.correct_incongruent; 
        }

        String genData = "";

        if (sn.getMostPlayed().Length != 0)
        {
            genData += "Most Played Mini-game: \n";
            switch (sn.getMostPlayed())
            {
                case "gonogo": genData += "Go/No-Go";
                    break;
                case "nback": genData += "N-Back";
                    break;
                case "corsiblocktapping": genData += "Corsi Block-Tapping";
                    break;
                case "eriksenflanker": genData += "Eriksen Flanker";
                    break;
                case "null": genData += "No Data";
                    break;
            }
        }
        else
            genData += "Most Played Mini-game: \nNo Data";
        if (sn.getLeastPlayed().Length != 0)
        {
            genData += "\n\nLeast Played Mini-game: \n";
            switch (sn.getLeastPlayed())
            {
                case "gonogo": genData += "Go/No-Go";
                    break;
                case "nback": genData += "N-Back";
                    break;
                case "corsiblocktapping": genData += "Corsi Block-Tapping";
                    break;
                case "eriksenflanker": genData += "Eriksen Flanker";
                    break;
                case "null": genData += "No Data";
                    break;
            }        
        }
        else
            genData += "\n\nLeast Played Mini-game: \nNo Data";

        generalData.text = genData;
            
    }

    public void generalStatPress()
    {
        generalStatPanel.SetActive(true);
        minigameStatPanel.SetActive(false);

        generalStatBtn.color = new Color32(155, 89, 182, 255);
        gBtnBottom.color = new Color32(130, 76, 152, 255);
        minigameStatBtn.color = new Color32(179, 101, 212, 255);
        mBtnBottom.color = new Color32(141, 80, 167, 255);
    }

    public void minigameStatPress()
    {
        minigameStatPanel.SetActive(true);
        generalStatPanel.SetActive(false);

        minigameStatBtn.color = new Color32(155, 89, 182, 255);
        mBtnBottom.color = new Color32(130, 76, 152, 255);
        generalStatBtn.color = new Color32(179, 101, 212, 255);
        gBtnBottom.color = new Color32(141, 80, 167, 255);
    }

    public void jump()
    {

        if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping() && status.Equals("FORGETFUL"))
        {
            StartCoroutine(forgetfulJump());
            audioHandler.PlayOneShot(soundForgetful);
        }
        else if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping() && status.Equals("SLOW"))
        {
            StartCoroutine(slowJump());
            audioHandler.PlayOneShot(soundSlow);
        }
        else if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping() && status.Equals("FIDGETY"))
        {
            audioHandler.PlayOneShot(soundFidgety);
            RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
            RoboKuma.GetComponent<Rigidbody2D>().gravityScale = 250;
            RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 30000);
            
        }
        else if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping())
        {
            audioHandler.PlayOneShot(soundStable);
            RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
            RoboKuma.GetComponent<Rigidbody2D>().gravityScale = 250;
            RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 30000);

        }


    }

    public IEnumerator slowJump()
    {

        RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
        yield return new WaitForSecondsRealtime(0.5F);

        RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
        RoboKuma.GetComponent<Rigidbody2D>().gravityScale = 50;
        RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 15000);

    }

    public IEnumerator forgetfulJump()
    {

        RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
        Q1.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2F);
        Q2.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2F);
        Q3.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(0.2F);

        Q1.gameObject.SetActive(false);
        Q2.gameObject.SetActive(false);
        Q3.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(0.5F);

        RoboKuma.GetComponent<Rigidbody2D>().gravityScale = 50;
        RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 15000);

    }
    
    public void statusPress()
    {
        popup.SetActive(true);

        switch (status)
        {
            case "FORGETFUL":
                popupText.text = "Robokuma is currently forgetful because his MEMORY attribute is lagging behind. Remember the correct responses when playing N-BACK and CORSI BLOCK-TAPPING games to increase the memory attribute";
                break;
            case "FIDGETY":
                popupText.text = "Robokuma is fidgety because his ACCURACY attribute is too low when compared to his response attribute. Do the correct response when playing GO/NO-GO, and ERIKSEN FLANKER games to increase the accuracy attribute.";
                break;
            case "SLOW":
                popupText.text = "Robokuma is slow because his SPEED attribute is lagging behind. Respond faster when playing GO/NO-GO, N-BACK and ERIKSEN FLANKER games to increase the speed attribute.";
                break;
            case "STABLE":
                popupText.text = "Robokuma is currently stable, keep up the good work!";
                break;
        }

    }

    public void popupPress()
    {
        popup.SetActive(false);
        popupText.text = "";
    }

    public void dailyObjPress()
    {
        dailyPanel.SetActive(!dailyPanel.activeSelf);
    }

}
