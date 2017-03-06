using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class GoNoGoScript : MonoBehaviour {

    public Button[] buttons;
    public Sprite goSprite, noGoSprite;
    public Text scoreTxt;
    public Text timeTxt;
    public Text endTxt;

    public GameObject End;

    public int gngIndex;
    public int score = 0;

    public bool isGo;
    //public int[] noGoIndices;

    public Stopwatch stopwatch;
    public int iterations = 0;

    // Use this for initialization
    void Start () {
        buttons = this.GetComponentsInChildren<Button>();
        scoreTxt = scoreTxt.GetComponent<Text>();
        timeTxt = timeTxt.GetComponent<Text>();
        endTxt = endTxt.GetComponent<Text>();

        End = GameObject.Find("End");
        End.gameObject.SetActive(false);

        stopwatch = new Stopwatch();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void startGame()
    {
        stopwatch.Start();
        spawnBear();
    }

    public void spawnBear()
    {

        iterations++;

        if(iterations <= 10)
        {
            gngIndex = Random.Range(0, 5);
            for (int i = 0; i < 6; i++)
            {
                buttons[i].image.overrideSprite = null;
            }
            
            isGo = Random.value >= 0.5;
            if(isGo)
            {
                buttons[gngIndex].image.overrideSprite = goSprite;
                stopwatch.Reset();
                stopwatch.Start();
            }
            else
            {
                StartCoroutine(displayNoGo());
            }
        }
        else
        {
            this.gameObject.SetActive(false);
            End.gameObject.SetActive(true);
            endTxt.text = "GOOD JOB!\n" + "YOU GOT " + score + " OUT OF 10\n\n\nTAP ANYWHERE TO CONTINUE";
        }
        
    }

    public IEnumerator displayNoGo()
    {
        buttons[gngIndex].image.overrideSprite = noGoSprite;
        yield return new WaitForSecondsRealtime(1);

        score++;
        scoreTxt.text = "Score: " + score;
        timeTxt.text = "Great!";

        spawnBear();
    }

    public void calculateScore(int iClicked)
    {
        if(iClicked == gngIndex && isGo)
        {
            score++;
            scoreTxt.text = "Score: " + score;
            timeTxt.text = stopwatch.ElapsedMilliseconds + " ms";
        }
        else
        {

            timeTxt.text = "Incorrect!";
        }


    }

}
