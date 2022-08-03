
using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public class RegisterHandler : HandlerBase
{
    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        switch (operationResponse.ReturnCode)
        {
            case (short)ReturnCode.Success:
                promptMsg.Change("注册成功！", Color.green);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                //注册成功则直接为玩家显示登陆界面
                Dispatch(AreaCode.UI, UIEvent.REGIST_PANEL_ACTIVE, false);
                Dispatch(AreaCode.UI, UIEvent.LOGIN_PANEL_ACTIVE, true);
                break;
            case (short)ReturnCode.Fail:
                promptMsg.Change(operationResponse.DebugMessage, Color.red);
                Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
                break;
            default:
                break;
        }
    }

}
