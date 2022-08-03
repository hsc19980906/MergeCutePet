using Common;
using Common.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.REGIST_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REGIST_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    private Button btnRegist;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;
    private InputField inputRepeat;

    private PromptMsg promptMsg;

    // Use this for initialization
    void Start()
    {
        btnRegist = transform.Find("btnRegist").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();
        inputRepeat = transform.Find("inputRepeat").GetComponent<InputField>();

        btnClose.onClick.AddListener(closeClick);
        btnRegist.onClick.AddListener(registClick);

        promptMsg = new PromptMsg();

        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnClose.onClick.RemoveListener(closeClick);
        btnRegist.onClick.RemoveListener(registClick);
    }

    /// <summary>
    /// 注册按钮的点击事件处理
    /// </summary>
    private void registClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Change("密码不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (inputPassword.text.Length < 8
            || inputPassword.text.Length > 16)
        {
            promptMsg.Change("密码长度必须在8到16之间！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputRepeat.text)
            || inputRepeat.text != inputPassword.text)
        {
            promptMsg.Change("请确认两次输入密码的一致！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        //用户注册 向服务器提交
        Player player = new Player { Account = inputAccount.text, Password = inputPassword.text };

        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.player, EncodeObject(player));
        OpCustom(OpCode.Register, data);
    }

    private void closeClick()
    {
        setPanelActive(false);
        if (PlayerCharacter.Instance.player.PlayerName == null)
            Dispatch(AreaCode.UI, UIEvent.REGISTED, false);
        else
            Dispatch(AreaCode.UI, UIEvent.REGISTED, true);
    }
}
