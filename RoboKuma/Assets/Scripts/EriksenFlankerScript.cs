using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class EriksenFlankerScript : MonoBehaviour {

	public Image[] images;
	public Sprite[] sprites;
	public Vector2 firstPressPos;
	public Vector2 secondPressPos;
	public Vector2 currentSwipe;
	public GameObject End, mainPanel;
	public int score;
	public Stopwatch stopwatch;
	public Text endTxt;
	public int x, y;
	public int iteration;
	public Text feedbackText;
	public int[] nMiddleRow, nXPattern, nInnerBoxPattern, nOuterBoxPattern;


	// Use this for initialization
	/*
	 * LEGEND:
	 * 1  2  3  4  5
	 * 6  7  8  9  10
	 * 11 12 0  13 14
	 * 15 16 17 18 19
	 * 20 21 22 23 24
	*/
	void Start () {
		iteration = 0;
		score = 0;
		nMiddleRow = new int[] {11,12,13,14};
		nXPattern = new int[]{ 7, 9, 16, 18 };
		nInnerBoxPattern = new int[]{ 7, 8, 9, 12, 13, 16, 17, 18 };
		nOuterBoxPattern = new int[]{ 1, 2, 3, 4, 5, 6, 10, 11, 14, 15, 19, 20, 21, 22, 23, 24 };

		feedbackText = feedbackText.GetComponent<Text> ();
		endTxt = endTxt.GetComponent<Text> ();
		End = GameObject.Find("End");
		mainPanel = GameObject.Find ("MainPanel");
		End.gameObject.SetActive(false);
		stopwatch = new Stopwatch();
	}
	
	// Update is called once per frame
	void Update () {
		SwipeForComputer();
		//SwipeForMobile ();
	}
	public void startGame(){
		
		if (iteration != 10) {

			iteration++;
			x = Random.Range (0, 100);
			if (x < 50) {
				images [0].sprite = sprites [0];
			} else {
				images [0].sprite = sprites [1];
			}
			for (int i = 1; i < images.Length; i++) {
				images [i].enabled = false;
			}
			y = Random.Range (0, 100);
			var pattern = getRandomPattern ();
			for (int i = 0; i < pattern.Length; i++) {
				images [pattern [i]].enabled = true;
				if (y >= 50) {
					images [pattern [i]].sprite = sprites [0];
				} else {
					images [pattern [i]].sprite = sprites [1];
				}
			}
			stopwatch.Reset ();
			stopwatch.Start ();
		} else {
			mainPanel.gameObject.SetActive(false);
			End.gameObject.SetActive(true);
			stopwatch.Stop();
			endTxt.text = "GOOD JOB!\n" + "YOU GOT " + score + " OUT OF 10\n\n\nTAP ANYWHERE TO CONTINUE";
		}
	}

	public int[] getRandomPattern(){
		int x;
		x = Random.Range (0, 5);
		switch (x) {
		case 0:
			return nMiddleRow;
		case 1:
			return nXPattern;
		case 2:
			return nInnerBoxPattern;
		case 3:
			return nOuterBoxPattern;
		case 4:
			var z = new int[nMiddleRow.Length + nInnerBoxPattern.Length];
			nMiddleRow.CopyTo (z, 0);
			nInnerBoxPattern.CopyTo(z, nMiddleRow.Length);
			return z;
		}
		return nMiddleRow;
	}


	public void SwipeForComputer()
	{
		if(Input.GetMouseButtonDown(0))
		{
			//save began touch 2d point
			firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		}
		if(Input.GetMouseButtonUp(0))
		{
			//save ended touch 2d point
			secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

			//create vector from the two points
			currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

			//normalize the 2d vector
			currentSwipe.Normalize();

			//swipe upwards
			if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
			{
			}
			//swipe down
			if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
			{
			}
			//swipe left
			if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			{
				if (x < 50) {
					feedbackText.text = "CORRECT " + "TIME: " + stopwatch.ElapsedMilliseconds + "ms";
					score++;
				} else {
					feedbackText.text = "WRONG";
				}
				startGame ();
			}
			//swipe right
			if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			{
				if (x >= 50) {
					feedbackText.text = "CORRECT " + "TIME: " + stopwatch.ElapsedMilliseconds + "ms";
					score++;
				} else {
					feedbackText.text = "WRONG";
				}
				startGame ();
			}
		}
	}

	public void SwipeForMobile()
	{
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				//save began touch 2d point
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				//save ended touch 2d point
				secondPressPos = new Vector2(t.position.x,t.position.y);

				//create vector from the two points
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize();

				//swipe upwards
				if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
				}
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
				}
				//swipe left
				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					if (x < 50) {
						feedbackText.text = "CORRECT " + "TIME: " + stopwatch.ElapsedMilliseconds + "ms";
						score++;
					} else {
						feedbackText.text = "WRONG";
					}
					startGame ();
				}
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					if (x >= 50) {
						feedbackText.text = "CORRECT " + "TIME: " + stopwatch.ElapsedMilliseconds + "ms";
						score++;
					} else {
						feedbackText.text = "WRONG";
					}
					startGame ();
				}
			}
		}
	}

}
