using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CorsiBlockTappingScript : MonoBehaviour {

    public Button[] buttons;
    public Sprite bearSprite, blankSprite;
    public Text display;

    public int[] sequence;
    public int sequenceLength = 4;
    public bool start = true;
    public int clickSequence;

	void Start ()
    {
	    buttons = this.GetComponentsInChildren<Button>();
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
            }
            else
            {
                display.text = "INCORRECT!";
            }

            clickSequence++;
        }
    }
}
