using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMsg : UIBase
{
    public Text msg;
    private void Awake()
    {
        Bind(UIEvent.SYSTEM_MSG);
    }

    private void Start()
    {
        if(!PetCharacter.Instance.state.isOnline)
        {
            msg.text += "\n" + "亲爱的玩家:\n您现在正离线游戏\n无法使用联网功能";
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SYSTEM_MSG:
                msg.text += "\n" + message.ToString();
                break;
            default:
                break;
        }
    }

}
