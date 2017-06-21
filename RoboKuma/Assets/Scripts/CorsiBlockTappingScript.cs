﻿using UnityEngine;
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

    public GameObject End;

    public int[] sequence;
    //public int[] spriteSequence;
    public int sequenceLength = 4;
    public bool start = true;
    public int clickSequence;
    public int score = 0;

	public int log_id { get; set; }
	SQLiteDatabase sn;
	void Start ()
    {
        //spriteSequence = new int[sequenceLength];
        endTxt.text = "";

		End.gameObject.SetActive(false);
		sn = new SQLiteDatabase();

		string currentTime = System.DateTime.Now + "";
		//player_id, log_id, time_start, time_end, prev_status, new_status, game_progress, is_updated
		log_id = sn.insertintoPlayerLog (PlayerPrefs.GetInt("player_id"), 2, currentTime, "null", PlayerPrefs.GetString("status"), "null", "Started", 0);
		PlayerPrefs.SetInt ("log_id", log_id);
    }
	
	void Update ()
    {
	
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
        for (int i = 0; i < 4; i++)
        {
            sequence[i] = Random.Range(0, buttons.Length);
        }

        StartCoroutine(displaySequence());
    }

    public IEnumerator displaySequence()
    {

        yield return new WaitForSecondsRealtime(0.5F);
        int rand;
        for (int i = 0; i < sequence.Length; i++)
        {
            rand = Random.Range(0, sequenceSprites.Length);
            buttons[sequence[i]].image.overrideSprite = sequenceSprites[rand];
            //spriteSequence[i] = rand;
            yield return new WaitForSecondsRealtime(1);
            buttons[sequence[i]].image.overrideSprite = null;
            yield return new WaitForSecondsRealtime(1);
        }
        start = true;
        display.text = "TAP IN THE SAME ORDER";
        itemTxt.text = "Item 1 of 4";
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
                display.text = "GREAT!";
                score++;

                StartCoroutine(showCorrect(btn));
            }
            else
            {
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
                itemTxt.text = "Item " + (clickSequence + 1) + " of 4";
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

        int exp = (int)(score / 4.0 * 100);
        int coins = (int)(score / 4.0 * 500);

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

		sn.insertintoCorsi(PlayerPrefs.GetInt("player_id"),PlayerPrefs.GetInt("log_id"), correct_trials, score, sequenceLength, 1);

		sn.updatePlayerLog (PlayerPrefs.GetInt ("log_id"), System.DateTime.Now + "", PlayerPrefs.GetString ("status"), "FINISHED");
        sn.getPlayerStatistics(1);

        PlayerPrefs.SetInt("Experience", exp);
        PlayerPrefs.SetInt("Bearya", coins);

    }
}
