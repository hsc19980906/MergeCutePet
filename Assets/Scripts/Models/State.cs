using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class State
{
    public int PetCount=0;//记录玩家所有宠物数量
    public Map map = Map.None;
    public DateTime offTime = DateTime.MinValue;//离线时间
    public long TotalExp = 0;
    public bool isOnline = false;
    public int ExpBuffMinutes = 0;//离线经验加成时间
    public float ExpBuff = 1f;//离线经验加成倍数

    public int MergeNum = 0;//合成次数
    public int KillNum1 = 0;//杀怪数
    public int KillNum2 = 0;//杀怪数
    public int KillNum3 = 0;//杀怪数
    public int KillNum4 = 0;//杀怪数
    public int KillNum5 = 0;//杀怪数
    public int KillNum6 = 0;//杀怪数
    public int KillNum7 = 0;//杀怪数
    public int KillNum8 = 0;//杀怪数
    public int KillNum9 = 0;//杀怪数
    public int KillNum10 = 0;//杀怪数
    public int KillNum11 = 0;//杀怪数
    public int KillNum12 = 0;//杀怪数
    public int KillNum13 = 0;//杀怪数
}
