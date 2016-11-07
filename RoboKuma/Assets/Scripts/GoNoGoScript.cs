using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class GoNoGoScript : MonoBehaviour {

    public Button[] buttons;
    public Sprite bearSprite;
    public Text scoreTxt;
    public Text timeTxt;
    public Text endTxt;

    public GameObject End;

    public int bearIndex;
    public int score = 0;
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
            bearIndex = Random.Range(0, 5);
            for (int i = 0; i < 6; i++)
            {
                buttons[i].image.overrideSprite = null;
            }

            buttons[bearIndex].image.overrideSprite = bearSprite;
            stopwatch.Reset();
            stopwatch.Start();
        }
        else
        {
            this.gameObject.SetActive(false);
            End.gameObject.SetActive(true);
            endTxt.text = "GOOD JOB!\n" + "YOU GOT " + score + " OUT OF 10\n\n\nTAP ANYWHERE TO CONTINUE";
        }
        
    }

    public void calculateScore(int iClicked)
    {
        if(iClicked == bearIndex)
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
