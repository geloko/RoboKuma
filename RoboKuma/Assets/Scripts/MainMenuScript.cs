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
    public GameObject StatisticsScreen;


    // Use this for initialization
    void Start () {

		MainScreen = MainScreen.GetComponent<Canvas>();
        MinigameScreen = GameObject.Find ("MinigameScreen");
		PetScreen = GameObject.Find ("PetScreen");
        CustomizationScreen = GameObject.Find("CustomizationScreen");
        AchievementScreen = GameObject.Find("AchievementScreen");
        StatisticsScreen = GameObject.Find("StatisticsScreen");
        MinigameScreen.gameObject.SetActive (false);
        //CustomizationScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void mainPress()
	{

		PetScreen.gameObject.SetActive(false);
		MinigameScreen.gameObject.SetActive (false);
        AchievementScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(false);
        //CustomizationScreen.gameObject.SetActive(false);
        PetScreen.gameObject.SetActive(true);
	}

	public void minigamePress()
	{
		PetScreen.gameObject.SetActive(false);
		MinigameScreen.gameObject.SetActive (false);
        AchievementScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(false);
        //CustomizationScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(true);
	}

    public void achievementPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(false);
        //CustomizationScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(true);
    }

    public void statisticsPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(false);
        //CustomizationScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(true);
    }

    /*public void customizationPress()
    {
        PetScreen.gameObject.SetActive(false);
        MinigameScreen.gameObject.SetActive(false);
        AchievementScreen.gameObject.SetActive(false);
        StatisticsScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(false);
        CustomizationScreen.gameObject.SetActive(true);

    }*/

}
