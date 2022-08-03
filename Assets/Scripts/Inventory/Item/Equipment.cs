using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public double Attack { get; private set; }
    public double Defense { get; private set; }
    public double Hp { get; private set; }
    public double Mp { get; private set; }
    public double Speed { get; private set; }
    public EquipmentType Equipmentype { get; private set; }
    public int Level { get; private set; }//需求等级
    public Equipment(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice,string sprite
        , double atk, double def, double hp, double mp, double sp,EquipmentType equipmentType, ItemMoney buymoney, ItemMoney sellmoney, int level)
        :base(id,name,type,quality,des,capacity,buyPrice,sellPrice,sprite,buymoney,sellmoney)
    {
        Attack = atk;
        Defense = def;
        Hp = hp;
        Mp = mp;
        Speed = sp;
        Level = level;
        Equipmentype = equipmentType;
    }

    public Equipment(Item item, double atk, double def, double hp, double mp, double sp, EquipmentType equipmentType, int level)
    : base(item)
    {
        Attack = atk;
        Defense = def;
        Hp = hp;
        Mp = mp;
        Speed = sp;
        Level = level;
        Equipmentype = equipmentType;
    }

    public enum EquipmentType
    {
        Head,Body,Weapon,Wing,Shoe,Neck,//项链
        Bangle,//手镯
        Ring,//戒指
        Jewel//宝石
    }
}
