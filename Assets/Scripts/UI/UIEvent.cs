using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 存储所有的UI事件码
/// </summary>
public class UIEvent
{
    public const int LOGIN_PANEL_ACTIVE = 0;//设置登陆面板的显示
    public const int REGIST_PANEL_ACTIVE = 1;//设置注册面板的显示
    public const int START_PANEL_ACTIVE = 1111;//设置注册面板的显示

    public const int REFRESH_PLAYER_SIMPLE_FIRST = 2;//刷新信息面板 参数：由服务器定
    public const int REFRESH_PLAYER_SIMPLE = 22;//刷新信息面板 参数：由服务器定
    public const int REFRESH_PLAYER_MONEY = 222;//刷新角色金钱数量

    public const int CHOOSE_PANEL_ACTIVE = 3;//设置选角选宠面板显示
    public const int BATTLE_PANEL_ACTIVE = 4;//设置战斗面板的显示
    public const int BAG_PANEL_ACTIVE = 5;//设置背包面板的显示
    public const int PLAY_INFO_ACTIVE = 6;//设置玩家详细信息面板的显示
    public const int PET_PANEL_ACTIVE = 7;//设置宠物面板的显示
    public const int TASK_PANEL_ACTIVE = 8;//设置任务面板的显示
    public const int TOWN_PANEL_ACTIVE = 9;//设置主城面板的显示
    public const int SHOP_PANEL_ACTIVE = 11;//设置商店面板的显示
    public const int TEMPLE_PANEL_ACTIVE = 12;//设置宠物神殿面板的显示
    public const int RANCH_PANEL_ACTIVE = 13;//设置宠物牧场面板的显示
    public const int EXIT_DIALOG_ACTIVE = 133;//设置宠物牧场面板的显示
    public const int RANK_PANEL_ACTIVE = 1333;//设置宠物牧场面板的显示
    public const int WARN_PANEL_ACTIVE = 40;//设置警告面板显示

    public const int RANCH_REFRESH = 14;//宠物牧场更新
    public const int PET_BAG_REFRESH = 15;//宠物背包更新 
    public const int PET_INFO_REFRESH = 16;//宠物信息更新 
    public const int PET_EQUIP_REFRESH = 20;//宠物装备更新
    public const int PET_REVIVE = 19;//宠物复活
    public const int LOAD_ENEMY = 111;//加载宠物数据
    public const int BATTLE_PET_REFRESH = 17;//战斗宠物更新

    public const int BAG_ADD_REFRESH = 18;//背包刷新 增加物品
    public const int BAG_REMOVE_REFRESH = 23;//背包刷新 去除物品
    //public const int MAP_SHOW = 18;//显示地图 选择地图

    public const int TASK_FINISH = 21;//任务是否可以完成
    public const int EVOLUTEA_FINISH = 24;//进化要求是否满足
    public const int EVOLUTEB_FINISH = 26;//进化要求是否满足
    public const int EVOLUTEA = 25;//进化要求满足 开始进化
    public const int EVOLUTEB = 27;//进化要求满足 开始进化
    public const int GET_MERGE_ITEM = 28;//在背包获取合成物品
    public const int MERGE_ITEM_REFRESH = 29;//刷新合成物品

    public const int LEARN_SKILL_BOOK = 30;//主宠学习技能

    public const int RANK_REFRESH = 31;//刷新排名信息
    public const int REGISTED = 32;//已注册 获取本地存档

    public const int SYSTEM_MSG = 33;//显示系统消息
    public const int ITEM_MSG = 34;//显示物品消息
    public const int PROMPT_MSG = int.MaxValue;

    public const int REMOVE_TASK = 35;//删除只完成指定次数的任务
    public const int TITLE_REFRESH = 36;//刷新称号
    //public const int MAIN_TASK_FINISH = 37;//主线任务全部完成
    public const int MAIN_TASK_FINISH_TIME = 38;//主线任务完成次数
    public const int OFFTIME_BENEFIR_COUNT = 39;//离线收益计算

    public const int RESTART = 41;//重新开始游戏 登录后重新创建角色
}
