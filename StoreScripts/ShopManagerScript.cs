using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    [SerializeField]
    private Transform[] shopButtons;
    public Transform GoldTxt,SelectedCharTxt;
    public Color SelectedColor, NonSelectedColor;

    private int prevSelectedIndex;

    private void Start()
    {
        //Names the buttons in the shop
        for (int i = 0; i < shopButtons.Length; i++)
        {
            Debug.Log(shopButtons[i].GetChild(0));
            shopButtons[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = Inventory.items[i].Name;
        }
        UpdateUI();
    }

    public void BuyOrSelectItem(int index)
    {
        if (Inventory.items[index].Bought)
        {
            Inventory.Instance.selectItem(index);
            UpdateUI();
        }
        else
        {
            Inventory.Instance.BuyItem(index);
            UpdateUI();
        }
    }
    //Updates the text onscreen
    public void UpdateUI()
    {
        //Changes the text on the text that displays gold,diamonds and selected char
        GoldTxt.GetComponent<TextMeshProUGUI>().text = "Gold: " + PlayerPrefs.GetInt("Gold");
        SelectedCharTxt.GetComponent<TextMeshProUGUI>().text = "Selected char: " + Inventory.items[Inventory.selectedItemIndex].Name;

        //Change color on the buttons based upon if they are selected or not
        shopButtons[prevSelectedIndex].GetComponent<Image>().color = NonSelectedColor;
        shopButtons[Inventory.selectedItemIndex].GetComponent<Image>().color = SelectedColor;

        prevSelectedIndex = Inventory.selectedItemIndex;

    }
}


