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
    public GameObject popup;

    public Text popupTxt;
    public Text confirmTxt;

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

        iAccessories.color = new Color32(238, 109, 88, 255);
        iHead.color = new Color32(246, 145, 116, 255);
        iBody.color = new Color32(246, 145, 116, 255);
        iLeg.color = new Color32(246, 145, 116, 255);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void buyItem()
    {
        PlayerPrefs.SetInt("item_" + itemNum, 1);
        
        if(itemNum > 30 && itemNum < 40)
        {
            PlayerPrefs.SetInt("body", itemNum);
        }

        if (itemNum > 10 && itemNum < 20)
        {
            PlayerPrefs.SetInt("accessory", itemNum);
        }

        if (itemNum > 20 && itemNum < 30)
        {
            PlayerPrefs.SetInt("head", itemNum);
        }

        if (itemNum > 40 && itemNum < 50)
        {
            PlayerPrefs.SetInt("leg", itemNum);
        }

        confirmTxt.text = "buy";
        coinIcon.SetActive(true);
        popup.SetActive(false);
        PlayerPrefs.SetInt("TBearya", PlayerPrefs.GetInt("TBearya") - price);

    }

    public void itemPress(int item)
    {

        this.itemNum = item;
        this.price = 25;

        
        if (price > PlayerPrefs.GetInt("TBearya"))
        {

        }
        else if (PlayerPrefs.GetInt("item_" + item) == 1)
        {
            popup.SetActive(true);

            confirmTxt.text = "equip";
            coinIcon.SetActive(false);

            popupTxt.text = "Do you wish to equip this item?";
                
        }
        else
        {
            popup.SetActive(true);

            switch (item)
            {
                case 31:
                    popupTxt.text = "Buy a green shirt for 25 bearyas?";
                    break;
                case 32:
                    popupTxt.text = "Buy a blue shirt for 25 bearyas?";
                    break;
                case 33:
                    popupTxt.text = "Buy a red shirt for 25 bearyas?";
                    break;
                case 34:
                    popupTxt.text = "Buy a black shirt for 25 bearyas?";
                    break;
                case 41:
                    popupTxt.text = "Buy a blue shorts for 25 bearyas?";
                    break;
                case 42:
                    popupTxt.text = "Buy a blue pants for 25 bearyas?";
                    break;
                case 43:
                    popupTxt.text = "Buy a khaki pants for 25 bearyas?";
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
