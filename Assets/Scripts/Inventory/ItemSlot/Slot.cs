using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 物品槽
/// </summary>
public class Slot : UIBase
{
    public GameObject ItemPrefab;
    protected Button btnShow;

    protected Button BtnShow
    {
        get
        {
            if (btnShow == null)
            {
                btnShow = GetComponent<Button>();
            }
            return btnShow;
        }
    }

    public virtual void Start()
    {
        BtnShow.onClick.AddListener(ShowMessage);
    }

    private void ShowMessage()
    {
        if(transform.childCount>0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<ItemUI>().Item.GetToolTipText();
            Dispatch(AreaCode.UI, UIEvent.ITEM_MSG, new ItemMsg() { itemMsg = toolTipText, position = new Vector3(Screen.width / 6, Screen.height / 2) });
        }
    }

    //清理后 initBag没有存储物品 unity认为slot下面还有物品
    //怎么解决？
    public void StoreItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject gameObject = Instantiate(ItemPrefab) as GameObject;
            gameObject.transform.SetParent(this.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }

    public void RemoveItem(int id)
    {
        if(transform.childCount>0)
        {
            transform.GetChild(0).GetComponent<ItemUI>().ReduceAmount();
            if (GetItemAmount() == 0)
                DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    //unity销毁物体 当前帧可以继续使用
    //相关属性例如transform还可以用；
    //在获取它Parent的所有物体时是能读到该gameObject属性的，
    //并且Parent读取childCount是包含该gameObject计数的
    public void RemoveItem()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }

    public int GetItemID()
    {
        if (transform.childCount > 0)
            return transform.GetChild(0).GetComponent<ItemUI>().Item.ID;
        else
            return -1;
    }

    public string GetItemName()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Name;
    }

    public int GetItemSellPrice()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.SellPrice;
    }

    public int GetItemBuyPrice()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.BuyPrice;
    }

    public Item.ItemMoney GetItemBuyMoney()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.BuyMoney;
    }

    public Item.ItemMoney GetItemSellMoney()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.SellMoney;
    }

    public int GetItemAmount()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Amount;
    }

    public bool IsFilled()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().Amount >= transform.GetChild(0).GetComponent<ItemUI>().Item.Capacity;
    }

}
