using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnMainScript : MonoBehaviour {

    public GameObject resultsPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void returnResult()
    {
        PlayerPrefs.SetInt("Score", 1);
        SceneManager.LoadScene(0);
    }

    public void returnMain()
    {
        SceneManager.LoadScene(0);
    }
}
