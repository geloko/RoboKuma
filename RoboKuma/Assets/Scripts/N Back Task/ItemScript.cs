﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class ItemScript : MonoBehaviour {

    //public float fallSpeed = 700.0f;
    //public float spinSpeed = 250.0f;

    public Vector2 firstPressPos;
    public Vector2 secondPressPos;
    public Vector2 currentSwipe;

    public Rigidbody2D rb;

    public Text display;
    public Text cheat;
    public Text scoreTxt;
    public Text endTxt;

    public GameObject end;
    
    public Transform panel;

    public GameObject[] objects = new GameObject[2];

    public int item0 = 0;
    public int item1 = -1;
    public int item2 = -1;

    public bool swiped = false;

    public int score = 0;
    public double[] time;

    public int count;
    public Stopwatch stopwatch;

    void Start()
    {
        panel = this.transform.parent;
        Transform child = panel.transform.Find("FeedbackText");
        display = child.GetComponent<Text>();

        child = panel.transform.Find("CheatSheet");
        cheat = child.GetComponent<Text>();

        child = panel.transform.Find("Score");
        scoreTxt = child.GetComponent<Text>();
        
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(-transform.up * 20000);

        if (item2 == 0)
        {
            cheat.text = "Wrench";
        }
        else if(item2 == 1)
        {
            cheat.text = "Water Bottle";
        }

        stopwatch = new Stopwatch();
        stopwatch.Start();


        UnityEngine.Debug.Log(count - 3 + "____" + count);

        if (count > 2)
        {
            time[count - 3] = -1;
        }


    }


    void Update()
    {
        panel = this.transform.parent;
        //transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        //transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
        SwipeForComputer();

    }

    public void createObject()
    {
        if(count <= 11)
        {
            item2 = item1;
            item1 = item0;
            item0 = Random.Range(0, objects.Length);
            GameObject obj = (GameObject)Instantiate(objects[item0], new Vector3(0, 356, 0), Quaternion.identity);
            obj.transform.SetParent(panel.transform, false);

            ItemScript itemScript = obj.GetComponent<ItemScript>();
            itemScript.item0 = item0;
            itemScript.item1 = item1;
            itemScript.item2 = item2;
            itemScript.score = score;
            itemScript.count = count + 1;
            itemScript.endTxt = endTxt;
            itemScript.end = end;
            itemScript.time = time;

            rb = GetComponent<Rigidbody2D>();

            rb.AddForce(-transform.up * 20000);

            if (item2 == 0)
            {
                cheat.text = "Wrench";
            }
            else if (item2 == 1)
            {
                cheat.text = "Water Bottle";
            }

        }
        else
        {
            for(int i = 0; i < 10; i++)
            {
                UnityEngine.Debug.Log("Time: " + time[i]);
            }
            end.gameObject.SetActive(true);
            endTxt.text = "You got " + score + " out of 10\n" + "Tap anywhere to CONTINUE";
            panel.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D()
    {
        if (item2 != -1 && gameObject.Equals(objects[item2]) && !swiped)
        {
            display.text = "Missed";
        }
        else if(swiped)
        {
            swiped = false;
        }
        else if(count > 2)
        {
            //double timeElapsed = stopwatch.ElapsedMilliseconds;
            //time[count - 3] = timeElapsed;
            //UnityEngine.Debug.Log(timeElapsed + "----" + (count - 3));

            display.text = "Correct";
            score++;
            scoreTxt.text = "Score:" + score;
        }
        else
        {
            display.text = "";

            

        }
        createObject();
        Destroy(gameObject);
    }

    public void SwipeForComputer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                UnityEngine.Debug.Log("up swipe");
            }
            //swipe down
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                UnityEngine.Debug.Log("down swipe");
            }
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (item2 != -1 && gameObject.Equals(objects[item2]))
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - 3] = timeElapsed;
                    UnityEngine.Debug.Log(timeElapsed + "----" + (count - 3));

                    display.text = "Correct";
                    score++;
                    scoreTxt.text = "Score:" + score;
                }
                else if (item2 != -1)
                {
                    display.text = "Wrong";
                }
                //createObject();

                if(count > 2)
                {
                    rb.AddForce(-transform.right * 500000);
                    swiped = true;
                }
                    
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (item2 != -1 && gameObject.Equals(objects[item2]))
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - 3] = timeElapsed;
                    UnityEngine.Debug.Log(timeElapsed + "----" + (count - 3));

                    display.text = "Correct";
                    score++;
                    scoreTxt.text = "Score:" + score;
                }
                else if (item2 != -1)
                {
                    display.text = "Wrong";
                }
                //createObject();
                if (count > 2)
                {
                    rb.AddForce(transform.right * 500000);
                    swiped = true;
                }
            }
        }
    }

    public void SwipeForMobile()
    {
        if (Input.touches.Length > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //normalize the 2d vector
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (item2 != -1 && gameObject.Equals(objects[item2]))
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - 3] = timeElapsed;
                        UnityEngine.Debug.Log(timeElapsed + "----" + (count - 3));

                        display.text = "Correct";
                        score++;
                        scoreTxt.text = "Score:" + score;
                    }
                    else if (item2 != -1)
                    {
                        display.text = "Wrong";
                    }
                    //createObject();

                    if (count > 2)
                    {
                        rb.AddForce(-transform.right * 500000);
                        swiped = true;
                    }
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (item2 != -1 && gameObject.Equals(objects[item2]))
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - 3] = timeElapsed;
                        UnityEngine.Debug.Log(timeElapsed + "----" + (count - 3));

                        display.text = "Correct";
                        score++;
                        scoreTxt.text = "Score:" + score;
                    }
                    else if (item2 != -1)
                    {
                        display.text = "Wrong";
                    }
                    //createObject();
                    if (count > 2)
                    {
                        rb.AddForce(transform.right * 500000);
                        swiped = true;
                    }
                }
            }
        }
    }
}
