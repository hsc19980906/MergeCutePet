using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Model;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHandler : HandlerBase
{
    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        dict = operationResponse.Parameters;

        switch (operationResponse.OperationCode)
        {
            case (byte)OpCode.Create:
                //第一次创建 需要保存宠物信息
                Initial();
                break;
            case (byte)OpCode.Refresh:
                //非第一次创建 从本地读取
                Dispatch(AreaCode.CHARACTER, CharacterEvent.LOAD_PLAYER, null);
                break;
            case (byte)OpCode.Rank:
                if (dict.ContainsKey((byte)ParameterCode.pet))
                {
                    PlayerCharacter.Instance.player.rank = Decode(dict[(byte)ParameterCode.pet]) as PlayerRank;
                    Dispatch(AreaCode.UI, UIEvent.TITLE_REFRESH, null);
                }
                Dispatch(AreaCode.UI, UIEvent.RANK_REFRESH, Decode(dict[(byte)ParameterCode.player]));
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 初始化玩家及其宠物信息
    /// </summary>
    private void Initial()
    {
        //服务器传来的玩家信息
        Player player = (Player)Decode(dict[(byte)ParameterCode.player]);
        string petname = (string)dict[(byte)ParameterCode.pet];
        Dispatch(AreaCode.CHARACTER, CharacterEvent.SAVE_FIRST, player);
        //Debug.Log("创建宠物");
        Dispatch(AreaCode.CHARACTER, CharacterEvent.ADD_PET_BY_NAME, petname);
    }

}
