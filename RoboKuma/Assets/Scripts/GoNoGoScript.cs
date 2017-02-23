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

    public int goIndex;
    public int score = 0;
    public int[] noGoIndices;

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
            goIndex = Random.Range(0, 5);
            int numNoGo = Random.Range(0, 3);
            for (int i = 0; i < 6; i++)
            {
                buttons[i].image.overrideSprite = null;
            }

            noGoIndices = new int[numNoGo];

            for(int i = 0; i < numNoGo; i++)
            {
                int temp = Random.Range(0, 5); 
                while(temp == goIndex)
                {
                    temp = Random.Range(0, 5);
                }

                noGoIndices[i] = temp;
                buttons[i].image.overrideSprite = noGoSprite;

            }

            buttons[goIndex].image.overrideSprite = goSprite;
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
        if(iClicked == goIndex)
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
