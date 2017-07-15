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
    public Text itemTxt;

    public GameObject End;
    
    public int score = 0;
    public int noGoCnt = 0;
	public int goCnt = 0;
	public int noGoCorrect = 0;
	public int goCorrect = 0;
    public double[] time;
	public int log_id { get; set; }
    public bool isGo;
    public bool clicked;
    //public int[] noGoIndices;

    public Stopwatch stopwatch;
    public int iterations = 0;

	SQLiteDatabase sn;

    public int trialCount = 10;

    public Text counter;
    public GameObject instructionScreen;
    //for SFX
    public AudioSource audioHandler;
    public AudioClip soundCorrect;
    public AudioClip soundIncorrect;
    public AudioClip soundSuccess;

    // Use this for initialization
    void Start () {
        scoreTxt = scoreTxt.GetComponent<Text>();
        timeTxt = timeTxt.GetComponent<Text>();
        endTxt = endTxt.GetComponent<Text>();

        End = GameObject.Find("End");
        End.gameObject.SetActive(false);

        time = new double[trialCount];

        stopwatch = new Stopwatch();
		sn = new SQLiteDatabase();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ReturnMainScript.returnToMainWithNotif();
    }

    public void startGame()
    {
        StartCoroutine(startDelay());
    }

    public IEnumerator startDelay()
    {
        counter.text = "3";
        yield return new WaitForSecondsRealtime(1F);
        counter.text = "2";
        yield return new WaitForSecondsRealtime(1F);
        counter.text = "1";
        yield return new WaitForSecondsRealtime(1F);
        instructionScreen.SetActive(false);
        int rand = Random.Range(1, 2);
        yield return new WaitForSecondsRealtime(rand);

        string currentTime = System.DateTime.Now.ToString("g");
        //player_id, log_id, time_start, time_end, prev_status, new_status, game_progress, is_updated
        log_id = sn.insertintoPlayerLog(PlayerPrefs.GetInt("player_id"), 1, currentTime, "null", PlayerPrefs.GetString("status"), "null", "Started", 0);
        PlayerPrefs.SetInt("log_id", log_id);

        stopwatch.Start();
        spawnBear();
    }

    public void spawnBear()
    {

        iterations++;
        timeTxt.text = "";
        itemTxt.text = "Item " + iterations + " of " + trialCount;
        if(iterations <= trialCount)
        {
            button.image.overrideSprite = null;
            clicked = false;
            
            isGo = Random.value >= 0.4;
            if(isGo)
            {
                button.image.overrideSprite = goSprite;
                stopwatch.Reset();
                stopwatch.Start();
				goCnt++;
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
            itemTxt.text = "";

            audioHandler.clip = soundSuccess;
            audioHandler.Play();
            int exp = (int)((score * 1.0 / trialCount) * 10.0 * (trialCount / 10.0));
            int coins = (int)((score * 1.0 / trialCount) * 50.0 * (trialCount / 10.0));
            
            End.gameObject.SetActive(true);
            endTxt.text = score + "";
            coinsTxt.text = coins + "";
            expTxt.text = exp + "";

            double ave = 0;
            for(int i = 0; i < time.Length; i++)
            {
                if(time[i] != -1)
                    ave += time[i];
            }
            ave /= (trialCount - noGoCnt);

            PlayerPrefs.SetInt("Experience", exp);
            PlayerPrefs.SetInt("Bearya", coins);
            PlayerPrefs.SetString("Last_Played", System.DateTime.Now.ToString("g"));

            sn.insertintoGoNoGo (PlayerPrefs.GetInt("player_id"),PlayerPrefs.GetInt ("log_id"), goCorrect, noGoCorrect, ave, time, goCnt, trialCount);

			sn.updatePlayerLog (PlayerPrefs.GetInt ("log_id"), System.DateTime.Now.ToString("g"), PlayerPrefs.GetString ("status"), "FINISHED");
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
			noGoCorrect++;
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
            audioHandler.clip = soundCorrect;
            audioHandler.Play();
            score++;
			goCorrect++;
            scoreTxt.text = "" + score;

            double timeElapsed = stopwatch.ElapsedMilliseconds;
            time[iterations - 1] = timeElapsed;

            timeTxt.text = "GREAT!\n" + timeElapsed + " ms";
            
            StartCoroutine(WaitSeconds(Random.Range(0, 3)));

        }
        else
        {

            audioHandler.clip = soundIncorrect;
            audioHandler.Play();
            time[iterations - 1] = -1;

            timeTxt.text = "OOPS!";
            StopAllCoroutines();
            StartCoroutine(WaitSeconds(Random.Range(0, 3)));
        }


    }

}
