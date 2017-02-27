using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MiniGameMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void gonogoPress()
    {
        SceneManager.LoadScene(1);
    }

    public void nbackPress()
    {
        SceneManager.LoadScene(2);
    }

    public void corsiPress()
    {
        SceneManager.LoadScene(3);
    }

    public void eriksenPress()
    {
        SceneManager.LoadScene(4);
    }
}
