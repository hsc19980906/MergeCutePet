using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家信息面板
/// 显示的信息直接在Text文本上“+”就可以 
/// </summary>
public class PlayInfoPanel : UIBase
{
    private Text[] texts;

    private void Awake()
    {
        Bind(UIEvent.PLAY_INFO_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PLAY_INFO_ACTIVE:
                setPanelActive((bool)message);
                if((bool)message)
                   Refresh();
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        texts = GetComponentsInChildren<Text>();
        setPanelActive(false);
    }


    private void Refresh()
    {
        PlayerModel player = PlayerCharacter.Instance.player;
        texts[0].text = "玩家编号：" + player.id.ToString();
        texts[1].text = "玩家性别：" + player.Sex;
        if(player.rank!=null)
        {
            texts[2].text = "玩家称号：" + player.rank.WhichTitle();
            texts[3].text = "最强战力：" + player.rank.Max_CE.ToString();
        }
        else
        {
            texts[2].text = "玩家称号：暂未获取 请稍等" ;
            texts[3].text = "最强战力：暂未获取 请稍等";
        }
        texts[4].text = "宠物数量：" + PetCharacter.Instance.state.PetCount;
        texts[5].text = "游戏时长：" + player.day.ToString()+"天"+ player.hour.ToString() + "小时" + player.minute.ToString() + "分钟" ;
    }

  
}
