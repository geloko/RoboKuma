using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MainMenuScript : MonoBehaviour {

	public Canvas MainScreen;
    public GameObject PetScreen;
	public GameObject MinigameScreen;
    public GameObject CustomizationScreen;
    public GameObject AchievementScreen;
    public GameObject AttributeScreen;
    public GameObject ResultsPanel;
    public GameObject RoboKuma;
    public GameObject Q1, Q2, Q3;

    public Slider Memory, Response, Speed, Accuracy, MemoryR, ResponseR, SpeedR, AccuracyR;
    public Text MemoryT, ResponseT, SpeedT, AccuracyT, MemoryRT, ResponseRT, SpeedRT, AccuracyRT;
    public Text petStatus;
    public Text experience, bearya;
    public Text tLevel, tBearya;
    public Slider tExperience;

    public bool isForgetful;

    public SQLiteDatabase sn;


    // Use this for initialization
    void Start () {

		MainScreen = MainScreen.GetComponent<Canvas>();
        MinigameScreen = GameObject.Find ("MinigameScreen");
		PetScreen = GameObject.Find ("PetScreen");
        CustomizationScreen = GameObject.Find("CustomizationScreen");
        AchievementScreen = GameObject.Find("AchievementScreen");
        AttributeScreen = GameObject.Find("AttributeScreen");
        MinigameScreen.gameObject.SetActive (false);
        CustomizationScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        Q1.gameObject.SetActive(false);
        Q2.gameObject.SetActive(false);
        Q3.gameObject.SetActive(false);

        sn = new SQLiteDatabase();
        isForgetful = false;

        //PlayerPrefs.DeleteAll();
        Debug.Log(PlayerPrefs.GetInt("DB"));

        if (PlayerPrefs.GetInt("DB", -1) == -1)
        {
            sn.Start();
            PlayerPrefs.SetInt("DB", 1);
            Debug.Log("DB");
        }

        updateAttributes();

        if (PlayerPrefs.GetInt("Score", -1) != 1)
            ResultsPanel.gameObject.SetActive(false);
        else
        {
            experience.text = PlayerPrefs.GetInt("Experience", 0) + "";
            bearya.text = PlayerPrefs.GetInt("Bearya", 0) + "";

            int tExp = PlayerPrefs.GetInt("TExperience", 0) + PlayerPrefs.GetInt("Experience", 0);
            int tBear = PlayerPrefs.GetInt("TBearya", 0) + PlayerPrefs.GetInt("Bearya", 0);

            while (tExp >= Math.Pow((PlayerPrefs.GetInt("Level", 1) + 1), 3))
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            
            PlayerPrefs.SetInt("TExperience", tExp);
            PlayerPrefs.SetInt("TBearya", tBear);

            PlayerPrefs.SetInt("Experience", 0);
            PlayerPrefs.SetInt("Bearya", 0);

            ResultsPanel.gameObject.SetActive(true);
            PlayerPrefs.SetInt("Score", 0);
        }

        tLevel.text = "LVL" + PlayerPrefs.GetInt("Level", 1);
        tBearya.text = PlayerPrefs.GetInt("TBearya", 0) + "";

        Debug.Log(PlayerPrefs.GetInt("TExperience", 0));
        Debug.Log(Math.Pow((PlayerPrefs.GetInt("Level", 1)), 3));

        tExperience.value = (float) (PlayerPrefs.GetInt("TExperience", 0) - Math.Pow((PlayerPrefs.GetInt("Level", 1)), 3));
        tExperience.maxValue = (float) (Math.Pow((PlayerPrefs.GetInt("Level", 1) + 1), 3) - Math.Pow(PlayerPrefs.GetInt("Level", 1), 3));






    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateAttributes()
    {
        int[] stats = sn.getPlayerStatistics(1);
        //Debug.Log("STATS " + stats[0] + " " + stats[1] + " " + stats[2] +  " " + stats[3] + "");
        //Debug.Log("PSTATS " + Manager.Instance.pStats[0] + " " + Manager.Instance.pStats[1] + " " + Manager.Instance.pStats[2] + " " + Manager.Instance.pStats[3] + "");
        Memory.value = stats[0];
        MemoryR.value = stats[0];
        Accuracy.value = stats[1];
        AccuracyR.value = stats[1];
        Speed.value = stats[2];
        SpeedR.value = stats[2];
        Response.value = stats[3];
        ResponseR.value = stats[3];

        MemoryT.text = stats[0] + "";
        MemoryRT.text = "" + (stats[0] - Manager.Instance.pStats[0]);
        if (stats[0] - Manager.Instance.pStats[0] >= 0)
            MemoryRT.text = "+" + MemoryRT.text;
        AccuracyT.text = "" + stats[1];
        AccuracyRT.text = "" + (stats[1] - Manager.Instance.pStats[1]);
        if (stats[1] - Manager.Instance.pStats[1] >= 0)
            AccuracyRT.text = "+" + AccuracyRT.text;
        SpeedT.text = "" + stats[2];
        SpeedRT.text = "" + (stats[2] - Manager.Instance.pStats[2]);
        if (stats[2] - Manager.Instance.pStats[2] >= 0)
            SpeedRT.text = "+" + SpeedRT.text;
        ResponseT.text = "" + stats[3];
        ResponseRT.text = "" + (stats[3] - Manager.Instance.pStats[3]);
        if (stats[3] - Manager.Instance.pStats[3] >= 0)
            ResponseRT.text = "+" + ResponseRT.text;

        if ((stats[1] * 1.5) >= stats[2])
        {
            RoboKuma.GetComponent<Animator>().SetBool("isFidgety", false);
        }
        else
        {
            petStatus.text = "FIDGETY";
        }

        if (stats[0] >= 5)
        {
            isForgetful = false;
        }
        else
        {
            isForgetful = true;
            petStatus.text = "FORGETFUL";
        }

        if(!RoboKuma.GetComponent<Animator>().GetBool("isFidgety") && !isForgetful)
        {
            petStatus.text = "NORMAL";
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

        updateAttributes();
    }

	public void minigamePress()
	{
		/*PetScreen.gameObject.SetActive(true);
		MinigameScreen.gameObject.SetActive (false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);*/
        MinigameScreen.gameObject.SetActive(!MinigameScreen.gameObject.activeSelf);
        ResultsPanel.gameObject.SetActive(false);
    }

    public void achievementPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(true);
        ResultsPanel.gameObject.SetActive(false);
    }

    public void statisticsPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(true);
        ResultsPanel.gameObject.SetActive(false);
    }

    public void customizationPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(true);
        ResultsPanel.gameObject.SetActive(false);

    }

    public void resultsPress()
    {
        StartCoroutine(dosomething());
    }

    public IEnumerator dosomething()
    {
        yield return new WaitForSecondsRealtime(0.5F);
        ResultsPanel.gameObject.SetActive(false);
    }

    public void gonogoPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene(1);
    }

    public void nbackPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene(2);
    }

    public void corsiPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene(3);
    }

    public void eriksenPress()
    {
        Manager.Instance.pStats = sn.getPlayerStatistics(1);
        SceneManager.LoadScene(4);
    }

    public void jump()
    {
        
        if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping() && isForgetful)
        {
            StartCoroutine(forgetfulJump());
        }
        else if (RoboKuma.GetComponent<Rigidbody2D>().IsSleeping())
        {
            RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
            RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 15000);
        }


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

        RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 15000);

    }

}
