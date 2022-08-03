using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public ItemType Type { get; private set; }
    public ItemQuality Quality { get; private set; }
    public string Description { get; private set; }
    public int Capacity { get; private set; }
    public int BuyPrice { get; private set; }
    public int SellPrice { get; private set; }
    public string Sprite { get; private set; }
    public ItemMoney BuyMoney { get; private set; }
    public ItemMoney SellMoney { get; private set; }

    public Item()
    {
        ID = -1;
    }

    public Item(Item item)
    {
        ID = item.ID;
        Name = item.Name;
        Type = item.Type;
        Quality = item.Quality;
        Description = item.Description;
        BuyPrice = item.BuyPrice;
        SellPrice = item.SellPrice;
        BuyMoney = item.BuyMoney;
        SellMoney = item.SellMoney;

    }

    public Item(int id,string name,ItemType type,ItemQuality quality, string des,int capacity,int buyPrice,int sellPrice,string sprite,ItemMoney buymoney, ItemMoney sellmoney)
    {
        ID = id;
        Name = name;
        Type = type;
        Quality = quality;
        Description = des;
        Capacity = capacity;
        BuyPrice = buyPrice;
        SellPrice = sellPrice;
        Sprite = sprite;
        BuyMoney = buymoney;
        SellMoney = sellmoney;
    }

    public Item(int id,string name, ItemType type, ItemQuality quality, string des, string sprite)
    {
        ID = id;
        Name = name;
        Type = type;
        Quality = quality;
        Description = des;
        Sprite = sprite;
    }

    //物品类型
    public enum ItemType
    {
        Consumable,
        Equipment,
        Egg,
        Material,
        SkillBook
    }

    //品阶
    public enum ItemQuality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Myth
    }

    public enum ItemMoney
    {
        Coin,
        Diamond,
        Gold
    }

    private string UseMoneyKind(ItemMoney money)
    {
        switch (money)
        {
            case ItemMoney.Coin:
                return "金币";
            case ItemMoney.Diamond:
                return "钻石";
            case ItemMoney.Gold:
                return "元宝";
            default:
                return "不要钱了？";
        }
    }

    /// <summary> 
    /// 得到提示面板应该显示什么样的内容
    /// </summary>
    /// <returns></returns>
    public virtual string GetToolTipText()
    {
        string colorQuality = "";
        switch (Quality)
        {
            case ItemQuality.Common:
                colorQuality = "white";
                break;
            case ItemQuality.Uncommon:
                colorQuality = "lime";
                break;
            case ItemQuality.Rare:
                colorQuality = "navy";
                break;
            case ItemQuality.Epic:
                colorQuality = "magenta";
                break;
            case ItemQuality.Legendary:
                colorQuality = "orange";
                break;
            case ItemQuality.Myth:
                colorQuality = "red";
                break;
        }

        return string.Format("<color={4}>{0}</color>\n<size=40><color=green>购买价格：{1}{5} " +
            "\n出售价格：{2}{6}</color></size>\n<color=yellow><size=40>{3}</size></color>",
            Name, BuyPrice, SellPrice, Description, colorQuality,UseMoneyKind(BuyMoney), UseMoneyKind(SellMoney));
    }
}
