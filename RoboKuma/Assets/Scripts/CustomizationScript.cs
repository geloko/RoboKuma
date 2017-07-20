using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationScript : MonoBehaviour
{
    // 1 - Accessories
    // 2 - Head
    // 3 - Body
    // 4 - Leg
    
    public Vector2 firstPressPos;
    public Vector2 secondPressPos;
    public Vector2 currentSwipe;

    public GameObject leg, body, head, accessories;
    public GameObject popup, nmpopup;

    public Text popupTxt;
    public Text confirmTxt;
    public Text TBearya;
    public Text[] accessoryA, headA, bodyA, legA;
    public Image[] accessoryI, headI, bodyI, legI;
    public Button[] accessoryB, headB, bodyB, legB;
    public Sprite boughtI, lockedI;

    public RawImage[] lockedB, lockedL, lockedA;

    //public Text leg_1, leg_2, leg_3, body_1, body_2, body_3, body_4;

    public GameObject coinIcon;

    public Image iLeg, iBody, iHead, iAccessories;
    public RawImage iLegB, iBodyB, iHeadB, iAccessoriesB;

    public int itemNum, price, activeCategory;

    // Use this for initialization
    void Start()
    {
        leg.SetActive(false);
        body.SetActive(false);
        head.SetActive(false);
        accessories.SetActive(true);
        popup.SetActive(false);
        nmpopup.SetActive(false);

        activeCategory = 0;

        iAccessories.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);

        iAccessoriesB.color = new Color32(186, 82, 66, 255);
        iHeadB.color = new Color32(184, 112, 92, 255);
        iBodyB.color = new Color32(184, 112, 92, 255);
        iLegB.color = new Color32(184, 112, 92, 255);

        

        for (int i = 0; i < bodyA.Length; i++)
        {
            if (PlayerPrefs.GetInt("item_3" + (i + 1)) == 1)
            {
                bodyA[i].text = "Purchased";
                bodyI[i].overrideSprite = boughtI;
            }
        }

        for (int i = 0; i < legA.Length; i++)
        {
            if (PlayerPrefs.GetInt("item_4" + (i + 1)) == 1)
            { 
                legA[i].text = "Purchased";
                legI[i].overrideSprite = boughtI;
            }
        }

        for (int i = 0; i < headA.Length; i++)
        {
            if (PlayerPrefs.GetInt("item_2" + (i + 1)) == 1)
            {
                headA[i].text = "Purchased";
                headI[i].overrideSprite = boughtI;
            }
        }

        for (int i = 0; i < accessoryA.Length; i++)
        {
            if (PlayerPrefs.GetInt("item_1" + (i + 1)) == 1)
            {
                accessoryA[i].text = "Purchased";
                accessoryI[i].overrideSprite = boughtI;
            }
        }

        if (PlayerPrefs.GetInt("accessory") != 0)
            accessoryA[PlayerPrefs.GetInt("accessory") - 11].text = "Equipped";

        if (PlayerPrefs.GetInt("head") != 0)
            headA[PlayerPrefs.GetInt("head") - 21].text = "Equipped";

        if (PlayerPrefs.GetInt("body") != 0)
            bodyA[PlayerPrefs.GetInt("body") - 31].text = "Equipped";

        if (PlayerPrefs.GetInt("leg") != 0)
            legA[PlayerPrefs.GetInt("leg") - 41].text = "Equipped";

    }

    public void updateLockedItems()
    {
        if (PlayerPrefs.GetInt("Level", 1) >= 10)
        {
            lockedB[0].gameObject.SetActive(false);
            bodyB[5].interactable = true;
            bodyI[5].overrideSprite = null;
            bodyA[5].text = "500";
            lockedL[0].gameObject.SetActive(false);
            legI[2].overrideSprite = null;
            legB[2].interactable = true;
            legA[2].text = "500";
            lockedA[0].gameObject.SetActive(false);
            accessoryI[0].overrideSprite = null;
            accessoryB[0].interactable = true;
            accessoryA[0].text = "1000";
        }
        else
        {
            lockedB[0].gameObject.SetActive(true);
            bodyB[5].interactable = false;
            bodyI[5].overrideSprite = lockedI;
            bodyA[5].text = "Unlocked at Level 10";
            lockedL[0].gameObject.SetActive(true);
            legI[2].overrideSprite = lockedI;
            legB[2].interactable = false;
            legA[2].text = "Unlocked at Level 10";
            lockedA[0].gameObject.SetActive(true);
            accessoryI[0].overrideSprite = lockedI;
            accessoryB[0].interactable = false;
            accessoryA[0].text = "Unlocked at Level 10";
        }

        if (PlayerPrefs.GetInt("Level", 1) >= 15)
        {
            lockedB[1].gameObject.SetActive(false);
            bodyB[6].interactable = true;
            bodyI[6].overrideSprite = null;
            bodyA[6].text = "350";
        }
        else
        {
            lockedB[1].gameObject.SetActive(true);
            bodyB[6].interactable = false;
            bodyI[6].overrideSprite = lockedI;
            bodyA[6].text = "Unlocked at Level 15";
        }
    }

    // Update is called once per frame
    void Update()
    {
        SwipeForComputer();
    }

    public void equipItem()
    {
        PlayerPrefs.SetInt("item_" + itemNum, 1);
        
        if (itemNum > 10 && itemNum < 20)
        {
            if (PlayerPrefs.GetInt("accessory") != 0)
                accessoryA[PlayerPrefs.GetInt("accessory") - 11].text = "Purchased";
           
            if (itemNum == PlayerPrefs.GetInt("accessory"))
            {
                PlayerPrefs.SetInt("accessory", 0);
                accessoryA[itemNum - 11].text = "Purchased";
            }
            else
            {
                PlayerPrefs.SetInt("accessory", itemNum);
                accessoryA[itemNum - 11].text = "Equipped";
                accessoryI[itemNum - 11].overrideSprite = boughtI;
            }

        }

        if (itemNum > 20 && itemNum < 30)
        {
            if (PlayerPrefs.GetInt("head") != 0)
                headA[PlayerPrefs.GetInt("head") - 21].text = "Purchased";

            if (itemNum == PlayerPrefs.GetInt("head"))
            {
                PlayerPrefs.SetInt("head", 0);
                headA[itemNum - 21].text = "Purchased";
            }
            else
            {
                PlayerPrefs.SetInt("head", itemNum);
                headA[itemNum - 21].text = "Equipped";
                headI[itemNum - 21].overrideSprite = boughtI;
            }
        }

        if (itemNum > 30 && itemNum < 40)
        {
            if(PlayerPrefs.GetInt("body") != 0)
                bodyA[PlayerPrefs.GetInt("body") - 31].text = "Purchased";

            if (itemNum == PlayerPrefs.GetInt("body"))
            {
                PlayerPrefs.SetInt("body", 0);
                bodyA[itemNum - 31].text = "Purchased";
            }
            else
            {
                PlayerPrefs.SetInt("body", itemNum);
                bodyA[itemNum - 31].text = "Equipped";
                bodyI[itemNum - 31].overrideSprite = boughtI;
            }

        }

        if (itemNum > 40 && itemNum < 50)
        {
            if (PlayerPrefs.GetInt("leg") != 0)
                legA[PlayerPrefs.GetInt("leg") - 41].text = "Purchased";
            if (itemNum == PlayerPrefs.GetInt("leg"))
            {
                PlayerPrefs.SetInt("leg", 0);
                legA[itemNum - 41].text = "Purchased";
            }
            else
            {
                PlayerPrefs.SetInt("leg", itemNum);
                legA[itemNum - 41].text = "Equipped";
                legI[itemNum - 41].overrideSprite = boughtI;
            }

            

        }

        confirmTxt.text = "buy";
        coinIcon.SetActive(true);
        popup.SetActive(false);
        PlayerPrefs.SetInt("TBearya", PlayerPrefs.GetInt("TBearya") - price);
        TBearya.text = PlayerPrefs.GetInt("TBearya") + "";

    }

    public void itemPress(int item)
    {

        this.itemNum = item;
        this.price = 350;

        if (item > 10 && item < 20)
            this.price = 1000;
        if (item == 22)
            price = 500;

        if (item == 36)
            price = 500;

        if (item == 43)
            price = 500;
        
        
        if (PlayerPrefs.GetInt("item_" + item) == 1)
        {
            popup.SetActive(true);

            if(itemNum == PlayerPrefs.GetInt("accessory") || itemNum == PlayerPrefs.GetInt("head") || itemNum == PlayerPrefs.GetInt("body") || itemNum == PlayerPrefs.GetInt("leg"))
            {
                confirmTxt.text = "unequip";
                coinIcon.SetActive(false);
                this.price = 0;

                popupTxt.text = "Do you wish to unequip this item?";
            }
            else
            {
                confirmTxt.text = "equip";
                coinIcon.SetActive(false);
                this.price = 0;

                popupTxt.text = "Do you wish to equip this item?";
            }

            
                
        }
        else if (price > PlayerPrefs.GetInt("TBearya"))
        {
            nmpopup.SetActive(true);
        }
        else
        {
            popup.SetActive(true);

            switch (item)
            {
                case 11:
                    popupTxt.text = "Buy a Wristwatch for " + price + " bearyas?";
                    break;
                case 21:
                    popupTxt.text = "Buy a Soul Ring for " + price + " bearyas?";
                    break;
                case 22:
                    popupTxt.text = "Buy an Orange Drooping Cat for " + price + " bearyas?";
                    break;
                case 31:
                    popupTxt.text = "Buy a Green Shirt for "+ price + " bearyas?";
                    break;
                case 32:
                    popupTxt.text = "Buy a Blue Shirt for " + price + " bearyas?";
                    break;
                case 33:
                    popupTxt.text = "Buy a Red Shirt for " + price + " bearyas?";
                    break;
                case 34:
                    popupTxt.text = "Buy a Black Shirt for " + price + " bearyas?";
                    break;
                case 35:
                    popupTxt.text = "Buy a Yellow Shirt for " + price + " bearyas?";
                    break;
                case 41:
                    popupTxt.text = "Buy a Blue Shorts for " + price + " bearyas?";
                    break;
                case 42:
                    popupTxt.text = "Buy a Blue Pants for " + price + " bearyas?";
                    break;
                case 43:
                    popupTxt.text = "Buy a Khaki Pants for " + price + " bearyas?";
                    break;
            }
        }
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
            }
            //swipe down
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
            }
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                activeCategory += 1;

                switch (activeCategory)
                {
                    case 1:
                        headPress();
                        break;
                    case 2:
                        bodyPress();
                        break;
                    case 3:
                        legPress();
                        break;
                }
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                activeCategory -= 1;
                switch (activeCategory)
                {
                    case 0:
                        accessoriesPress();
                        break;
                    case 1:
                        headPress();
                        break;
                    case 2:
                        bodyPress();
                        break;
                }
            }
        }
    }

    public void accessoriesPress()
    {
        leg.SetActive(false);
        body.SetActive(false);
        head.SetActive(false);
        accessories.SetActive(true);

        activeCategory = 0;


        iAccessories.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);

        iAccessoriesB.color = new Color32(186, 82, 66, 255);
        iHeadB.color = new Color32(184, 112, 92, 255);
        iBodyB.color = new Color32(184, 112, 92, 255);
        iLegB.color = new Color32(184, 112, 92, 255);
    }

    public void headPress()
    {
        leg.SetActive(false);
        body.SetActive(false);
        head.SetActive(true);
        accessories.SetActive(false);

        activeCategory = 1;

        iHead.color = new Color32(238, 109, 88, 255);
        iAccessories.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);

        iHeadB.color = new Color32(186, 82, 66, 255);
        iAccessoriesB.color = new Color32(184, 112, 92, 255);
        iBodyB.color = new Color32(184, 112, 92, 255);
        iLegB.color = new Color32(184, 112, 92, 255);
    }

    public void bodyPress()
    {
        leg.SetActive(false);
        body.SetActive(true);
        head.SetActive(false);
        accessories.SetActive(false);

        activeCategory = 2;
        
        iBody.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iAccessories.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);

        iBodyB.color = new Color32(186, 82, 66, 255);
        iAccessoriesB.color = new Color32(184, 112, 92, 255);
        iHeadB.color = new Color32(184, 112, 92, 255);
        iLegB.color = new Color32(184, 112, 92, 255);
    }

    public void legPress()
    {
        leg.SetActive(true);
        body.SetActive(false);
        head.SetActive(false);
        accessories.SetActive(false);

        activeCategory = 3;

        iLeg.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iAccessories.color = new Color32(246, 145, 116, 255);

        iLegB.color = new Color32(186, 82, 66, 255);
        iAccessoriesB.color = new Color32(184, 112, 92, 255);
        iHeadB.color = new Color32(184, 112, 92, 255);
        iBodyB.color = new Color32(184, 112, 92, 255);
    }

}
