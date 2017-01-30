using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CorsiBlockTappingScript : MonoBehaviour {

    public Button[] buttons;
    public Sprite bearSprite, blankSprite;
    public Text display;
    public Text endTxt;

    public GameObject End;

    public int[] sequence;
    public int sequenceLength = 4;
    public bool start = true;
    public int clickSequence;
    public int score = 0;

	void Start ()
    {
	    buttons = this.GetComponentsInChildren<Button>();
        endTxt.text = "";

    }
	
	void Update ()
    {
	
	}

    public void startGame()
    {
        start = false;
        clickSequence = 0;

        sequence = new int[4];
        for (int i = 0; i < 4; i++)
        {
            sequence[i] = Random.Range(0, buttons.Length);
        }

        StartCoroutine(displaySequence());
    }

    public IEnumerator displaySequence()
    {
        for (int i = 0; i < sequence.Length; i++)
        {
            buttons[sequence[i]].image.overrideSprite = bearSprite;
            yield return new WaitForSecondsRealtime(1);
            buttons[sequence[i]].image.overrideSprite = blankSprite;
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
                score++;
            }
            else
            {
                display.text = "INCORRECT!";
            }

            clickSequence++;

            if(clickSequence >= 4)
            {
                this.gameObject.SetActive(false);
                End.gameObject.SetActive(true);
                endTxt.text = "GOOD JOB!\n" + "YOU GOT " + score + " OUT OF 4\n\n\nTAP ANYWHERE TO CONTINUE";
            }
        }
    }
}
