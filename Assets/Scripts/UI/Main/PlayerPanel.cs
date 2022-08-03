using Common;
using Common.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示玩家信息的简略面板 只显示玩家头像和名字
/// 点击头像 显示详细面板即PlayInfoPanel
/// </summary>
public class PlayerPanel : UIBase
{
    private Button btnPlayer;
    private Text PlayerName;
    private Text CoinNum;
    private Text DiaNum;
    private Text GoldNum;
    private Sprite[] sprites;

    private void Awake()
    {
        Bind(UIEvent.REFRESH_PLAYER_SIMPLE, UIEvent.REFRESH_PLAYER_SIMPLE_FIRST,UIEvent.REFRESH_PLAYER_MONEY);
        //Debug.Log("刷新一下");
        btnPlayer = transform.Find("btnPlayer").GetComponent<Button>();
        Text[] texts = GetComponentsInChildren<Text>();
        PlayerName = texts[0];
        CoinNum = texts[1];
        DiaNum = texts[2];
        GoldNum = texts[3];

        btnPlayer.onClick.AddListener(PlayInfoClick);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REFRESH_PLAYER_SIMPLE_FIRST:
                //Debug.Log("刷新一下界面");                
                Refresh(message as Player);
                break;
            case UIEvent.REFRESH_PLAYER_SIMPLE:
                //Debug.Log("刷新一下界面");                
                Refresh(message as PlayerModel);
                break;
            case UIEvent.REFRESH_PLAYER_MONEY:
                //Debug.Log("刷新一下界面");                
                RefreshMoney(message as PlayerModel);
                break;
            //case UIEvent.PLAY_INFO_ACTIVE:
            //    setPanelActive((bool)message);
            //    break;
            default:
                break;
        }
    }

    private void RefreshMoney(PlayerModel player)
    {
        CoinNum.text = player.Coin.ToString();
        DiaNum.text = player.Diamond.ToString();
        GoldNum.text = player.Gold.ToString();
    }

    private void Refresh(PlayerModel player)
    {
        btnPlayer.image.sprite = Resources.Load<Sprite>(player.profile);
        PlayerName.text = player.PlayerName;
        RefreshMoney(player);
    }


    public override void OnDestroy()
    {
        base.OnDestroy();
        btnPlayer.onClick.RemoveAllListeners();

        //Player player = new Player()
        //{
        //    id_player = PlayerCharacter.Instance.player.id,
        //};
    }

    /// <summary>
    /// 显示玩家信息
    /// </summary>
    private void PlayInfoClick()
    {
        Dispatch(AreaCode.UI, UIEvent.PLAY_INFO_ACTIVE, true);
    }

    /// <summary>
    /// 刷新面板信息
    /// </summary>
    /// <param name="player"></param>
    private void Refresh(Player player)
    {
        btnPlayer.image.sprite = Resources.Load<Sprite>(player.Profile);
        PlayerName.text = player.PlayerName;
        CoinNum.text = "1000";
        DiaNum.text = "100";
        GoldNum.text = "0";
    }
}
