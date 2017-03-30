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
        StartCoroutine(dosomething());
    }

    public void returnMain()
    {
        SceneManager.LoadScene(0);
    }

    public IEnumerator dosomething()
    {
        yield return new WaitForSecondsRealtime(0.5F);
        Manager.Instance.score = 0;
        SceneManager.LoadScene(0);
    }
}
