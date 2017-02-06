using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EriksenFlankerScript : MonoBehaviour {

	public Image[] images;
	public Sprite[] sprites;
	public Vector2 firstPressPos;
	public Vector2 secondPressPos;
	public Vector2 currentSwipe;
	public int x;
	public Text feedbackText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SwipeForComputer();
		//SwipeForMobile ();
	}

	public void startGame(){
		x = Random.Range (0, 100);
		if (x < 50) {
			images [4].sprite = sprites [0];
		} else {
			images [4].sprite = sprites [1];
		}
		for (int i = 0; i < images.Length; i++) {
			if (i != 4) {
				if (x >= 50) {
					images [i].sprite = sprites [0];
				} else {
				images [i].sprite = sprites [1];
				}
			}
		}

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
				Debug.Log("up swipe");
			}
			//swipe down
			if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
			{
				Debug.Log("down swipe");
			}
			//swipe left
			if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			{
				if (x < 50) {
					feedbackText.text = "CORRECT";
				} else {
					feedbackText.text = "WRONG";
				}
				startGame ();
			}
			//swipe right
			if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			{
				if (x >= 50) {
					feedbackText.text = "CORRECT";
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
					Debug.Log("up swipe");
				}
				//swipe down
				if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
				{
					Debug.Log("down swipe");
				}
				//swipe left
				if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					Debug.Log("left swipe");
				}
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					Debug.Log("right swipe");
				}
			}
		}
	}

}
