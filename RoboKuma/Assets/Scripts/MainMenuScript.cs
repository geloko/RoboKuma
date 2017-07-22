using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Threading;
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
    public GameObject deletePanel;

    public Slider Memory, Response, Speed, Accuracy, MemoryR, ResponseR, SpeedR, AccuracyR, MemoryS, ResponseS, SpeedS, AccuracyS;
    public Image MemoryRF, ResponseRF, SpeedRF, AccuracyRF, MemorySF, ResponseSF, SpeedSF, AccuracySF;
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
    public Text speechBubbleText;
    public Image resultI;
    public Slider tExperience;

    public Slider achievementsGoNoGo, achievementsEriksen, achievementsCorsi, achievementsNback, achievementsMaxStat, achievementMemory, achievementResponse, achievementSpeed, achievementAccuracy;
    public Text achievementsGoNoGoText, achievementsEriksenText, achievementsCorsiText, achievementsNbackText, achievementsMaxStatText, achievementsMemoryText, achievementsResponseText, achievementsSpeedText, achievementsAccuracyText;
    public GameObject[] achievementRewards;
    public GameObject[] achievementComplete;

	public Slider dailyObjectivesGoNoGo, dailyObjectivesEriksen, dailyObjectivesCorsi, dailyObjectivesNback;
	public Text dailyObjectivesGoNoGoText, dailyObjectivesEriksenText, dailyObjectivesCorsiText, dailyObjectivesNbackText;
    public GameObject[] dailyRewards;
    public GameObject[] dailyComplete;

    public Image leg, body, head, accessory, eyes;
    public CustomizationScript customScript;

    public Button back;
    public Image pointer1;
    public Button cBtn, sBtn, mBtn, aBtn, iBtn,statusBtn;
    public GameObject dBtn;

    public int log_id { get; set; }

    public String status;

    public Boolean leveledup = false;
    public bool inTutorial = false;

    public ArrayList messages;
    public String[] bookComments;
    public String[] bearyaComments;
    public IEnumerator speechCoroutine;

    public SQLiteDatabase sn;
	public SSH_Connector ssh_connector;

    public Sprite[] bodySprites;
    public Sprite[] legSprites;
    public Sprite[] headSprites;
    public Sprite[] accessorySprites;
    public Sprite trophy, sad_icon, eyes_2, dailyCompleted;


    public String dailyReward = "";
    public String achievementReward = "";
    public ArrayList achievements;
    
    //for sounds SFX
    public AudioSource audioHandler;
    public AudioClip soundStable;
    public AudioClip soundSlow;
    public AudioClip soundFidgety;
    public AudioClip soundForgetful;
    public AudioClip soundListless;
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
        deletePanel.SetActive(false);

        back.gameObject.SetActive(false);

        SpeechBubble.SetActive(false);
        pointer1.gameObject.SetActive(false);
        popup.SetActive(false);
        dailyPanel.SetActive(false);

        customScript = (CustomizationScript) CustomizationScreen.GetComponent(typeof(CustomizationScript));

        sn = new SQLiteDatabase();
		ssh_connector = new SSH_Connector ();
        status = "STABLE";

        bookComments = new String[2];
        bearyaComments = new String[4];

        bookComments[0] = "So many books, so little time.";
        bookComments[1] = "Do bears even need books?";

        bearyaComments[0] = "I am in need of financial uplifting.";
        bearyaComments[1] = "Why is it called Bearya?";
        bearyaComments[2] = "Money often costs too much.";
        bearyaComments[3] = "The best thing money can buy is financial freedom.";

        //PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetInt("DB"));

        audioHandler.clip = backGround;
        audioHandler.Play();
        audioHandler.loop = true;

        bool attributesReduced = false;
        inTutorial = false;
        int reduction = 0;
        if (!PlayerPrefs.GetString("Last_Played", "N/A").Equals("N/A"))
        {
            DateTime dt = DateTime.Parse(PlayerPrefs.GetString("Last_Played"));
            reduction = System.DateTime.Now.Subtract(dt).Hours / 24 * 1;
            Debug.Log("Hours: " + System.DateTime.Now.Subtract(dt).Hours + " " + dt.ToString());

            if (reduction > 0)
            {
				Manager.Instance.pStats = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
                PlayerPrefs.SetString("Last_Played", System.DateTime.Now.ToString("g"));
                attributesReduced = true;

				int[] attributes = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
                Debug.Log("Reduce by: " + reduction);
                for (int i = 0; i < attributes.Length; i++)
                {
                    Debug.Log(attributes[i]);
                    attributes[i] -= reduction;
                    Debug.Log(attributes[i]);
                }
                
                sn.updateAttributes(attributes);

            }
        }

        messages = new ArrayList();
        achievements = new ArrayList();
		ThreadPool.SetMaxThreads (1, 1);
        if (PlayerPrefs.GetInt("DB", -1) == -1)
        {
            sn.Start();
			object state;
			ThreadPool.QueueUserWorkItem (ssh_connector.Start);
            PlayerPrefs.SetInt("DB", 1);
            Debug.Log("DB");

            SpeechBubble.SetActive(true);

            inTutorial = true;

            messages.Add("Hi I am Robokuma, nice to meet you.");
            messages.Add("I believe you're here to help me improve my skills.");
            messages.Add("You can do so by playing the mini-games here.");
            messages.Add("Doing so would affect my attributes and current status.");
            messages.Add("You can see my attributes and your performance in the mini-games here.");
            messages.Add("You can see my current status here.");
            messages.Add("You can customize my looks here.");
            messages.Add("You can view the achievements you have unlocked here.");
            messages.Add("Also remember to play everyday to get the daily rewards.");
            messages.Add("Playing regularly also prevents the degradation of my skills.");
            messages.Add("For more information, you can refer here.");
            messages.Add("You can now start by playing a mini-game.");
            messages.Add("Good Luck!");
            speechBubbleText.text = messages[0].ToString();
            messages.RemoveAt(0);
            SpeechBubble.GetComponent<Animator>().SetBool("hasNextSpeech", true);

            cBtn.interactable = false;
            sBtn.interactable = false;
            mBtn.interactable = false;
            aBtn.interactable = false;
            iBtn.interactable = false;
            statusBtn.interactable = false;
            dBtn.SetActive(false);

        }
        else
        {
            SpeechBubble.SetActive(true);
            speechBubbleText.text = "Welcome Back!";
            speechCoroutine = closeSpeechBubble();
            StartCoroutine(speechCoroutine);
        }

        updateAttributes();
        updateAchievements();
        updateAssets();
        updateDailyObjectives();

        if (PlayerPrefs.GetInt("Score", -1) != 1 && !attributesReduced)
            ResultsPanel.gameObject.SetActive(false);
        else if(attributesReduced)
        {
            resultI.overrideSprite = null;
            resultLevel.text = "";
            rewards.SetActive(false);
            
            resultText.text = "\nRobokuma's skills have degraded\n\n\n\n\ndue to your absence";
            resultI.overrideSprite = sad_icon;

        }
        else
        {
			object state;
			ThreadPool.QueueUserWorkItem(ssh_connector.callUploadPlayerLogs);
            resultI.overrideSprite = null;
            resultLevel.text = "";
            rewards.SetActive(false);

            for(int i = 0; i < achievements.Count; i++)
            {
                int exp = PlayerPrefs.GetInt("TExperience", 0);
                int bear = PlayerPrefs.GetInt("TBearya", 0);
                exp += 500;
                bear += 600;
                PlayerPrefs.SetInt("TBearya", bear);
                PlayerPrefs.SetInt("TExperience", exp);
            }

            if(dailyReward.Length != 0)
            {
                int exp = PlayerPrefs.GetInt("TExperience", 0);
                int bear = PlayerPrefs.GetInt("TBearya", 0);
                exp += 50;
                bear += 50;
                PlayerPrefs.SetInt("TBearya", bear);
                PlayerPrefs.SetInt("TExperience", exp);
            }
            
            //experience.text = PlayerPrefs.GetInt("Experience", 0) + "";
            //bearya.text = PlayerPrefs.GetInt("Bearya", 0) + "";

            int tExp = PlayerPrefs.GetInt("TExperience", 0) + PlayerPrefs.GetInt("Experience", 0);
            int tBear = PlayerPrefs.GetInt("TBearya", 0) + PlayerPrefs.GetInt("Bearya", 0);

            while (tExp >= Math.Pow((PlayerPrefs.GetInt("Level", 0) + 1), 3))
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 0) + 1);
                leveledup = true;
            }

            
            if (achievements.Count != 0)
            {
                rewards.SetActive(true);
                audioHandler.PlayOneShot(soundAchievement);
                experience.text = "500";
                bearya.text = "600";
                resultText.text = "\nYou have unlocked\n\n\n\n\n" + achievements[0].ToString();
                resultI.overrideSprite = trophy;

                achievements.RemoveAt(0);

            }
            else if (dailyReward.Length != 0)
            {
                rewards.SetActive(true);
                experience.text = "50";
                bearya.text = "50";
                audioHandler.PlayOneShot(soundAchievement);
                resultText.text = "\nYou have completed\n\n\n\n\n" + dailyReward;
                resultI.overrideSprite = dailyCompleted;

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

        customScript.updateLockedItems();

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
            NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(86400), "RoboKuma", "Robokuma misses you.", new Color(1, 0.3f, 0.15f), NotificationIcon.Message);
        }
    }

    public void speechBubblePress()
    {
        if(messages.Count > 0)
        {
            speechBubbleText.text = messages[0].ToString();
            pointer1.gameObject.SetActive(false);
            pointer1.GetComponent<Animator>().SetBool("pointer", false);


            switch (messages[0].ToString())
            {
                case "You can now start by playing a mini-game.":
                case "You can do so by playing the mini-games here.":
                    iBtn.interactable = false;
                    mBtn.interactable = true;
                    //pointer1.gameObject.SetActive(true);
                    //pointer1.GetComponent<Animator>().SetBool("pointer", true);
                    break;
                case "You can see my attributes and your performance in the mini-games here.":
                    mBtn.interactable = false;
                    sBtn.interactable = true;
                    //pointer2.gameObject.SetActive(true);
                    //pointer2.GetComponent<Animator>().SetBool("pointer", true);
                    break;
                case "You can customize my looks here.":
                    cBtn.interactable = true;
                    break;
                case "You can view the achievements you have unlocked here.":
                    cBtn.interactable = false;
                    aBtn.interactable = true;
                    break;
                case "Also remember to play everyday to get the daily rewards":
                    aBtn.interactable = false;
                    dBtn.SetActive(true);
                    break;
                case "You can see my current status here.":
                    sBtn.interactable = false;
                    pointer1.gameObject.SetActive(true);
                    pointer1.GetComponent<Animator>().SetBool("pointer", true);
                    statusBtn.interactable = true;
                    break;
                case "For more information, you can refer here.":
                    iBtn.interactable = true;
                    break;
                case "Good Luck!":
                    cBtn.interactable = true;
                    sBtn.interactable = true;
                    mBtn.interactable = true;
                    aBtn.interactable = true;
                    iBtn.interactable = true;
                    dBtn.SetActive(true);
                    inTutorial = false;
                    break;              
            }
            messages.RemoveAt(0);
            if(messages.Count == 0)
                SpeechBubble.GetComponent<Animator>().SetBool("hasNextSpeech", false);

        }
        else
        {
            SpeechBubble.SetActive(false);
        }
    }

    public IEnumerator closeSpeechBubble()
    {
        yield return new WaitForSecondsRealtime(1.2F);
        SpeechBubble.SetActive(false);
    }

    public void updateAchievements()
    {
        int gonogoCount = sn.count("gonogo");
        int eriksenCount = sn.count("eriksen");
        int nbackCount = sn.count("nback");
        int corsiCount = sn.count("corsi");

        if (gonogoCount > 30)
        {
            gonogoCount = 30;
        }

        if (eriksenCount > 30)
        {
            eriksenCount = 30;
        }

        if (nbackCount > 30)
        {
            nbackCount = 30;
        }

        if (corsiCount > 30)
        {
            corsiCount = 30;
        }

        if (gonogoCount == 30 && PlayerPrefs.GetInt("A1", 0) == 0)
        {
            achievements.Add("Go/No-Go Veteran");
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

        if (nbackCount == 30 && PlayerPrefs.GetInt("A2", 0) == 0)
        {
            achievements.Add("N-Back Veteran");
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

        if (corsiCount == 30 && PlayerPrefs.GetInt("A3", 0) == 0)
        {
            achievements.Add("Corsi Block-Tapping Veteran");
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

        if (eriksenCount == 30 && PlayerPrefs.GetInt("A4", 0) == 0)
        {
            achievements.Add(achievementReward = "Eriksen Flanker Veteran");
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

        if (Memory.value >= 100 && Response.value >= 100 && Speed.value >= 100 && Accuracy.value >= 100 && PlayerPrefs.GetInt("A5", 0) == 0)
        {
            achievements.Add("Max All Stats");
            achievementRewards[4].SetActive(false);
            achievementComplete[4].SetActive(true);
            PlayerPrefs.SetInt("A5", 1);
        }
        else if (PlayerPrefs.GetInt("A5", 0) == 0)
        {
            achievementComplete[4].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A5", 0) == 1)
        {
            achievementRewards[4].SetActive(false);
        }

        if (Memory.value >= 100 && PlayerPrefs.GetInt("A6", 0) == 0)
        {
            achievements.Add("Max Memory");
            achievementRewards[5].SetActive(false);
            achievementComplete[5].SetActive(true);
            PlayerPrefs.SetInt("A6", 1);
        }
        else if (PlayerPrefs.GetInt("A6", 0) == 0)
        {
            achievementComplete[5].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A6", 0) == 1)
        {
            achievementRewards[5].SetActive(false);
        }

        if (Response.value >= 100 && PlayerPrefs.GetInt("A7", 0) == 0)
        {
            achievements.Add("Max Response");
            achievementRewards[6].SetActive(false);
            achievementComplete[6].SetActive(true);
            PlayerPrefs.SetInt("A7", 1);
        }
        else if (PlayerPrefs.GetInt("A7", 0) == 0)
        {
            achievementComplete[6].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A7", 0) == 1)
        {
            achievementRewards[6].SetActive(false);
        }

        if (Speed.value >= 100 && PlayerPrefs.GetInt("A8", 0) == 0)
        {
            achievements.Add("Max Speed");
            achievementRewards[7].SetActive(false);
            achievementComplete[7].SetActive(true);
            PlayerPrefs.SetInt("A8", 1);
        }
        else if (PlayerPrefs.GetInt("A8", 0) == 0)
        {
            achievementComplete[7].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A8", 0) == 1)
        {
            achievementRewards[7].SetActive(false);
        }

        if (Accuracy.value >= 100 && PlayerPrefs.GetInt("A9", 0) == 0)
        {
            achievements.Add("Max Accuracy");
            achievementRewards[8].SetActive(false);
            achievementComplete[8].SetActive(true);
            PlayerPrefs.SetInt("A9", 1);
        }
        else if (PlayerPrefs.GetInt("A9", 0) == 0)
        {
            achievementComplete[8].SetActive(false);
        }
        else if (PlayerPrefs.GetInt("A9", 0) == 1)
        {
            achievementRewards[8].SetActive(false);
        }
        int maxstats = 0;
        for(int i = 6; i <= 9; i++)
        {
            if (PlayerPrefs.GetInt("A" + i, 0) == 1)
                maxstats++;
        }

        achievementsGoNoGo.value = gonogoCount;
		achievementsGoNoGo.maxValue = 30;
        achievementsCorsi.value = corsiCount;
		achievementsCorsi.maxValue = 30;
        achievementsNback.value = nbackCount;
		achievementsNback.maxValue = 30;
        achievementsEriksen.value = eriksenCount;
		achievementsEriksen.maxValue = 30;
        achievementsMaxStat.value = maxstats;
		achievementsMaxStat.maxValue = 4;
        achievementMemory.value = Memory.value;
		achievementMemory.maxValue = 100;
        achievementResponse.value = Response.value;
		achievementResponse.maxValue = 100;
        achievementSpeed.value = Speed.value;
		achievementSpeed.maxValue = 100;
        achievementAccuracy.value = Accuracy.value;
		achievementAccuracy.maxValue = 100;
        achievementsMaxStatText.text = maxstats + "/4";
        achievementsMemoryText.text = Memory.value + "/100";
        achievementsResponseText.text = Response.value + "/100";
        achievementsSpeedText.text = Speed.value + "/100";
        achievementsAccuracyText.text = Accuracy.value + "/100";

        achievementsCorsiText.text = corsiCount + "/30";
        achievementsGoNoGoText.text = gonogoCount + "/30";
        achievementsEriksenText.text = eriksenCount + "/30";
        achievementsNbackText.text = nbackCount + "/30";

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

        if (eriksenCount > 1)
		{
			eriksenCount = 1;
		}

        if (nbackCount > 1)
		{
			nbackCount = 1;
		}

        if (corsiCount > 1)
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
        for (int i = 0; i < accessorySprites.Length; i++)
        {
            if (PlayerPrefs.GetInt("accessory") == 11 + i)
                accessory.overrideSprite = accessorySprites[i];
            else if(PlayerPrefs.GetInt("accessory") == 0)
            {
                accessory.overrideSprite = null;
                break;
            }
        }

        for (int i = 0; i < headSprites.Length; i++)
        {
            if (PlayerPrefs.GetInt("head") == 21 + i)
                head.overrideSprite = headSprites[i];
            else if (PlayerPrefs.GetInt("head") == 0)
            {
                head.overrideSprite = null;
                break;
            }
        }

        for (int i = 0; i < bodySprites.Length; i++)
        {
            if (PlayerPrefs.GetInt("body") == 31 + i)
                body.overrideSprite = bodySprites[i];
            else if (PlayerPrefs.GetInt("body") == 0)
            {
                body.overrideSprite = null;
                break;
            }
        }

        for (int i = 0; i < legSprites.Length; i++)
        {
            if (PlayerPrefs.GetInt("leg") == 41 + i)
                leg.overrideSprite = legSprites[i];
            else if (PlayerPrefs.GetInt("leg") == 0)
            {
                leg.overrideSprite = null;
                break;
            }
        }
    }

    public void tryAsset(int itemNum)
    {
        for (int i = 0; i < accessorySprites.Length; i++)
        {
            if (itemNum == 11 + i)
                accessory.overrideSprite = accessorySprites[i];
        }

        for (int i = 0; i < headSprites.Length; i++)
        {
            if (itemNum == 21 + i)
                head.overrideSprite = headSprites[i];
        }

        for (int i = 0; i < bodySprites.Length; i++)
        {
            if (itemNum == 31 + i)
                body.overrideSprite = bodySprites[i];
        }

        for (int i = 0; i < legSprites.Length; i++)
        {
            if (itemNum == 41 + i)
                leg.overrideSprite = legSprites[i];
        }

        /*if (itemNum == PlayerPrefs.GetInt("accessory"))
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
        }*/

        dailyPanel.SetActive(false);
        popup.SetActive(false);
        SpeechBubble.SetActive(false);
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
        if(!inTutorial)
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
		
    }

    public void achievementPress()
    {
        if (!inTutorial)
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
    }

    public void statisticsPress()
    {
        if (!inTutorial)
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
    }

    public void customizationPress()
    {
        if (!inTutorial)
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
    }

    public void settingsPress()
    {
        if (!inTutorial)
        {
            PetScreen.gameObject.SetActive(false);
            MinigameScreen.gameObject.SetActive(false);
            AchievementScreen.gameObject.SetActive(false);
            AttributeScreen.gameObject.SetActive(false);
            CustomizationScreen.gameObject.SetActive(false);
            ResultsPanel.gameObject.SetActive(false);
            SettingsScreen.gameObject.SetActive(!SettingsScreen.gameObject.activeSelf);

            if (SettingsScreen.gameObject.activeSelf)
            {
                back.gameObject.SetActive(true);
                deletePanel.SetActive(false);
            }
            else
            {
                back.gameObject.SetActive(false);
                PetScreen.gameObject.SetActive(true);
                updateAttributes();
            }
        }
    }

    public void resultsPress()
    {
        if (achievements.Count != 0)
        {
            rewards.SetActive(true);
            experience.text = "500";
            bearya.text = "500";
            audioHandler.PlayOneShot(soundAchievement);
            resultText.text = "\nYou have unlocked\n\n\n\n\n" + achievements[0].ToString();
            resultI.overrideSprite = trophy;

            achievements.RemoveAt(0);

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
            leveledup = false;
            audioHandler.PlayOneShot(soundLevelup);
            resultText.text = "\nYou have reached level\n\n\n\n\nCongratulations!";
            resultLevel.text = PlayerPrefs.GetInt("Level") + "";
            resultI.overrideSprite = trophy;
        }
        else
            ResultsPanel.gameObject.SetActive(false);
    }

    public void gonogoPress()
    {
		Manager.Instance.pStats = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
        SceneManager.LoadScene("Go No-Go");
    }

    public void nbackPress()
    {
		Manager.Instance.pStats = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
        SceneManager.LoadScene("NBack");
    }

    public void corsiPress()
    {
		Manager.Instance.pStats = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
        SceneManager.LoadScene("Corsi Block Tapping");
    }

    public void eriksenPress()
    {
		Manager.Instance.pStats = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
        SceneManager.LoadScene("Eriksen Flanker");
    }

    public void updateAttributes()
    {
		int[] stats = sn.getPlayerStatistics(SQLiteDatabase.getPlayer().player_id);
        Debug.Log("STATS " + stats[0] + " " + stats[1] + " " + stats[2] + " " + stats[3] + "");
        Debug.Log("PSTATS " + Manager.Instance.pStats[0] + " " + Manager.Instance.pStats[1] + " " + Manager.Instance.pStats[2] + " " + Manager.Instance.pStats[3] + "");


        Memory.value = stats[0];
        Accuracy.value = stats[1];
        Speed.value = stats[2];
        Response.value = stats[3];

        if (stats[0] >= 100)
            stats[0] = 100;

        if (stats[1] >= 100)
            stats[1] = 100;

        if (stats[2] >= 100)
            stats[2] = 100;
    
        if (stats[3] >= 100)
            stats[3] = 100;


        if (stats[0] >= Manager.Instance.pStats[0])
        {
            MemoryS.value = stats[0];
            MemoryR.value = Manager.Instance.pStats[0];

        }
        else
        {
            MemoryR.value = stats[0];
            MemoryS.value = Manager.Instance.pStats[0];
            MemorySF.color = new Color32(253, 95, 95, 255);
            MemoryRF.color = new Color32(255, 255, 255, 255);
        }

        if (stats[1] >= Manager.Instance.pStats[1])
        {
            AccuracyS.value = stats[1];
            AccuracyR.value = Manager.Instance.pStats[1];

        }
        else
        {
            AccuracyR.value = stats[1];
            AccuracyS.value = Manager.Instance.pStats[1];
            AccuracySF.color = new Color32(253, 95, 95, 255);
            AccuracyRF.color = new Color32(255, 255, 255, 255);

        }

        if (stats[2] >= Manager.Instance.pStats[2])
        {
            SpeedS.value = stats[2];
            SpeedR.value = Manager.Instance.pStats[2];

        }
        else
        {
            SpeedR.value = stats[2];
            SpeedS.value = Manager.Instance.pStats[2];
            SpeedSF.color = new Color32(253, 95, 95, 255);
            SpeedRF.color = new Color32(255, 255, 255, 255);

        }

        if (stats[3] >= Manager.Instance.pStats[3])
        {
            ResponseS.value = stats[3];
            ResponseR.value = Manager.Instance.pStats[3];

        }
        else
        {
            ResponseR.value = stats[3];
            ResponseS.value = Manager.Instance.pStats[3];
            ResponseSF.color = new Color32(253, 95, 95, 255);
            ResponseRF.color = new Color32(255, 255, 255, 255);
        }

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

        eyes.overrideSprite = null;

        if ((stats[1] * 1.7) < stats[3] && stats[1] >= 0)
        {
            status = "FIDGETY";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", true);
            SpeechBubble.SetActive(true);
        }
        else if (((statAve * 0.6) > stats[0] && statAve >= 0) || stats[0] < 0)
        {
            status = "FORGETFUL";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else if (((statAve * 0.6) > stats[1] && statAve >= 0) || stats[1] < 0)
        {
            status = "CLUMSY";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else if (((statAve * 0.6) > stats[2] && statAve >= 0) || stats[2] < 0)
        {
            status = "SLOW";
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else if (((statAve * 0.6) > stats[3] && statAve >= 0) || stats[3] < 0)
        {
            status = "LISTLESS";
            eyes.overrideSprite = eyes_2;
            RKAnimation.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else if (stats[0] >= 100 && stats[1] >= 100 && stats[2] >= 100 && stats[3] >= 100)
        {
            status = "GOD";
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
            testsAverage[0].text = "Avg. Reaction Time: No Data";
        else
            testsAverage[0].text = "Avg. Reaction Time: " + ((Int32)float.Parse(sn.getAvgGoNoGo()[2].ToString())) + " ms";
        
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
            testsAverage[1].text = "Accuracy: " + ((Int32)(Double.Parse(sn.getAvgNBack()[2].ToString()) * 100)) + "%";
        
        NBackData n = sn.getBestNBack();
        if(n == null)
        {
            testsBest[1].text = "Score: No Data\nN Count: No Data";
        }
        else
            testsBest[1].text = "Score: " + n.correct_count + "/" + n.trial_count + "\nN Count: " + n.n_count;

        if (sn.getAvgCorsi()[2] == null)
            testsAverage[2].text = "Accuracy: No Data\nCurrent Difficulty: " + (PlayerPrefs.GetInt("Corsi_Difficulty", 0) + 4) + " items";
        else
            testsAverage[2].text = "Accuracy: " + ((Int32)(float.Parse(sn.getAvgCorsi()[2].ToString()) * 100)) + "%" + "\nCurrent Difficulty: " + (PlayerPrefs.GetInt("Corsi_Difficulty", 0) + 4) + " items";
        
        CorsiData c = sn.getBestCorsi();
        if(c == null)
            testsBest[2].text = "Score: No Data\nSequence Length: No Data";
        else
            testsBest[2].text = "Score: " + c.correct_length + "/" + c.seq_length + "\nSequence Length: " + c.seq_length;


        if (sn.getAvgEriksen()[2] == null)
            testsAverage[3].text = "Avg. Reaction Time (Congruent): No Data" + "\nAvg. Reaction Time (Incongruent): No Data" + "\nAvg. Score: No Data";
        else
            testsAverage[3].text = "Avg. Reaction Time (Congruent): " + ((Int32)Double.Parse(sn.getAvgEriksen()[2].ToString())) + " ms" + "\nAvg. Reaction Time (Incongruent): " + ((Int32)float.Parse(sn.getAvgEriksen()[3].ToString())) + " ms" + "\nAvg. Score: " + sn.getAvgEriksen()[4];

        EriksenData e = sn.getBestEriksen();
        if(e == null)
            testsBest[3].text = "Score: No Data\nAvg. Reaction Time (Congruent): No Data\nAvg. Reaction Time (Incongruent): No Data\nCorrect Congruent: No Data\nCorrect Inconguent: No Data";
        else
        {
            testsBest[3].text = "Score: " + (e.correct_congruent + e.correct_incongruent) + "/" + e.trial_count
                    + "\nAvg. Reaction Time (Congruent): " + ((Int32) e.time_congruent) + " ms" 
                    + "\nAvg. Reaction Time (Incongruent): " + ((Int32) e.time_incongruent) + " ms"
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
                case "null":
                case "N/A":
                    genData += "No Data";
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
                case "null":
                case "N/A":
                    genData += "No Data";
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

        minigameStatBtn.color = new Color32(155, 89, 182, 255);
        mBtnBottom.color = new Color32(130, 76, 152, 255);
        generalStatBtn.color = new Color32(179, 101, 212, 255);
        gBtnBottom.color = new Color32(141, 80, 167, 255);
    }

    public void minigameStatPress()
    {
        minigameStatPanel.SetActive(true);
        generalStatPanel.SetActive(false);

        generalStatBtn.color = new Color32(155, 89, 182, 255);
        gBtnBottom.color = new Color32(130, 76, 152, 255);
        minigameStatBtn.color = new Color32(179, 101, 212, 255);
        mBtnBottom.color = new Color32(141, 80, 167, 255);
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
        else if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping() && status.Equals("LISTLESS"))
        {
            StartCoroutine(slowJump());
            audioHandler.PlayOneShot(soundListless);
        }
        else if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping() && status.Equals("FIDGETY"))
        {
            audioHandler.PlayOneShot(soundFidgety);
            RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
            RoboKuma.GetComponent<Rigidbody2D>().gravityScale = 100;
            RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 35000);

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
        dailyPanel.SetActive(false);

        switch (status)
        {
            case "FORGETFUL":
                popupText.text = "Robokuma is currently forgetful because his MEMORY attribute is lagging behind. Remember the correct responses when playing N-BACK and CORSI BLOCK-TAPPING games to increase the memory attribute";
                SpeechBubble.SetActive(true);
                if(!inTutorial)
                    speechBubbleText.text = "I am forgetful.";
                break;
            case "FIDGETY":
                popupText.text = "Robokuma is fidgety because his ACCURACY attribute is too low when compared to his response attribute. Do the correct response when playing GO/NO-GO, and ERIKSEN FLANKER games to increase the accuracy attribute.";
                SpeechBubble.SetActive(true);
                if (!inTutorial)
                    speechBubbleText.text = "I am fidgety.";
                break;
            case "SLOW":
                popupText.text = "Robokuma is slow because his SPEED attribute is lagging behind. Respond faster when playing GO/NO-GO, N-BACK and ERIKSEN FLANKER games to increase the speed attribute.";
                SpeechBubble.SetActive(true);
                if (!inTutorial)
                    speechBubbleText.text = "I am slow.";
                break;
            case "CLUMSY":
                popupText.text = "Robokuma is clumsy because his ACCURACY attribute is lagging behind.  Do the correct response when playing GO/NO-GO, and ERIKSEN FLANKER games to increase the accuracy attribute.";
                SpeechBubble.SetActive(true);
                if (!inTutorial)
                    speechBubbleText.text = "I am clumsy.";
                break;
            case "LISTLESS":
                popupText.text = "Robokuma is listless because his RESPONSE attribute is lagging behind. Respond faster when playing GO/NO-GO and ERIKSEN FLANKER games to increase the response attribute.";
                SpeechBubble.SetActive(true);
                if (!inTutorial)
                    speechBubbleText.text = "I am listless.";
                break;
            case "GOD":
                popupText.text = "Robokuma has broken through the limits of his cognitive abilities. Congratulations!";
                SpeechBubble.SetActive(true);
                if (!inTutorial)
                    speechBubbleText.text = "I am a god?";
                break;
            case "STABLE":
                popupText.text = "Robokuma is currently stable, keep up the good work!";
                SpeechBubble.SetActive(true);
                if (!inTutorial)
                    speechBubbleText.text = "I am fine.";
                break;
        }

    }

    public void popupPress()
    {
        popup.SetActive(false);
        popupText.text = "";
        if(!inTutorial)
            SpeechBubble.SetActive(false);
    }

    public void dailyObjPress()
    {
        dailyPanel.SetActive(!dailyPanel.activeSelf);

        if (dailyPanel.activeSelf)
            popup.SetActive(false);
    }

    public void bookPress()
    {
        if (!inTutorial)
        {
            SpeechBubble.SetActive(true);
            speechBubbleText.text = bookComments[new System.Random().Next(bookComments.Length)];
            if(speechCoroutine != null)
                StopCoroutine(speechCoroutine);
            speechCoroutine = closeSpeechBubble();
            StartCoroutine(speechCoroutine);

        }
    }

    public void coinPress()
    {
        if (!inTutorial)
        {
            SpeechBubble.SetActive(true);
            speechBubbleText.text = bearyaComments[new System.Random().Next(bearyaComments.Length)];
            if (speechCoroutine != null)
                StopCoroutine(speechCoroutine);
            speechCoroutine = closeSpeechBubble();
            StartCoroutine(speechCoroutine);
        }
    }

    public void deleteData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main ver 2");
    }

    public void donotDeletePress()
    {
        deletePanel.SetActive(false);
    }

    public void deletePress()
    {
        deletePanel.SetActive(true);
    }
}
