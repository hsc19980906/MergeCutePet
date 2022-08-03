using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Common.Model;
using Common;
using Common.Tool;

public class ChoosePanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.CHOOSE_PANEL_ACTIVE);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.CHOOSE_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 获取UI
    /// </summary>
    private Sprite[] Players;
    private InputField InputName;
    private Toggle[] toggles;
    private Button Right1;
    private Button Right2;
    private Image Profile;
    private Image imgPet;
    private Button btnConfirm;
    private Text Kind;
    private int id1 =0;//决定player第几张
    private int id2 = 0;//决定pet第几张
    private bool isMale = true;
    private string[] Kinds = { "金", "木", "水", "火", "土" };
    private string[] petSprites = { "Pet/Gold/0", "Pet/Wood/1", "Pet/Water/2", "Pet/Fire/3", "Pet/Soil/4" };
    private double cc;//初始随机生成宠物成长值 介于1-1.5之间
    private string attris;

    private string petname;

    private void Start()
    {
        InputName = transform.Find("InputName").GetComponent<InputField>();
        Right1 = transform.Find("ChooseProfile/Right").GetComponent<Button>();
        Right2 = transform.Find("ChoosePet/Right").GetComponent<Button>();
        Profile = transform.Find("ChooseProfile/Profile").GetComponent<Image>();
        imgPet = transform.Find("ChoosePet/Pet").GetComponent<Image>();
        btnConfirm = transform.Find("btnConfirm").GetComponent<Button>();
        Kind = transform.Find("ChoosePet/Kind").GetComponent<Text>();
        Players = Resources.LoadAll<Sprite>("Player/Men");

        Right1.onClick.AddListener(rightClick1);
        Right2.onClick.AddListener(rightClick2);
        btnConfirm.onClick.AddListener(comfirmClick);
        //找到所有的toggles
        toggles = transform.GetComponentsInChildren<Toggle>();
        //给toggle添加事件
        for (int i = 0; i < toggles.Length; i++)
        {
            //这一步是必须记录的，用来区分那个toggle
            int K = i;
            toggles[K].onValueChanged.AddListener((bool value) => SetEveryToggle(value, K));
        }

        setPanelActive(false);
        Profile.sprite = Players[id1];
        imgPet.sprite = Resources.Load<Sprite>(petSprites[id2]);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        Right1.onClick.RemoveAllListeners();
        Right2.onClick.RemoveAllListeners();
        btnConfirm.onClick.RemoveAllListeners();
        toggles[0].onValueChanged.RemoveAllListeners();
        toggles[1].onValueChanged.RemoveAllListeners();
    }

    private void comfirmClick()
    {
        //显然这里需要和服务器交互

        //传值到服务器
        Player player = new Player()
        {
            id_player=PlayerCharacter.Instance.player.id,
            PlayerName = InputName.text,
            Sex = isMale ? "男" : "女",
            Profile = isMale ? "Player/Men/" + id1 : "Player/Women/" + id1
        };

        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.player, EncodeObject(player));
        data.Add((byte)ParameterCode.pet, Kind.text+"波姆");
        OpCustom(OpCode.Create, data);
    }

    private void rightClick1()
    {
        id1 = ++id1 % Players.Length;
        Profile.sprite = Players[id1];
    }


    private void rightClick2()
    {        
        imgPet.sprite = Resources.Load<Sprite>(petSprites[++id2 % petSprites.Length]);
        Kind.text = Kinds[id2 % petSprites.Length];
    }


    private void SetEveryToggle(bool value, int k)
    {
        id1 = 0;
        if (k == 0 && value)
        {
            //"Player/Men"
            Players = Resources.LoadAll<Sprite>("Player/Men");
            Profile.sprite = Players[0];
            isMale = true;
        }
        if (k == 1 && value)
        {
            //"Player/Women"
            Players = Resources.LoadAll<Sprite>("Player/Women");
            Profile.sprite = Players[0];
            isMale = false;
        }
    }
}
