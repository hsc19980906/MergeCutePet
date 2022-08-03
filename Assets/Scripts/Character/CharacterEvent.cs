using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEvent
{
    public const int SAVE_PLAYER = 0;//保存玩家数据

    public const int LOAD_PET = 1;//加载宠物信息
    public const int LOAD_PLAYER = 2;//加载玩家数据

    public const int SAVE_FIRST = 3;//第一次创建角色后 保存宠物信息

    public const int ADD_PET_BY_NAME= 4;//根据宠物名添加宠物 属于初始化 一开始没有该宠物
    public const int REFRESH_PET = 5;//有宠物 改变它的归属 从一方移除 添加到另一方

    public const int RANCH_REFRESH = 14;//宠物牧场更新
    public const int PET_BAG_REFRESH = 15;//宠物背包更新 

    public const int SAVE_MAP = 16;//导入敌人
    public const int GET_STATE = 24;//获取游戏状态 离线时间、经验池、宠物数量


    public const int START_GAME = 42;//开始游戏后 再读入数据
}
