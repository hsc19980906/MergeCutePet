using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDialog : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.EXIT_DIALOG_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.EXIT_DIALOG_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    public void Start()
    {
        setPanelActive(false);
    }

    public void OnConfirmClick()
    {
        Application.Quit();
    }

    public void OnCancelClick()
    {
        setPanelActive(false);
    }
}
