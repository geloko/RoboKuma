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
    public Text expTxt;
    public Text coinsTxt;

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
        StartCoroutine(startDelay());
    }

    public IEnumerator startDelay()
    {
        timeTxt.text = "READY";
        yield return new WaitForSecondsRealtime(1F);
        timeTxt.text = "GO!";
        int rand = Random.Range(1, 2);
        yield return new WaitForSecondsRealtime(rand);

        stopwatch.Start();
        spawnBear();
    }

    public void spawnBear()
    {

        iterations++;
        timeTxt.text = "";
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

            int exp = (int)(score / 10.0 * 100);
            int coins = (int)(score / 10.0 * 500);
            
            End.gameObject.SetActive(true);
            endTxt.text = score + "";
            coinsTxt.text = coins + "";
            expTxt.text = exp + "";

            SQLiteDatabase sn = new SQLiteDatabase();
            double ave = 0;
            for(int i = 0; i < time.Length; i++)
            {
                if(time[i] != -1)
                    ave += time[i];
            }
            ave /= (10 - noGoCnt);

            PlayerPrefs.SetInt("Experience", exp);
            PlayerPrefs.SetInt("Bearya", coins);


            sn.insertinto("gonogo", 1, score, 10, ave / 1000);
        }
        
    }

    public IEnumerator WaitSeconds(int i)
    {
        button.image.overrideSprite = null;
        button.gameObject.SetActive(false);

        if (i == 0)
            yield return new WaitForSecondsRealtime(0.5F);
        else
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
            scoreTxt.text = "" + score;
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
            scoreTxt.text = "" + score;

            double timeElapsed = stopwatch.ElapsedMilliseconds;
            time[iterations - 1] = timeElapsed;

            timeTxt.text = "GREAT!\n" + timeElapsed + " ms";
            
            StartCoroutine(WaitSeconds(Random.Range(0, 3)));

        }
        else
        {
            time[iterations - 1] = -1;

            timeTxt.text = "OOPS!";
            StopAllCoroutines();
            StartCoroutine(WaitSeconds(Random.Range(0, 3)));
        }


    }

}
