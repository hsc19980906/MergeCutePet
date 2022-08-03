using Common.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModel
{
    public int id;//玩家id
    public string PlayerName;//玩家昵称
    public string Sex;//玩家性别
    public string profile;//玩家头像
    public PlayerRank rank;//玩家排行榜信息

    public int day = 0;//记录游戏天数
    public int hour = 0;//记录游戏小时数
    public int minute = 0;//记录游戏分钟数

    public int Coin = 1000;//金币
    public int Diamond = 100;//钻石
    public int Gold = 0;//元宝

    /// <summary>
    /// 第一次创建角色 初始化
    /// </summary>
    public void Initial(Player player)
    {
        id = player.id_player;
        PlayerName = player.PlayerName;
        Sex = player.Sex;
        profile = player.Profile;
        day = 0;
        hour = 0;
        minute = 0;
        Coin = 1000;
        Diamond = 100;
        Gold = 0;
        rank = new PlayerRank();
    }

    //修改玩家游戏时间
    public void ChangeTime(int day, int hour, int minute)
    {
        this.day += day;
        this.minute += minute;
        this.hour += hour;
    }

    //修改玩家金钱数量
    public void ChangeMoney(int coin,int diamond,int gold)
    {
        Coin += coin;
        Diamond += diamond;
        Gold += gold;
    }


}


