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

    public void returntomain()
    {
        Manager.Instance.score = 0;
        SceneManager.LoadScene(0);
    }
}
