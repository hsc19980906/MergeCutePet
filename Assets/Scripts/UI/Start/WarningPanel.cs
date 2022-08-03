using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WarningPanel : UIBase
{
    // Use this for initialization
    private void Awake()
    {
        Bind(UIEvent.WARN_PANEL_ACTIVE);
    }

    private void Start()
    {
        setPanelActive(false);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.WARN_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    public void ConfirmClick()
    {
        Dispatch(AreaCode.UI, UIEvent.RESTART, null);
        //Dispatch(AreaCode.UI, UIEvent.REGISTED, true);
        setPanelActive(false);
    }

    public void CloseClick()
    {
        setPanelActive(false);
        Dispatch(AreaCode.UI, UIEvent.START_PANEL_ACTIVE, true);
    }
}
