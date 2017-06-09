﻿using System.Collections;
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
    public double[] time, timeCongruent, timeIncongruent;
	public double timeCongruentAve = 0, timeIncongruentAve = 0;
	public int timeCongruentCtr = 0, timeIncongruentCtr = 0;
    public Stopwatch stopwatch;
	public Text endTxt, scoreTxt;
    public Text coinsTxt, expTxt;
	public int x, y;
	public int iteration;
	public Text feedbackText;
	public int[] nMiddleRow, nXPattern, nInnerBoxPattern, nOuterBoxPattern;
	public int congruentCount = 0, incongruentCount = 0, correctCongruent = 0, correctIncongruent = 0;
    public bool inGame;
	public bool isCongruent;
	public int log_id { get; set; }
	SQLiteDatabase sn;
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
        time = new double[10];
		timeCongruent = new double[10];
		timeIncongruent = new double[10];
		//feedbackText = feedbackText.GetComponent<Text> ();
		//endTxt = endTxt.GetComponent<Text> ();
		//End = GameObject.Find("End");
		//mainPanel = GameObject.Find ("MainPanel");
		End.gameObject.SetActive(false);
		stopwatch = new Stopwatch();

        inGame = true;

		sn = new SQLiteDatabase();
		string currentTime = System.DateTime.Now + "";
		//player_id, log_id, time_start, time_end, prev_status, new_status, game_progress, is_updated
		log_id = sn.insertintoPlayerLog (PlayerPrefs.GetInt("player_id"), 4, currentTime, "null", PlayerPrefs.GetString("status"), "null", "Started", 0);
		PlayerPrefs.SetInt ("log_id", log_id);
	}

    // Update is called once per frame
    void Update () {
        if(inGame)
		    SwipeForComputer();
		//SwipeForMobile ();
	}

    public IEnumerator startDelay()
    {
        feedbackText.text = "READY";
        yield return new WaitForSecondsRealtime(1);
        feedbackText.text = "GO!";
        //int rand = Random.Range(1, 2);
        yield return new WaitForSecondsRealtime(1);
        feedbackText.text = "";

        iteration++;
        x = Random.Range(0, 100);

        if (x < 50)
        {
            images[0].sprite = sprites[0];
        }
        else
        {
            images[0].sprite = sprites[1];
        }

        for (int i = 1; i < images.Length; i++)
        {
            images[i].enabled = false;
        }

        y = Random.Range(0, 100);
        var pattern = getRandomPattern();

        for (int i = 0; i < pattern.Length; i++)
        {
            images[pattern[i]].enabled = true;

            if (y < 50)
            {
                images[pattern[i]].sprite = sprites[0];
            }
            else
            {
                images[pattern[i]].sprite = sprites[1];
            }
        }

		if (y < 50 && x < 50 || y > 50 && x > 50) {
			congruentCount++;
			isCongruent = true;
			UnityEngine.Debug.Log ("ERIKSEN TEST IS CONGRUENT");
		} else {
			incongruentCount++;
			isCongruent = false;
			UnityEngine.Debug.Log ("ERIKSEN TEST IS INCONGRUENT");
		}
        stopwatch.Reset();
        stopwatch.Start();
    }

    public void startGame(){
		
        if(iteration == 0)
        {
            StartCoroutine(startDelay());
        }
		else if (iteration != 10)
        {
            iteration++;
			x = Random.Range (0, 100);

			if (x < 50)
            {
				images [0].sprite = sprites [0];
			}
            else
            {
				images [0].sprite = sprites [1];
			}

            for (int i = 1; i < images.Length; i++)
            {
				images [i].enabled = false;
			}

			y = Random.Range (0, 100);
			var pattern = getRandomPattern ();

			for (int i = 0; i < pattern.Length; i++)
            {
				images [pattern [i]].enabled = true;
				if (y < 50)
                {
					images [pattern [i]].sprite = sprites [0];
				}
                else
                {
					images [pattern [i]].sprite = sprites [1];
				}
			}

			if (y < 50 && x < 50 || y > 50 && x > 50) {
				congruentCount++;
				isCongruent = true;
				UnityEngine.Debug.Log ("ERIKSEN TEST IS CONGRUENT");
			} else {
				incongruentCount++;
				isCongruent = false;
				UnityEngine.Debug.Log ("ERIKSEN TEST IS INCONGRUENT");
			}
			stopwatch.Reset ();
			stopwatch.Start ();
		} else
        {
            inGame = false;
			mainPanel.gameObject.SetActive(false);

            int exp = (int)(score / 10.0 * 100);
            int coins = (int)(score / 10.0 * 500);

            End.gameObject.SetActive(true);
            endTxt.text = score + "";
            coinsTxt.text = coins + "";
            expTxt.text = exp + "";
            stopwatch.Stop();

            double ave = 0;
            int cnt = 0;
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] != -1)
                {
                    ave += time[i];
                    cnt++;
                }
            }

			for (int i = 0; i < timeCongruent.Length; i++) {
				timeCongruentAve += timeCongruent [i];
			}
			for (int i = 0; i < timeIncongruent.Length; i++) {
				timeIncongruentAve += timeIncongruent [i];
			}
			timeCongruentAve = timeCongruentAve / timeCongruentCtr;
			timeIncongruentAve = timeIncongruentAve / timeIncongruentCtr;
            if (cnt != 0)
                ave = ave/cnt/1000;
            else
                ave = 0;

            PlayerPrefs.SetInt("Experience", exp);
            PlayerPrefs.SetInt("Bearya", coins);

			sn.insertintoEriksen (PlayerPrefs.GetInt("player_id"),PlayerPrefs.GetInt("log_id"),correctCongruent, timeCongruentAve, correctIncongruent, timeIncongruentAve, congruentCount, 10);

			sn.updatePlayerLog (PlayerPrefs.GetInt ("log_id"), System.DateTime.Now + "", PlayerPrefs.GetString ("status"), "FINISHED");
            endTxt.text = score + "";
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
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[iteration - 1] = timeElapsed;
                    feedbackText.text = "CORRECT " + "TIME: " + timeElapsed  + "ms";
					score++;
                    scoreTxt.text = "" + score;
					if (isCongruent) {
						correctCongruent++;
						timeCongruent [timeCongruentCtr] = timeElapsed;
						timeCongruentCtr++;
					} else {
						correctIncongruent++;
						timeIncongruent [timeIncongruentCtr] = timeElapsed;
						timeIncongruentCtr++;
					}
				}
                else
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[iteration - 1] = timeElapsed;
                    feedbackText.text = "WRONG";
				}
                StartCoroutine(gameDelay());
			}
			//swipe right
			if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
			{
				if (x >= 50)
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[iteration - 1] = timeElapsed;
                    feedbackText.text = "CORRECT " + "TIME: " + timeElapsed + "ms";
					score++;
                    scoreTxt.text = "" + score;
					if (isCongruent) {
						correctCongruent++;
						timeCongruent [timeCongruentCtr] = timeElapsed;
						timeCongruentCtr++;
					} else {
						correctIncongruent++;
						timeIncongruent [timeIncongruentCtr] = timeElapsed;
						timeIncongruentCtr++;
					}
                } else
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[iteration - 1] = timeElapsed;
                    feedbackText.text = "WRONG";
                }
                StartCoroutine(gameDelay());
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
					if (x < 50)
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[iteration - 1] = timeElapsed;
                        feedbackText.text = "CORRECT " + "TIME: " + timeElapsed + "ms";
						score++;
						if (isCongruent) {
							correctCongruent++;
							timeCongruent [timeCongruentCtr] = timeElapsed;
							timeCongruentCtr++;
						} else {
							correctIncongruent++;
							timeIncongruent [timeIncongruentCtr] = timeElapsed;
							timeIncongruentCtr++;
						}
					}
                    else
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[iteration - 1] = timeElapsed;
                        feedbackText.text = "WRONG";
                    }
                    StartCoroutine(gameDelay());
                }
				//swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
				{
					if (x >= 50)
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[iteration - 1] = timeElapsed;
                        feedbackText.text = "CORRECT " + "TIME: " + timeElapsed + "ms";
						score++;
					}
                    else
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[iteration - 1] = timeElapsed;
                        feedbackText.text = "WRONG";
						if (isCongruent) {
							correctCongruent++;
							timeCongruent [timeCongruentCtr] = timeElapsed;
							timeCongruentCtr++;
						} else {
							correctIncongruent++;
							timeIncongruent [timeIncongruentCtr] = timeElapsed;
							timeIncongruentCtr++;
						}
                    }
                    StartCoroutine(gameDelay());
                }
			}
		}
	}

    public IEnumerator gameDelay()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }

        yield return new WaitForSecondsRealtime(0.2F);
        images[0].enabled = true;
        startGame();
    }

}
