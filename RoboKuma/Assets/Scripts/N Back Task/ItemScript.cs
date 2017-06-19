using System.Collections;
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
    public Text helpTxt, trialsTxt;
    public Text coinsTxt, expTxt;

    public GameObject end;
    
    public Transform panel;

    public GameObject[] objects = new GameObject[2];

    //public int item0 = 0;
    //public int item1 = -1;
    //public int item2 = -1;
    public int[] items;

    public bool swiped = false;

    public int score = 0;
    public double[] time;

    public int count;
    public Stopwatch stopwatch;

	SQLiteDatabase sn;

    void Start()
    {
        panel = this.transform.parent;
        Transform child;

        child = panel.transform.Find("CheatSheet");
        cheat = child.GetComponent<Text>();
        
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(-transform.up * 20000);

        if (items[2] == 0)
        {
            cheat.text = "Wrench";
        }
        else if(items[2] == 1)
        {
            cheat.text = "Water Bottle";
        }

        stopwatch = new Stopwatch();
        stopwatch.Start();

        if (count > 2)
        {
            time[count - 3] = -1;
        }

        if (count == 1)
            helpTxt.text = "Remember the items shown";
        else if (count == 2)
            helpTxt.text = "Remember the items shown";
        else if (count >= 3)
        {
            helpTxt.text = "";
            trialsTxt.text = "Item " + (count - 2) + " of 10";
        }
        sn = new SQLiteDatabase();
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
            items[2] = items[1];
            items[1] = items[0];
            items[0] = Random.Range(0, objects.Length);
            GameObject obj = (GameObject)Instantiate(objects[items[0]], new Vector3(0, 356, 0), Quaternion.identity);
            obj.transform.SetParent(panel.transform, false);

            ItemScript itemScript = obj.GetComponent<ItemScript>();
            itemScript.items = items;
            itemScript.score = score;
            itemScript.count = count + 1;
            itemScript.endTxt = endTxt;
            itemScript.helpTxt = helpTxt;
            itemScript.display = display;
            itemScript.coinsTxt = coinsTxt;
            itemScript.expTxt = expTxt;
            itemScript.trialsTxt = trialsTxt;
            itemScript.end = end;
            itemScript.time = time;
            itemScript.scoreTxt = scoreTxt;

            rb = GetComponent<Rigidbody2D>();

            rb.AddForce(-transform.up * 20000);

            if (items[2] == 0)
            {
                cheat.text = "Wrench";
            }
            else if (items[2] == 1)
            {
                cheat.text = "Water Bottle";
            }

            StopAllCoroutines();

        }
        else
        {
            for(int i = 0; i < 10; i++)
            {
                UnityEngine.Debug.Log("Time: " + time[i]);
            }


            int exp = (int)(score / 10.0 * 100);
            int coins = (int)(score / 10.0 * 500);

            end.gameObject.SetActive(true);
            endTxt.text = score + "";
            coinsTxt.text = coins + "";
            expTxt.text = exp + "";
            panel.gameObject.SetActive(false);

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
            ave /= cnt;

            PlayerPrefs.SetInt("Experience", exp);
            PlayerPrefs.SetInt("Bearya", coins);

			sn.insertintoNback(PlayerPrefs.GetInt("player_id"), PlayerPrefs.GetInt("log_id"), ave, score, 2, 2, 10);

			sn.updatePlayerLog (PlayerPrefs.GetInt ("log_id"), System.DateTime.Now + "", PlayerPrefs.GetString ("status"), "FINISHED");
        }
    }

    void OnTriggerEnter2D()
    {
        if (items[2] != -1 && gameObject.Equals(objects[items[2]]) && !swiped)
        {
            display.text = "Oops!";
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

            display.text = "GREAT!";
            score++;
            scoreTxt.text = "" + score;
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
                if (items[2] != -1 && gameObject.Equals(objects[items[2]]))
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - 3] = timeElapsed;

                    display.text = "GREAT!";
                    score++;
                    scoreTxt.text = "" + score;
                }
                else if (items[2] != -1)
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - 3] = timeElapsed;
                    display.text = "Oops!";
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
                if (items[2] != -1 && gameObject.Equals(objects[items[2]]))
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - 3] = timeElapsed;

                    display.text = "Great!";
                    score++;
                    scoreTxt.text = "" + score;
                }
                else if (items[2] != -1)
                {
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - 3] = timeElapsed;
                    display.text = "Oops!";
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
                    if (items[2] != -1 && gameObject.Equals(objects[items[2]]))
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - 3] = timeElapsed;

                        display.text = "Correct";
                        score++;
                        scoreTxt.text = "" + score;
                    }
                    else if (items[2] != -1)
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - 3] = timeElapsed;
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
                    if (items[2] != -1 && gameObject.Equals(objects[items[2]]))
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - 3] = timeElapsed;

                        display.text = "Correct";
                        score++;
                        scoreTxt.text = "" + score;
                    }
                    else if (items[2] != -1)
                    {
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - 3] = timeElapsed;
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
