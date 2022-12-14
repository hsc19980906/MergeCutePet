using Common;
using Common.Tool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBase
{
    /// <summary>
    /// 自身关心的消息集合
    /// </summary>
    private List<int> list = new List<int>();

    /// <summary>
    /// 绑定一个或多个消息
    /// </summary>
    /// <param name="eventCodes">Event codes.</param>
    protected void Bind(params int[] eventCodes)
    {
        list.AddRange(eventCodes);
        UIManager.Instance.Add(list.ToArray(), this);
    }

    /// <summary>
    /// 解除绑定的消息
    /// </summary>
    protected void UnBind()
    {
        UIManager.Instance.Remove(list.ToArray(), this);
        list.Clear();
    }

    /// <summary>
    /// 自动移除绑定的消息
    /// </summary>
    public  virtual void OnDestroy()
    {
        if (list != null)
        {
            UnBind();
        }
            
    }

    /// <summary>
    /// 封装MsgCenter的发消息
    /// </summary>
    /// <param name="areaCode">Area code.</param>
    /// <param name="eventCode">Event code.</param>
    /// <param name="message">Message.</param>
    public void Dispatch(int areaCode, int eventCode, object message)
    {
        MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
    }

    /// <summary>
    /// 封装Photon发消息
    /// </summary>
    /// <param name="opCode">区别模块</param>
    /// <param name="data">数据</param>
    /// <param name="sendReliable">发送时是否可靠</param>
    public void OpCustom(OpCode opCode,Dictionary<byte,object> data)
    {
        NetManager.peer.OpCustom((byte)opCode, data, true);
    }

    public byte[] EncodeObject(object value)
    {
        return EncodeTool.EncodeObject(value);
    }

    /// <summary>
    /// 设置面板显示
    /// </summary>
    /// <param name="active"></param>
    protected void setPanelActive(bool active)
    {
        gameObject.SetActive(active);
    }

}
