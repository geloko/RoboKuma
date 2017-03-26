using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CorsiBlockTappingScript : MonoBehaviour {

    public Button[] buttons;
    public Sprite blankSprite, incorrectSprite;
    public Sprite[] sequenceSprites;
    public Text display;
    public Text endTxt;

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
	    buttons = this.GetComponentsInChildren<Button>();
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
        if(start)
        {
            bool correct = false;
            if (btn == sequence[clickSequence])
            {
                display.text = "GREAT!";
                //buttons[sequence[clickSequence]].image.overrideSprite = sequenceSprites[spriteSequence[clickSequence]];
                score++;
            }
            else
            {
                display.text = "INCORRECT!";
            }

            clickSequence++;

            if(clickSequence >= sequenceLength)
            {
                End.gameObject.SetActive(true);
                endTxt.text = "YOU GOT " + score + " OUT OF 4\n\nTAP ANYWHERE TO CONTINUE";
            }
        }
    }
}
