using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NBackScript : MonoBehaviour {

	public Sprite[] sprites;
	public Image mainImage;
	public Text feedbackText;
	public ArrayList gameSprites;
	public Button yesButton;
	public Button noButton;
	public bool isYes;
	public Image twoImageAgo;
	public int n = 2;
	public int count = 10;
	// Use this for initialization
	void Start () {
		gameSprites = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void startGame(){
		StartCoroutine(displayImage ());
	}

	public void startLoop(){


		if(count != 10){
			if (count != 0) {
				if (mainImage.sprite == gameSprites [gameSprites.Count - 3]) {
					if (isYes) {
						feedbackText.text = "CORRECT!";
					} else {
						feedbackText.text = "WRONG!";
					}
				} else {
					if (isYes) {
						feedbackText.text = "WRONG!";
					} else {
						feedbackText.text = "CORRECT!";
					}
				}
			} else {
				
			}
		}
		count--;
		int x = Random.Range (0, 3);
		mainImage.sprite = sprites [x];
		gameSprites.Add(mainImage.sprite);
		twoImageAgo.sprite = gameSprites [gameSprites.Count - 3] as Sprite;
	}

	public IEnumerator displayImage(){
		for (int i = 0; i < 2; i++) {
			int x = Random.Range (0, 3);
			mainImage.sprite = sprites [x];
			gameSprites.Add (mainImage.sprite);
			if(i == 0)
				twoImageAgo.sprite = sprites [x];
			yield return new WaitForSecondsRealtime (1);
		}

		startLoop ();
	}

	public void IsYesButton()
	{
		isYes = true;
	}

	public void IsNoButton()
	{
		isYes = false;
	}
}
