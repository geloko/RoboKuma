using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class CountdownScript : MonoBehaviour {

    public Text counter;
    public GameObject instructionScreen;
    public 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startCountDown()
    {
        StartCoroutine(countDown());
    }

    public IEnumerator countDown()
    {
        counter.text = "3";
        yield return new WaitForSecondsRealtime(1F);
        counter.text = "2";
        yield return new WaitForSecondsRealtime(1F);
        counter.text = "1";
        yield return new WaitForSecondsRealtime(1F);
        instructionScreen.SetActive(false);
    }

}
