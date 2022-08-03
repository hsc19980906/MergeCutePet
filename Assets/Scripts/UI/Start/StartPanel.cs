using Common;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 开始的游戏面板
/// 放置“开始游戏”和“注册游戏”
/// </summary>
public class StartPanel : UIBase
{
    public Text playername;
    private CanvasGroup[] canvasGroups;
    // Use this for initialization
    void Awake()
    {
        Bind(UIEvent.REGISTED,UIEvent.START_PANEL_ACTIVE);
        canvasGroups = GetComponentsInChildren<CanvasGroup>();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.START_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            case UIEvent.REGISTED:
                IsRegisted((bool)message);
                break;
            default:
                break;
        }
    }

    private void IsRegisted(bool b)
    {
        if (b)
        {
            //print(PlayerCharacter.Instance.player.PlayerName == null);
            if (PlayerCharacter.Instance.player.PlayerName == null)
            {
                canvasGroups[1].alpha = 1;
                canvasGroups[1].blocksRaycasts = true;
            }
            else
            {
                playername.text = PlayerCharacter.Instance.player.PlayerName;
                canvasGroups[0].alpha = 1;
                canvasGroups[0].blocksRaycasts = true;
            }
        }
        else
        {
            canvasGroups[1].alpha = 1;
            canvasGroups[1].blocksRaycasts = true;
        }
    }

    public void LoginClick()
    {
        Dispatch(AreaCode.UI, UIEvent.LOGIN_PANEL_ACTIVE, true);
        //GameObject.Find("").gameObject.SetActive(true);
        canvasGroups[0].alpha = 0;
        canvasGroups[0].blocksRaycasts = false;
        canvasGroups[1].alpha = 0;
        canvasGroups[1].blocksRaycasts = false;
    }

    public void StartClick()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.player, PlayerCharacter.Instance.player.id);
        data.Add((byte)ParameterCode.pet, null);
        OpCustom(OpCode.Login, data);
        Dispatch(AreaCode.CHARACTER, CharacterEvent.LOAD_PLAYER, null);
    }

    public void ReStartClick()
    {
        if (NetManager.peer.PeerState == ExitGames.Client.Photon.PeerStateValue.Connected)
        {
            Dispatch(AreaCode.UI, UIEvent.WARN_PANEL_ACTIVE, true);
            canvasGroups[0].alpha = 0;
            canvasGroups[0].blocksRaycasts = false;
            canvasGroups[1].alpha = 0;
            canvasGroups[1].blocksRaycasts = false;
        }
        else
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG,new PromptMsg( "请确保连接到服务器！",Color.red));
    }

    public void RegistClick()
    {
        Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, true);
        canvasGroups[0].alpha = 0;
        canvasGroups[0].blocksRaycasts = false;
        canvasGroups[1].alpha = 0;
        canvasGroups[1].blocksRaycasts = false;
    }
}
