using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReturnMainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void returntomain()
    {
        SceneManager.LoadScene(0);
    }
}
