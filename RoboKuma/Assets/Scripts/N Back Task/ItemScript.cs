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

    public int trialCount;
    public int nValue;

    public bool swiped = false;

    public int score = 0;
    public double[] time;

    public int count;
    public Stopwatch stopwatch;

	SQLiteDatabase sn;

    //for SFX
    public AudioSource audioHandler;
    public AudioClip soundCorrect;
    public AudioClip soundIncorrect;
    public AudioClip soundSuccess;

    void Start()
    {
        panel = this.transform.parent;
        Transform child;

        child = panel.transform.Find("CheatSheet");
        cheat = child.GetComponent<Text>();
        
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(-transform.up * 20000);

        stopwatch = new Stopwatch();
        stopwatch.Start();

        if (count > nValue)
        {
            time[count - nValue - 1] = -1;
        }

        if (count <= nValue)
            helpTxt.text = "Remember the items shown";
        else if (count > nValue)
        {
            helpTxt.text = "";
            trialsTxt.text = "Item " + (count - nValue) + " of " + trialCount;
        }
        sn = new SQLiteDatabase();

        /*switch (items[nValue])
        {
            case 0:
                cheat.text = "Wrench";
                break;
            case 1:
                cheat.text = "pliers";
                break;
            case 2:
                cheat.text = "pencil";
                break;
        }*/

    }


    void Update()
    {
        panel = this.transform.parent;
        //transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
        //transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
        SwipeForComputer();

    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            ReturnMainScript.returnToMainWithNotif();
    }

    public void createObject()
    {
        if(count <= trialCount + 1)
        {
            for(int i = nValue; i >= 1; i--)
            {
                items[i] = items[i - 1];
            }
            //items[2] = items[1];
            //items[1] = items[0];
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
            itemScript.audioHandler = audioHandler;

            itemScript.nValue = nValue;
            itemScript.trialCount = trialCount;

            rb = GetComponent<Rigidbody2D>();

            rb.AddForce(-transform.up * 20000);

            StopAllCoroutines();
            
        }
        else
        {
            for(int i = 0; i < 10; i++)
            {
                UnityEngine.Debug.Log("Time: " + time[i]);
            }


            int exp = (int)((score * 1.0 / trialCount) * 15.0 * (trialCount/10.0));
            int coins = (int)((score * 1.0 / trialCount) * 80.0  * (trialCount/10.0));

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

            audioHandler.clip = soundSuccess;
            audioHandler.Play();
            trialsTxt.text = "";

            PlayerPrefs.SetInt("Experience", exp);
            PlayerPrefs.SetInt("Bearya", coins);
            PlayerPrefs.SetString("Last_Played", System.DateTime.Now.ToString("g"));

            sn.insertintoNback(PlayerPrefs.GetInt("player_id"), PlayerPrefs.GetInt("log_id"), ave, score, nValue, 2, trialCount);

			sn.updatePlayerLog (PlayerPrefs.GetInt ("log_id"), System.DateTime.Now.ToString("g"), PlayerPrefs.GetString ("status"), "FINISHED");
        }
    }

    void OnTriggerEnter2D()
    {
        if (items[nValue] != -1 && gameObject.Equals(objects[items[nValue]]) && !swiped && count > nValue)
        {
            audioHandler.clip = soundIncorrect;
            audioHandler.Play();
            display.text = "Oops!";
        }
        else if(swiped)
        {
            swiped = false;
        }
        else if(count > nValue)
        {
            //double timeElapsed = stopwatch.ElapsedMilliseconds;
            //time[count - 3] = timeElapsed;
            //UnityEngine.Debug.Log(timeElapsed + "----" + (count - 3));
            audioHandler.clip = soundCorrect;
            audioHandler.Play();

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
                
                if (items[nValue] != -1 && gameObject.Equals(objects[items[nValue]]))
                {
                    audioHandler.clip = soundCorrect;
                    audioHandler.Play();
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - nValue - 1] = timeElapsed;

                    display.text = "GREAT!";
                    score++;
                    scoreTxt.text = "" + score;
                }
                else if (items[nValue] != -1)
                {
                    audioHandler.clip = soundIncorrect;
                    audioHandler.Play();
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - nValue - 1] = timeElapsed;
                    display.text = "Oops!";
                }
                //createObject();

                if(count > nValue)
                {
                    rb.AddForce(-transform.right * 500000);
                    swiped = true;
                }
                    
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (items[nValue] != -1 && gameObject.Equals(objects[items[nValue]]))
                {
                    audioHandler.clip = soundCorrect;
                    audioHandler.Play();
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - nValue - 1] = timeElapsed;

                    display.text = "Great!";
                    score++;
                    scoreTxt.text = "" + score;
                }
                else if (items[nValue] != -1)
                {
                    audioHandler.clip = soundIncorrect;
                    audioHandler.Play();
                    double timeElapsed = stopwatch.ElapsedMilliseconds;
                    time[count - nValue - 1] = timeElapsed;
                    display.text = "Oops!";
                }
                //createObject();
                if (count > nValue)
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
                    if (items[nValue] != -1 && gameObject.Equals(objects[items[nValue]]))
                    {
                        audioHandler.clip = soundCorrect;
                        audioHandler.Play();
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - nValue - 1] = timeElapsed;

                        display.text = "Correct";
                        score++;
                        scoreTxt.text = "" + score;
                    }
                    else if (items[nValue] != -1)
                    {
                        audioHandler.clip = soundIncorrect;
                        audioHandler.Play();
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - nValue - 1] = timeElapsed;
                        display.text = "Wrong";
                    }
                    //createObject();

                    if (count > nValue)
                    {
                        rb.AddForce(-transform.right * 500000);
                        swiped = true;
                    }
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (items[nValue] != -1 && gameObject.Equals(objects[items[nValue]]))
                    {
                        audioHandler.clip = soundCorrect;
                        audioHandler.Play();
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - nValue - 1] = timeElapsed;

                        display.text = "Correct";
                        score++;
                        scoreTxt.text = "" + score;
                    }
                    else if (items[nValue] != -1)
                    {
                        audioHandler.clip = soundIncorrect;
                        audioHandler.Play();    
                        double timeElapsed = stopwatch.ElapsedMilliseconds;
                        time[count - nValue - 1] = timeElapsed;
                        display.text = "Wrong";
                    }
                    //createObject();
                    if (count > nValue)
                    {
                        rb.AddForce(transform.right * 500000);
                        swiped = true;
                    }
                }
            }
        }
    }
}
