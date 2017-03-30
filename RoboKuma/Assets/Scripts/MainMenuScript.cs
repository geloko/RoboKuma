using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	public Canvas MainScreen;
    public GameObject PetScreen;
	public GameObject MinigameScreen;
    public GameObject CustomizationScreen;
    public GameObject AchievementScreen;
    public GameObject AttributeScreen;
    public GameObject ResultsPanel;
    public GameObject RoboKuma;

    public Slider Memory, Response, Speed, Accuracy, MemoryR, ResponseR, SpeedR, AccuracyR;
    public Text MemoryT, ResponseT, SpeedT, AccuracyT, MemoryRT, ResponseRT, SpeedRT, AccuracyRT;

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

        sn = new SQLiteDatabase();
        updateAttributes();

        if (Manager.Instance.score == -1)
            ResultsPanel.gameObject.SetActive(false);
        else
        {
            ResultsPanel.gameObject.SetActive(true);
        }


        
        //PlayerPrefs.DeleteKey("DB");

        if (PlayerPrefs.GetInt("DB") != 1 )
        {
            sn.Start();
            PlayerPrefs.SetInt("DB", 1);
            Debug.Log("DB");
            updateAttributes();
        }
       

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateAttributes()
    {
        int[] stats = sn.getPlayerStatistics(1);
        Debug.Log("STATS " + stats[0] + " " + stats[1] + " " + stats[2] +  " " + stats[3] + "");
        Debug.Log("PSTATS " + Manager.Instance.pStats[0] + " " + Manager.Instance.pStats[1] + " " + Manager.Instance.pStats[2] + " " + Manager.Instance.pStats[3] + "");
        Memory.value = stats[0];
        MemoryR.value = stats[0];
        Accuracy.value = stats[1];
        AccuracyR.value = stats[1];
        Speed.value = stats[2];
        SpeedR.value = stats[2];
        Response.value = stats[3];
        ResponseR.value = stats[3];

        MemoryT.text = stats[0] + "";
        MemoryRT.text = "+" + (stats[0] - Manager.Instance.pStats[0]);
        AccuracyT.text = "" + stats[1];
        AccuracyRT.text = "+" + (stats[1] - Manager.Instance.pStats[1]);
        SpeedT.text = "" + stats[2];
        SpeedRT.text = "+" + (stats[2] - Manager.Instance.pStats[2]);
        ResponseT.text = "" + stats[3];
        ResponseRT.text = "+" + (stats[3] - Manager.Instance.pStats[3]);

        if(stats[2] >= 10)
        {
            RoboKuma.GetComponent<Animator>().SetBool("isFidgety", false);
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
	}

	public void minigamePress()
	{
		/*PetScreen.gameObject.SetActive(true);
		MinigameScreen.gameObject.SetActive (false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);*/
        MinigameScreen.gameObject.SetActive(!MinigameScreen.gameObject.activeSelf);
	}

    public void achievementPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(true);
    }

    public void statisticsPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(true);
    }

    public void customizationPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        AttributeScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(true);

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
        if(RoboKuma.GetComponent<Rigidbody2D>().IsSleeping())
        {
            RoboKuma.GetComponent<Rigidbody2D>().WakeUp();
            RoboKuma.GetComponent<Rigidbody2D>().AddForce(transform.up * 15000);
        }
        
    }

}
