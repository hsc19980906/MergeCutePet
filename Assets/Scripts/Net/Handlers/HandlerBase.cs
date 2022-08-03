using Common;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HandlerBase
{
    public PromptMsg promptMsg;
    public Dictionary<byte, object> dict;

    public HandlerBase()
    {
        promptMsg = new PromptMsg();
    }
    public abstract void OnOperationResponse(OperationResponse operationResponse);

    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }

    public void OpCustom(OpCode opCode)
    {
        NetManager.peer.OpCustom((byte)opCode, null, true);
    }

    public object Decode(object value)
    {
        return Common.Tool.EncodeTool.DecodeObject((byte[])value);
    }
}
