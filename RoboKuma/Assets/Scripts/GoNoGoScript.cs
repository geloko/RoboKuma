using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;

public class GoNoGoScript : MonoBehaviour {

    public Button button;
    public Sprite goSprite, noGoSprite;
    public Text scoreTxt;
    public Text timeTxt;
    public Text endTxt;

    public GameObject End;
    
    public int score = 0;
    public int noGoCnt = 0;
    public double[] time;

    public bool isGo;
    public bool clicked;
    //public int[] noGoIndices;

    public Stopwatch stopwatch;
    public int iterations = 0;

    // Use this for initialization
    void Start () {
        scoreTxt = scoreTxt.GetComponent<Text>();
        timeTxt = timeTxt.GetComponent<Text>();
        endTxt = endTxt.GetComponent<Text>();

        End = GameObject.Find("End");
        End.gameObject.SetActive(false);

        time = new double[10];

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
            button.image.overrideSprite = null;
            clicked = false;
            
            isGo = Random.value >= 0.4;
            if(isGo)
            {
                button.image.overrideSprite = goSprite;
                stopwatch.Reset();
                stopwatch.Start();
            }
            else
            {
                noGoCnt++;
                StartCoroutine(displayNoGo());
                
                time[iterations - 1] = -1;
            }
        }
        else
        {
            this.gameObject.SetActive(false);
            End.gameObject.SetActive(true);
            endTxt.text = "YOU GOT " + score + " OUT OF 10\n\nTAP TO CONTINUE";
			SQLiteDatabase sn = new SQLiteDatabase();
			sn.insertinto ("gonogo", 1 , score, 10, 0.01);
        }
        
    }

    public IEnumerator WaitSeconds(int i)
    {
        button.image.overrideSprite = null;
        button.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(i);
        button.gameObject.SetActive(true);
        spawnBear();
    }

    public IEnumerator displayNoGo()
    {
        button.image.overrideSprite = noGoSprite;
        yield return new WaitForSecondsRealtime(2);

        if(!clicked)
        {
            score++;
            scoreTxt.text = "Score: " + score;
            timeTxt.text = "GREAT!";
        
            button.image.overrideSprite = null;
            button.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(2);
            button.gameObject.SetActive(true);

            spawnBear();

        }
    }

    public void calculateScore(int iClicked)
    {
        clicked = true;
        if(isGo)
        {
            score++;
            scoreTxt.text = "Score: " + score;

            double timeElapsed = stopwatch.ElapsedMilliseconds;
            time[iterations - 1] = timeElapsed;

            timeTxt.text = "GREAT!\n" + timeElapsed + " ms";
            
            StartCoroutine(WaitSeconds(2));

        }
        else
        {
            time[iterations - 1] = -1;

            timeTxt.text = "OOPS!";
            StartCoroutine(WaitSeconds(2));
        }


    }

}
