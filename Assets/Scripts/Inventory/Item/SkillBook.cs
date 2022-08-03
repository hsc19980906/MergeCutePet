using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook :Item
{
    public double Attack { get; private set; }
    public double Defense { get; private set; }
    public double Hp { get; private set; }
    public SkillBook(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, 
        string sprite, ItemMoney buymoney, ItemMoney sellmoney, double ack,double def,double hp)
    : base(id, name, type, quality, des, capacity, buyPrice, sellPrice, sprite, buymoney,sellmoney)
    {
        Attack = ack;
        Defense = def;
        Hp = hp;
    }
}
