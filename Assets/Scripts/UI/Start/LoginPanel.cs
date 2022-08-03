using Common;
using Common.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBase
{
    private bool restart = false;
    private void Awake()
    {
        Bind(UIEvent.LOGIN_PANEL_ACTIVE,UIEvent.RESTART);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.LOGIN_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            case UIEvent.RESTART:
                setPanelActive(true);
                restart = true;
                break;
            default:
                break;
        }
    }

    private void DeleteAllJson()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            DirectoryInfo direction = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".json"))
                {
                    string FilePath = Application.persistentDataPath + "/" + files[i].Name;
                    File.Delete(FilePath);
                }
            }
        }
    }

    private Button btnLogin;
    private Button btnClose;
    private InputField inputAccount;
    private InputField inputPassword;

    private PromptMsg promptMsg;

    // Use this for initialization
    void Start()
    {
        btnLogin = transform.Find("btnLogin").GetComponent<Button>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        inputAccount = transform.Find("inputAccount").GetComponent<InputField>();
        inputPassword = transform.Find("inputPassword").GetComponent<InputField>();

        btnLogin.onClick.AddListener(loginClick);
        btnClose.onClick.AddListener(closeClick);

        promptMsg = new PromptMsg();

        //面板需要默认隐藏
        setPanelActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnLogin.onClick.RemoveListener(loginClick);
        btnClose.onClick.RemoveListener(closeClick);
    }

    /// <summary>
    /// 登录按钮的点击事件处理
    /// </summary>
    private void loginClick()
    {
        if (string.IsNullOrEmpty(inputAccount.text))
        {
            promptMsg.Change("登录的用户名不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (string.IsNullOrEmpty(inputPassword.text))
        {
            promptMsg.Change("登录的密码不能为空！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }
        if (inputPassword.text.Length < 8 || inputPassword.text.Length > 16)
        {
            promptMsg.Change("登录的密码长度不合法，应该在8-16个字符之内！", Color.red);
            Dispatch(AreaCode.UI, UIEvent.PROMPT_MSG, promptMsg);
            return;
        }

        //发送用户的账户
        Player player=new Player() { Account = inputAccount.text, Password = inputPassword.text };

        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.player, EncodeObject(player));
        //需要重新创建角色
        if (restart)
        {
            DeleteAllJson();
            data.Add((byte)ParameterCode.Created, null);
        }
        OpCustom(OpCode.Login, data);
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
