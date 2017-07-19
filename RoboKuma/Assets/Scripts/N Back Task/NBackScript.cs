using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NBackScript : MonoBehaviour {

    /*public Sprite[] sprites;
	public Image mainImage;
	public Text feedbackText;
	public ArrayList gameSprites;
	public Button yesButton;
	public Button noButton;
	public bool isYes;
	public Image twoImageAgo;
	public int n = 2;
	public int count = 10;
	*/

    public GameObject[] objects;
    public Text endText, helpText, scoreText, display;
    public Text coinsTxt, expTxt;
    public GameObject end;
    public Text trialsTxt;

    public double[] time;
    public int trialCount = 10;
    public int nValue = 2;

    public Text counter;
    public GameObject instructionScreen;

    public AudioSource audioHandler;
    public AudioClip soundCorrect;
    public AudioClip soundIncorrect;
    public AudioClip soundSuccess;
    public AudioClip soundTimer;

    public int log_id { get; set; }

	SQLiteDatabase sn;
    // Use this for initialization

    void Start () {
        //gameSprites = new ArrayList();
        end.gameObject.SetActive(false);

		time = new double[trialCount];
		sn = new SQLiteDatabase();

		//player_id, log_id, time_start, time_end, prev_status, new_status, game_progress, is_updated

        NBackData[] lastNBack = sn.getLastNBack();

        int sum = 0;

        /*for(int i = 0; i < lastNBack.Length; i++)
        {
            if(lastNBack[i] == null)
            {
                PlayerPrefs.SetInt("NBack_Difficulty", 0);
                break;
            }
            else if(lastNBack[0].n_count != lastNBack[i].n_count)
            {
                break;
            }
            else
            {
                sum += lastNBack[i].correct_count / lastNBack[i].trial_count;
            }
        }

        if(sum == 3)
        {
            PlayerPrefs.SetInt("NBack_Difficulty", PlayerPrefs.GetInt("NBack_Difficulty", 0) + 1);
        }

        nValue = 2 + PlayerPrefs.GetInt("NBack_Difficulty", 0);*/

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
        StartCoroutine(startCoroutine());
    }

    public IEnumerator startCoroutine()
    {
        audioHandler.clip = soundTimer;
        audioHandler.Play();
        counter.text = "3";
        yield return new WaitForSecondsRealtime(1F);
        counter.text = "2";
        audioHandler.clip = soundTimer;
        audioHandler.Play();
        yield return new WaitForSecondsRealtime(1F);
        counter.text = "1";
        audioHandler.clip = soundTimer;
        audioHandler.Play();
        yield return new WaitForSecondsRealtime(1F);
        instructionScreen.SetActive(false);


        string currentTime = System.DateTime.Now.ToString("g");
		log_id = sn.insertintoPlayerLog(SQLiteDatabase.getPlayer().player_id, 3, currentTime, "null", PlayerPrefs.GetString("status"), "null", "Started", 0);
        PlayerPrefs.SetInt("log_id", log_id);

        int item0 = Random.Range(0, objects.Length);
        GameObject obj = (GameObject)Instantiate(objects[item0], new Vector3(0, 356, 0), Quaternion.identity);
        obj.transform.SetParent(this.transform, false);

        ItemScript itemScript = obj.GetComponent<ItemScript>();
        //itemScript.item0 = item0;
        //itemScript.item1 = -1;
        //itemScript.item2 = -1;
        itemScript.items = new int[nValue + 1];
        itemScript.items[0] = item0;

        for (int i = 1; i <= nValue; i++)
        {
            itemScript.items[i] = -1;
        }

        itemScript.score = 0;
        itemScript.count = 1;
        itemScript.endTxt = endText;
        itemScript.helpTxt = helpText;
        itemScript.display = display;
        itemScript.coinsTxt = coinsTxt;
        itemScript.expTxt = expTxt;
        itemScript.trialsTxt = trialsTxt;
        itemScript.end = end;
        itemScript.time = time;
        itemScript.scoreTxt = scoreText;
        itemScript.audioHandler = audioHandler;

        itemScript.trialCount = trialCount;
        itemScript.nValue = nValue;
    }
}
