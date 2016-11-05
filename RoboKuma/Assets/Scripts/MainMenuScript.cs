using UnityEngine;
using UnityEngine.UI;// we need this namespace in order to access UI elements within our script
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	public Canvas MainScreen;
	public Button MainMenuBtn;
	public Button MinigameBtn;
	public GameObject PetScreen;
	public GameObject MinigameScreen;
    public GameObject StatisticScreen;

    // Use this for initialization
    void Start () {

		MainScreen = MainScreen.GetComponent<Canvas>();
		MainMenuBtn = MainMenuBtn.GetComponent<Button>();
		MinigameBtn = MinigameBtn.GetComponent<Button>();
		MinigameScreen = GameObject.Find ("MinigameScreen");
		PetScreen = GameObject.Find ("PetScreen");
        StatisticScreen = GameObject.Find("StatisticScreen");
        MinigameScreen.gameObject.SetActive (false);
        StatisticScreen.gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void mainPress()
	{

		PetScreen.gameObject.SetActive(false);
		MinigameScreen.gameObject.SetActive (false);
        PetScreen.gameObject.SetActive(true);
	}

	public void minigamePress()
	{
		PetScreen.gameObject.SetActive(false);
		MinigameScreen.gameObject.SetActive (false);
		MinigameScreen.gameObject.SetActive(true);
	}
}
