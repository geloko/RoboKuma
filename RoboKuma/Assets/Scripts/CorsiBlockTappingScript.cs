using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CorsiBlockTappingScript : MonoBehaviour {

    public Button[] buttons;
    public Sprite blankSprite, incorrectSprite, correctSprite;
    public Sprite[] sequenceSprites;
    public Text display;
    public Text endTxt;
    public Text coinsTxt;
    public Text expTxt;
    public Text itemTxt;
    public Text EndMessage;

    public GameObject End;

    public int[] sequence;
    //public int[] spriteSequence;
    public int sequenceLength;
    public bool start = true;
    public int clickSequence;
    public int score = 0;

    public Text counter;
    public GameObject instructionScreen;

    //for SFX
    public AudioSource audioHandler;
    public AudioClip soundCorrect;
    public AudioClip soundIncorrect;
    public AudioClip soundSuccess;
    public AudioClip soundPop;
    public AudioClip soundTimer;
    

    public int log_id { get; set; }
	SQLiteDatabase sn;
	void Start ()
    {
        //spriteSequence = new int[sequenceLength];
        endTxt.text = "";

		End.gameObject.SetActive(false);
		sn = new SQLiteDatabase();
        
        CorsiData[] lastCorsiData = sn.getLastCorsi();

        int sum = 0;
        bool isComplete = true;
        for(int i = 0; i < lastCorsiData.Length; i++)
        {
            if(lastCorsiData[i] == null)
            {
                PlayerPrefs.SetInt("Corsi_Difficulty", 0);
                isComplete = false;
                break;
            }
            else if(lastCorsiData[0].seq_length != lastCorsiData[i].seq_length)
            {
                isComplete = false;
                break;
            }
            else
            {
                sum += lastCorsiData[i].correct_length / lastCorsiData[i].seq_length;
            }
        }

        if(sum == 3 && lastCorsiData[0].seq_length >= 4 + PlayerPrefs.GetInt("Corsi_Difficulty", 0))
        {
            PlayerPrefs.SetInt("Corsi_Difficulty", PlayerPrefs.GetInt("Corsi_Difficulty", 0) + 1);
        }
        else if(PlayerPrefs.GetInt("Corsi_Difficulty", 0) >= 1 && sum <= 1.5 && isComplete)
        {
            PlayerPrefs.SetInt("Corsi_Difficulty", PlayerPrefs.GetInt("Corsi_Difficulty", 0) - 1);
        }

        sequenceLength = 4 + PlayerPrefs.GetInt("Corsi_Difficulty", 0);
        itemTxt.text = "Item 0 of " + sequenceLength;


    }
	
	void Update ()
    {
	
	}

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ReturnMainScript.returnToMainWithNotif();
    }

    public void startGame()
    {
        start = false;
        clickSequence = 0;

        /*for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.overrideSprite = null;
        }*/

        sequence = new int[sequenceLength];
        for (int i = 0; i < sequenceLength; i++)
        {
            sequence[i] = Random.Range(0, buttons.Length);
        }
       
        StartCoroutine(displaySequence());
    }

    public IEnumerator displaySequence()
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
        //player_id, log_id, time_start, time_end, prev_status, new_status, game_progress, is_updated
		log_id = sn.insertintoPlayerLog(SQLiteDatabase.getPlayer().player_id, 2, currentTime, "null", PlayerPrefs.GetString("status"), "null", "Started", 0);
        PlayerPrefs.SetInt("log_id", log_id);

        yield return new WaitForSecondsRealtime(0.5F);
        int rand;
        for (int i = 0; i < sequence.Length; i++)
        {
            itemTxt.text = "Item " + (i + 1) + " of " + sequenceLength;
            rand = Random.Range(0, sequenceSprites.Length);
            buttons[sequence[i]].image.overrideSprite = sequenceSprites[rand];
            //spriteSequence[i] = rand;
            yield return new WaitForSecondsRealtime(1);
            buttons[sequence[i]].image.overrideSprite = null;
            yield return new WaitForSecondsRealtime(1);
        }
        start = true;
        display.text = "TAP IN THE SAME ORDER";
        itemTxt.text = "Item 1 of " + sequenceLength;
    }

    public void ButtonClicked(int btn)
    {
        
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].image.overrideSprite = null;
        }

        if (start)
        {
            //bool correct = false;
            if (btn == sequence[clickSequence])
            {
                audioHandler.clip = soundCorrect;
                audioHandler.Play();
                display.text = "GREAT!";
                score++;

                StartCoroutine(showCorrect(btn));
            }
            else
            {
                audioHandler.clip = soundIncorrect;
                audioHandler.Play();
                display.text = "INCORRECT!";

                StartCoroutine(showIncorrect(btn));
            }

            clickSequence++;
            
            if(clickSequence >= sequenceLength)
            {
                StartCoroutine(showResults());
            }
            else
            {
                itemTxt.text = "Item " + (clickSequence + 1) + " of " + sequenceLength;
            }
        }
    }

    public IEnumerator showCorrect(int btn)
    {
        buttons[btn].image.overrideSprite = correctSprite;
        yield return new WaitForSecondsRealtime(0.5F);

        buttons[btn].image.overrideSprite = null;
    }

    public IEnumerator showIncorrect(int btn)
    {
        buttons[btn].image.overrideSprite = incorrectSprite;
        yield return new WaitForSecondsRealtime(0.5F);

        buttons[btn].image.overrideSprite = null;
    }

    public IEnumerator showResults()
    {

        int exp = (int)(score * 1.0 / sequenceLength * 10.0 * (sequenceLength/4.0));
        int coins = (int)(score * 1.0 / sequenceLength * 25.0 * (sequenceLength / 4.0));

       

        yield return new WaitForSecondsRealtime(0.5F);
        End.gameObject.SetActive(true);
        endTxt.text = score + "";
        coinsTxt.text = coins + "";
        expTxt.text = exp + "";

		int correct_trials;
		if (score == sequenceLength) {
			correct_trials = 1;
		} else {
			correct_trials = 0;
		}

        audioHandler.clip = soundSuccess;
        audioHandler.Play();

        EndMessage.text = "OUT OF " + sequenceLength + "\n\n\n\n\n\nTAP ANYWHERE TO CONTINUE";


		sn.insertintoCorsi(SQLiteDatabase.getPlayer().player_id,PlayerPrefs.GetInt("log_id"), correct_trials, score, sequenceLength, 1);
        PlayerPrefs.SetString("Last_Played", System.DateTime.Now.ToString("g"));

        sn.updatePlayerLog (PlayerPrefs.GetInt ("log_id"), System.DateTime.Now.ToString("g"), PlayerPrefs.GetString ("status"), "FINISHED");

        PlayerPrefs.SetInt("Experience", exp);
        PlayerPrefs.SetInt("Bearya", coins);

    }
}
