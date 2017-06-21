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

    public GameObject leg, body, head, accessories;
    public GameObject popup, nmpopup;

    public Text popupTxt;
    public Text confirmTxt;
    public Text TBearya;
    public Text[] legA, bodyA, headA, accessoryA;
    //public Text leg_1, leg_2, leg_3, body_1, body_2, body_3, body_4;

    public GameObject coinIcon;

    public Image iLeg, iBody, iHead, iAccessories;

    public int itemNum, price;

    // Use this for initialization
    void Start()
    {

        leg.SetActive(false);
        body.SetActive(false);
        head.SetActive(false);
        accessories.SetActive(true);
        popup.SetActive(false);
        nmpopup.SetActive(false);

        iAccessories.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);

        for(int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("item_3" + (i + 1)) == 1 && PlayerPrefs.GetInt("body") != 31 + i)
                bodyA[i].text = "Purchased";
        }

        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.GetInt("item_4" + (i + 1)) == 1 && PlayerPrefs.GetInt("leg") != 41 + i)
                legA[i].text = "Purchased";
        }

        if (PlayerPrefs.GetInt("body") != 0)
            bodyA[PlayerPrefs.GetInt("body") - 31].text = "Equipped";

        if (PlayerPrefs.GetInt("leg") != 0)
            legA[PlayerPrefs.GetInt("leg") - 41].text = "Equipped";

    }

    // Update is called once per frame
    void Update()
    {
        
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
        this.price = 250;

        
        
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

    public void accessoriesPress()
    {
        leg.SetActive(false);
        body.SetActive(false);
        head.SetActive(false);
        accessories.SetActive(true);


        iAccessories.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);
    }

    public void headPress()
    {
        leg.SetActive(false);
        body.SetActive(false);
        head.SetActive(true);
        accessories.SetActive(false);

        iHead.color = new Color32(238, 109, 88, 255);
        iAccessories.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);
    }

    public void bodyPress()
    {
        leg.SetActive(false);
        body.SetActive(true);
        head.SetActive(false);
        accessories.SetActive(false);
        
        iBody.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iAccessories.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);
    }

    public void legPress()
    {
        leg.SetActive(true);
        body.SetActive(false);
        head.SetActive(false);
        accessories.SetActive(false);

        iLeg.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iAccessories.color = new Color32(246, 145, 116, 255);
    }

}
