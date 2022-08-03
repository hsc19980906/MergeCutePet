using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Item Item { get;  set; }
    public int Amount { get; set; }
    private Image itemIcon;
    private Text amountText;
    private Text nameText;

    private Image ItemIcon
    {
        get
        {
            if (itemIcon == null)
            {
                itemIcon = GetComponent<Image>();
            }
            return itemIcon;
        }
    }

    private Text AmountText
    {
        get
        {
            if (amountText == null)
            {
                amountText = transform.Find("Amount").GetComponent<Text>();
            }
            return amountText;
        }
    }

    private Text NameText
    {
        get
        {
            if (nameText == null)
            {
                nameText = transform.Find("Name").GetComponent<Text>();
            }
            return nameText;
        }
    }


    public void SetItem(Item item,int amount = 1)
    {
        this.Item = item;
        this.Amount = amount;
        UpdateUI();
    }

    public void AddAmount()
    {
        Amount++;
        AmountText.text = Amount.ToString();
    }

    public void ReduceAmount()
    {
        Amount--;
        AmountText.text = Amount.ToString();
    }

    private void UpdateUI()
    {
        ItemIcon.sprite = Resources.Load<Sprite>(Item.Sprite);
        AmountText.text = Amount.ToString();
        NameText.text = Item.Name;
    }
}
