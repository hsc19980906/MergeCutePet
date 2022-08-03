using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : UIBase
{
    protected Slot[] slots;

    protected virtual void Awake()
    {
        //获取所有的物品槽
        slots = GetComponentsInChildren<Slot>();
    }

    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemByID<Item>(id);
        return StoreItem(item);
    }

    public void StoreItem(int id,int num)
    {
        while(num>0)
        {
            num--;
            StoreItem(id);
        }
    }

    public bool StoreItem(Item item)
    {
        if (item == null)
        {
            Debug.Log("未找到该物品");
            return false;
        }
        if (item.Capacity==1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.Log("没有物品槽");
                return false;
            }
            else
            {
                //这个槽肯定是空物品槽
                slot.StoreItem(item);
            }
        }
        else
        //存多样物品
        {
            Slot slot = FindSameIDItem(item);
            if (slot != null)
            {
                slot.StoreItem(item);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreItem(item);
                }
                else
                {
                    Debug.Log("没有物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    public int CountEmptySlot()
    {
        int num = 0;
        foreach (Slot slot in slots)
        {
            if (slot.transform.childCount == 0)
            {
                num++;
            }
        }
        return num;
    }

    private Slot FindEmptySlot()
    {
        foreach(Slot slot in slots)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    /// <summary>
    /// 找到相同物品ID的槽 存放同样的物品直到达到Capacity限制
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private Slot FindSameIDItem(Item item)
    {
        foreach(Slot slot in slots)
        {
            if (slot.transform.childCount > 0 && slot.GetItemID() == item.ID && !slot.IsFilled())
            {
                return slot;
            }
        }
        return null;
    }

    public List<Slot> GetItem()
    {
        List<Slot> items = new List<Slot>();
        foreach(Slot slot in slots)
        {
            items.Add(slot);
        }
        return items;
    }

    //找到存放对应物品id的物品槽
    public Slot FindSlotByItemID(int id)
    {
        foreach (Slot slot in slots)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemID() == id)
            {
                return slot;
            }
        }
        return null;
    }

    public int GetNumb()
    {
        int n = 0;
        foreach(Slot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                n++;
            }
        }
        return n;
    }

    //移除一个
    public void RemoveItem(Slot slot)
    {
        slot.RemoveItem(slot.GetItemID());
    }

    public void RemoveItem(Slot slot,int Amount)
    {
        while(Amount>0)
        {
            Amount--;
            RemoveItem(slot);
        }
    }

}
