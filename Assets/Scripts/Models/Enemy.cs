using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PetModel;

public class Enemy
{
    public int Level;
    public string petName;
    public bool isDie = false;//如果敌人死了 就让玩家宠物获得经验 和 物品
    public PetKind petKind;
    public string Sprite { get; private set; }
    public int Add_Exp;
    public int Add_Coin;
    public int Add_Diamond;
    public double CC;

    public double ack = 180;//实际攻击力
    public double def = 20;//实际防御力
    public double mp = 2000;
    public double hp = 2000;
    public double sp = 50;

    public Enemy(int level, PetKind petKind,string name,string sprite)
    {
        ChangeLevel(level);
        this.petKind = petKind;
        this.petName = name;
        this.Sprite = sprite;
        ChangeAttris();
        SetAddExp();
        SetAddCoin();
        SetAddDiamond();
    }

    //根据初始等级 也就是地图的不同 确定地图中怪物的等级
    private void ChangeLevel(int level)
    {
        switch (level)
        {
            case 1:
                Level = UnityEngine.Random.Range(1, 10);
                CC = 1;
                break;
            case 2:
                Level = UnityEngine.Random.Range(11, 20);
                CC = 5;
                break;
            case 3:
                Level = UnityEngine.Random.Range(21, 30);
                CC = 15;
                break;
            case 4:
                Level = UnityEngine.Random.Range(31, 40);
                CC = 30;
                break;
            case 5:
                Level = UnityEngine.Random.Range(41, 50);
                CC = 50;
                break;
            case 6:
                Level = UnityEngine.Random.Range(51, 60);
                CC = 100;
                break;
            case 7:
                Level = UnityEngine.Random.Range(61, 70);
                CC = 200;
                break;
            case 8:
                Level = UnityEngine.Random.Range(71, 80);
                CC = 300;
                break;
            case 9:
                Level = UnityEngine.Random.Range(81, 90);
                CC = 400;
                break;
            case 10:
                Level = UnityEngine.Random.Range(91, 100);
                CC = 500;
                break;
            case 11:
                Level = UnityEngine.Random.Range(101, 110);
                CC = 600;
                break;
            case 12:
                Level = UnityEngine.Random.Range(111, 120);
                CC = 800;
                break;
            case 13:
                Level = UnityEngine.Random.Range(121, 130);
                CC = 1000;
                break;
            default:
                break;
        }
    }

    //将枚举类型的宠物种类解析成string
    public string GetPetKind()
    {
        switch (petKind)
        {
            case PetKind.Gold:
                return "金";
            case PetKind.Wood:
                return "木";
            case PetKind.Water:
                return "水";
            case PetKind.Fire:
                return "火";
            case PetKind.Soil:
                return "土";
            case PetKind.Boss:
                return "Boss";
            case PetKind.God:
                return "神";
            default:
                return "你的宠物什么都不是";
        }
    }

    //根据种族确定属性成长
    public double[] ChangeAttrisByKind()
    {
        switch (petKind)
        {
            case PetKind.Gold:
                return new double[] { 15, 10, 0.1, 50, 5 };
            case PetKind.Wood:
                return (new double[] { 15, 10, 0.1, 48, 6 });
            case PetKind.Water:
                return (new double[] { 15, 10, 0.15, 48, 5 });
            case PetKind.Fire:
                return (new double[] { 16, 9, 0.15, 48, 4 });
            case PetKind.Soil:
                return (new double[] { 14, 11, 0.1, 50, 4 });
            case PetKind.Boss:
                return (new double[] { 20, 15, 0.5, 60, 10 });
            default:
                return new double[] { };
        }
    }

    //根据属性成长、等级、成长值cc确定宠物具体属性
    public void ChangeAttris()
    {
        double[] attris = ChangeAttrisByKind();
        ack = Math.Round((200 + attris[0] * CC * Level), 2);
        def = Math.Round((20 + attris[1] * CC * Level), 2);
        if (mp > 1)
        {
            mp = Math.Round((2000 - attris[2] * CC * Level), 2);
            if (mp <= 0)
                mp = 1;
        }
        hp = Math.Round((2000 + attris[3] * CC * Level), 2);
        sp = Math.Round((50 + attris[4] * CC * Level), 2);
    }

    private void SetAddExp()
    {
        Add_Exp = (Level + Level / 2) * 30;
    }

    private void SetAddCoin()
    {
        Add_Coin = (Level + Level / 2) * 10;
    }

    private void SetAddDiamond()
    {
        Add_Diamond = Level + Level / 2;
    }

    public enum Map
    {
        NewBase,//新手基地
        Forest,//妖精森林
        Cliff,//潮汐海崖
        Lode, //巨石山脉
        Ridge,//黄金陵
        Beach,//炙热沙滩
        Volcano,//尤玛火山
        Desert,//死亡沙漠
        Mirage,//海市蜃楼
        Ice,//冰滩
        Sea,//海底世界
        Christmas,//圣诞小屋
        Eddy,//黑漩涡
        None
    }
}
