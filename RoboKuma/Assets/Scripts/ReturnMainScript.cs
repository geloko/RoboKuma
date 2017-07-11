using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.SimpleAndroidNotifications;
using System;

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
        StartCoroutine(delayedLoad());
    }

    public void returnMain()
    {
        SceneManager.LoadScene("Main ver 2");
    }

    public static void returnToMainWithNotif()
    {
        SceneManager.LoadScene("Main ver 2");
        //NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(1), "Test Notification", "Should appear when the app closes.", new Color(1, 0.3f, 0.15f), NotificationIcon.Message);
        NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(86400), "RoboKuma", "Robokuma misses you.", new Color(1, 0.3f, 0.15f), NotificationIcon.Message);
    }

    public IEnumerator delayedLoad()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene("Main ver 2");

    }
}
