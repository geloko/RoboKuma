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

    public GameObject End;

    public int[] sequence;
    //public int[] spriteSequence;
    public int sequenceLength = 4;
    public bool start = true;
    public int clickSequence;
    public int score = 0;

	void Start ()
    {
        //spriteSequence = new int[sequenceLength];
        endTxt.text = "";

        End.gameObject.SetActive(false);

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


        SQLiteDatabase sn = new SQLiteDatabase();
        sn.insertinto("corsiblocktapping", 1, score, 4, 0.01);
        sn.getPlayerStatistics(1);

        PlayerPrefs.SetInt("Experience", exp);
        PlayerPrefs.SetInt("Bearya", coins);

    }
}
