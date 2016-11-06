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
}
