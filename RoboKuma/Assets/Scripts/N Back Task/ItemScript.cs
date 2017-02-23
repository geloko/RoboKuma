using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    public Transform panel;

    public GameObject[] objects = new GameObject[2];

    public int item0 = 0;
    public int item1 = -1;
    public bool swiped = false;
    public int score = 0;


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

        rb.AddForce(-transform.up * 10000);

        if (item1 == 0)
        {
            cheat.text = "Lamp";
        }
        else if(item1 == 1)
        {
            cheat.text = "Cabinet";
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
        item1 = item0;
        item0 = Random.Range(0, objects.Length);
        GameObject obj = (GameObject)Instantiate(objects[item0], new Vector3(0, 356, 0), Quaternion.identity);
        obj.transform.SetParent(panel.transform, false);

        ItemScript itemScript = obj.GetComponent<ItemScript>();
        itemScript.item0 = item0;
        itemScript.item1 = item1;
        itemScript.score = score;

        if (item1 == 0)
        {
            cheat.text = "Lamp";
        }
        else if (item1 == 1)
        {
            cheat.text = "Cabinet";
        }
    }

    void OnTriggerEnter2D()
    {
        if (item1 != -1 && gameObject.Equals(objects[item1]) && !swiped)
        {
            display.text = "Missed";
        }
        else if(swiped)
        {
            swiped = false;
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
                Debug.Log("up swipe");
            }
            //swipe down
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("down swipe");
            }
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (item1 != -1 && gameObject.Equals(objects[item1]))
                {
                    display.text = "Correct";
                    score++;
                    scoreTxt.text = "Score:" + score;
                }
                else if (item1 != -1)
                {
                    display.text = "Wrong";
                }
                //createObject();
                rb.AddForce(-transform.right * 500000);
                swiped = true;
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                if (item1 != -1 && gameObject.Equals(objects[item1]))
                {
                    display.text = "Correct";
                    score++;
                    scoreTxt.text = "Score:" + score;
                }
                else if (item1 != -1)
                {
                    display.text = "Wrong";
                }
                //createObject();
                rb.AddForce(transform.right * 500000);
                swiped = true;
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
                    Debug.Log("up swipe");
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                }
            }
        }
    }
}
