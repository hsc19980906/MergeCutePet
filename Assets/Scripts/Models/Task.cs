using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public int id;
    public string Name;
    public string Info;
    //任务要求或奖励类型 物品id和数量 非物品则为-1
    public Dictionary<RequireType, Dictionary<int, int>> Requires;
    public Dictionary<RewardType, Dictionary<int, int>> Rewards;

    public bool Finished;
    public TaskType taskType;
    //public int FinishTime;

    public Task()
    {
        Requires = new Dictionary<RequireType, Dictionary<int, int>>();
        Rewards = new Dictionary<RewardType, Dictionary<int, int>>();
    }

    public string RequireInfo()
    {
        string requireInfo = "任务要求：\n";
        string petRequire = "";
        foreach (Task.RequireType requireType in Requires.Keys)
        {
            switch (requireType)
            {
                case RequireType.NewBase:
                case RequireType.Forest:
                case RequireType.Cliff:
                case RequireType.Lode:
                case RequireType.Ridge:
                case RequireType.Beach:
                case RequireType.Volcano:
                case RequireType.Desert:
                case RequireType.Mirage:
                case RequireType.Ice:
                case RequireType.Sea:
                case RequireType.Christmas:
                case RequireType.Eddy:
                    requireInfo += GetMapName(requireType) + "打怪" + Requires[requireType][-1] + "只\n";
                    break;
                case RequireType.Item:
                    foreach (int id in Requires[requireType].Keys)
                    {
                        requireInfo += InventoryManager.Instance.GetItemNameByID(id) + " x " + Requires[requireType][id] + "\n";
                    }                    
                    break;
                case RequireType.Pet:
                case RequireType.Level:
                    if(requireType == RequireType.Pet)
                    {
                        if (Requires[requireType].ContainsKey(0)&&!Requires[requireType].ContainsKey(1))
                            petRequire = "任意一只五系波姆";
                        if (Requires[requireType].ContainsKey(1))
                            petRequire = "任意一只五系波姆、一只二阶宠物\n(如波波姆)";
                        if (Requires[requireType].ContainsKey(5))
                            petRequire = "一只二阶宠物(如波波姆)";
                    }
                    if (requireType == RequireType.Level && petRequire != "")
                    {
                        petRequire += Requires[requireType][-1] + "级\n";
                    }
                    requireInfo = "任务要求：\n" + petRequire;
                    break;
                case RequireType.Merge:
                    requireInfo +="合成" + Requires[requireType][-1] + "次\n";
                    break;
                case RequireType.Coin:
                    requireInfo += "金币:" + Requires[requireType][-1] + "\n";
                    break;
                case RequireType.Diamond:
                    requireInfo += "钻石:" + Requires[requireType][-1] + "\n";
                    break;
                default:
                    break;
            }
        }
        return requireInfo;
    }

    public string RewardInfo()
    {
        string rewardInfo = "任务奖励：\n";
        foreach (RewardType rewardType in Rewards.Keys)
        {
            switch (rewardType)
            {
                case RewardType.Item:
                    foreach (int id in Rewards[rewardType].Keys)
                    {
                        rewardInfo += InventoryManager.Instance.GetItemNameByID(id) + " x " + Rewards[rewardType][id] + "\n";
                    }
                    break;
                case RewardType.Coin:
                    rewardInfo += "金币+" + Rewards[rewardType][-1] + "\n";
                    break;
                case RewardType.Diamond:
                    rewardInfo += "钻石+" + Rewards[rewardType][-1] + "\n";
                    break;
                case RewardType.Gold:
                    rewardInfo += "元宝+" + Rewards[rewardType][-1] + "\n";
                    break;
                case RewardType.Exp:
                    rewardInfo += "Exp+" + Rewards[rewardType][-1] + "\n";
                    break;
                default:
                    break;
            }
        }
        return rewardInfo;
    }

    public enum TaskType
    {
        Main,//主线任务
        Daily,//日常任务
        Collect,//收集任务
        ExChange//兑换任务
    }

    public enum RewardType
    {
        Item,
        Coin,
        Diamond,
        Gold,
        Exp,
    }

    public enum RequireType
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
        Item,//物品
        Level,
        Pet,
        Merge,
        Coin,
        Diamond,
    }

    private string GetMapName(RequireType requireType)
    {
        switch (requireType)
        {
            case RequireType.NewBase:
                return "新手基地";
            case RequireType.Forest:
                return "妖精森林";
            case RequireType.Cliff:
                return "潮汐海崖";
            case RequireType.Lode:
                return "巨石山脉";
            case RequireType.Ridge:
                return "黄金陵";
            case RequireType.Beach:
                return "炙热沙滩";
            case RequireType.Volcano:
                return "尤玛火山";
            case RequireType.Desert:
                return "死亡沙漠";
            case RequireType.Mirage:
                return "海市蜃楼";
            case RequireType.Ice:
                return "冰滩";
            case RequireType.Sea:
                return "海底世界";
            case RequireType.Christmas:
                return "圣诞小屋";
            case RequireType.Eddy:
                return "黑漩涡";
            default:
                return "";
        }
    }
}