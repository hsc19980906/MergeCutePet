using System.Collections;
using System.Collections.Generic;
using Common;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : HandlerBase
{
    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        dict = operationResponse.Parameters;
        switch (operationResponse.ReturnCode)
        {
            case (short)ReturnCode.Success:
                Debug.Log("登陆成功");
                LoginSuccess();
                break;
            case (short)ReturnCode.Fail:
                Debug.Log("登陆失败");
                if(dict.ContainsKey((byte)ParameterCode.player))
                {
                    Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "id不存在！");
                    Application.Quit();
                }
                else
                {
                    promptMsg.Change(operationResponse.DebugMessage, Color.red);
                    Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                }
                break;
            default:
                break;
        }
    }

    private void LoginSuccess()
    {
        if (dict.ContainsKey((byte)ParameterCode.player))
        {
            PlayerCharacter.Instance.CountMaxCE();
        }
        else
        {
            bool isCreate = dict.ContainsKey((byte)ParameterCode.Created);
            //已创建角色 跳转到main场景 然后加载角色信息
            if (isCreate)
            {
                OpCustom(OpCode.Refresh);
            }
            //未创建 则显示choose面板
            else
            {
                Dispatch(AreaCode.UI, UIEvent.CHOOSE_PANEL_ACTIVE, true);
                Dispatch(AreaCode.UI, UIEvent.LOGIN_PANEL_ACTIVE, false);
            }
        }

    }
}
