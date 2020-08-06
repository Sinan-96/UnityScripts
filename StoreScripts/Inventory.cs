using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;
using Photon.Realtime;
using JetBrains.Annotations;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    private JSONObject itemsData; // Json document with all the different items we want to have in the shop
    public static int selectedItemIndex { get; private set; } // Index of the selected item in the items list
    public class shopItem
    {
        /* All the charactheristics of 
         * a shop item*/
        public bool Bought, Selected;
        public int GoldPrice;
        public string Name;

        //Constructor
        public shopItem(string name , bool bought, bool selected , int gp)
        {
            Bought = bought; Selected = selected; GoldPrice = gp; Name = name; 
        }

    }
    public static List<shopItem> items;
    
    /*Classes for the different currencies,
     * set is private to make it more secure*/
    public static int Gold { get; private set; }
    public static int Diamond { get; private set; }

    private void Awake()
    {
        Debug.Log("Im awake");
        Instance = this;
        //If this is the first time playing
        if (!PlayerPrefs.HasKey("Chars"))
        {
            PlayerPrefs.SetString("Chars", "{\"Chars\":[{\"Name\":\"Char1\",\"Bought\":true,\"Selected\":true,\"Gold price\":10000},{\"Name\":\"Char2\",\"Bought\":false,\"Selected\":false,\"Gold price\":10000},{\"Name\":\"Char3\",\"Bought\":false,\"Selected\":false,\"Gold price\":10000}]}");
            PlayerPrefs.SetInt("Gold", 1000);
        }
        Gold = PlayerPrefs.GetInt("Gold");

        itemsData = JSONObject.Parse(PlayerPrefs.GetString("Chars"));
        items = new List<shopItem>();
        /*Creating shopItems out of the items stored in the 
         * PlayerPrefs string*/
        for(int i = 0; i< itemsData.GetArray("Chars").Length; i++)
        {
            items.Add(new shopItem(itemsData.GetArray("Chars")[i].Obj.GetString("Name"),
                                   itemsData.GetArray("Chars")[i].Obj.GetBoolean("Bought"),
                                   itemsData.GetArray("Chars")[i].Obj.GetBoolean("Selected"),
                                   (int)itemsData.GetArray("Chars")[i].Obj.GetNumber("Gold price")));
            if (items[i].Selected)
                selectedItemIndex = i;

        }

    }
    /* Function for deselecting the previously 
     * selected item, and selecting the currently
     * selected item*/
    public void selectItem(int index)
    {
        //Deselecting
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].Selected)
            {
                items[i].Selected = false;
                itemsData.GetArray("Chars")[i].Obj.GetValue("Selected").Boolean = false;
            }

        }
        //Selecting
        items[index].Selected = true;
        itemsData.GetArray("Chars")[index].Obj.GetValue("Selected").Boolean = true;
        
        selectedItemIndex = index;
        PlayerPrefs.SetString("Chars", itemsData.ToString());
        PlayerPrefs.Save();
    }
    /*Method for buying and item, 
     * the bool gold is true if you buy with gold ,and
     * false if you buy with diamonds*/
    public void BuyItem(int index)
    {
        Gold = PlayerPrefs.GetInt("Gold");
        if (Subtract(items[index].GoldPrice)) 
            {
                items[index].Bought = true;
                itemsData.GetArray("Chars")[index].Obj.GetValue("Bought").Boolean = true;
                selectItem(index);
            }
        
        
    }
    // Subtract from your pool of a currency when you buy something with that currency
    private bool Subtract(int value)
    {
        // if you buy with gold
            if (Gold - value < 0)
                return false;
            Debug.Log(Gold);
            Gold -= value;
            PlayerPrefs.SetInt("Gold", Gold);
            return true;
    }


}
    

