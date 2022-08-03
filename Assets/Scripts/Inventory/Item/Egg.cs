﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Item
{
    public Egg(int id, string name, ItemType type, ItemQuality quality, string des, int capacity, int buyPrice, int sellPrice, string sprite, ItemMoney buymoney, ItemMoney sellmoney)
       : base(id, name, type, quality, des, capacity, buyPrice, sellPrice, sprite,buymoney,sellmoney)
    {

    }

}
