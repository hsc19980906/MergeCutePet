using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using Common.Model;
using System.Linq;
using Common;

public class PlayerCharacter : CharacterBase
{
    public static PlayerCharacter Instance = null;
    private string path;
    public PlayerModel player;

    private int day;//记录游戏天数
    private int hour;//记录游戏小时数
    private int minute;//记录游戏分钟数
    private float timer = 0.0f;//计时器

    private void Awake()
    {
        Instance = this;
        Bind(CharacterEvent.SAVE_PLAYER, CharacterEvent.LOAD_PLAYER,CharacterEvent.SAVE_FIRST);
        path = Application.persistentDataPath;
        player = new PlayerModel();
    }

    private void Start()
    {
        InitPlayer(path + "/Player.json");
    }

    private void InitPlayer(string path)
    {
        if (!File.Exists(path))
        {
            Dispatch(AreaCode.UI, UIEvent.REGISTED, false);
            File.Create(path).Dispose();
        }
        else
        {
            string json = File.ReadAllText(path);
            if (json != "")
                player = JsonConvert.DeserializeObject<PlayerModel>(json);
            Dispatch(AreaCode.UI, UIEvent.REGISTED, true);
        }
    }

    /// <summary>
    /// 宠物战斗力 上线更新一次 在线半小时更新一次
    /// 如何比较？ 比较ranchpets和bagpets中宠物的战斗力 最强的
    /// </summary>
    public void CountMaxCE()
    {
        PetModel pet = null;
        double max = 0;
        for (int i = 0; i < PetCharacter.Instance.bagPets.Count; i++)
        {
            if (PetCharacter.Instance.bagPets[i].CE > max)
            {
                pet = PetCharacter.Instance.bagPets[i];
                max = pet.CE;
            }
        }
        for (int i = 0; i < PetCharacter.Instance.ranchPets.Count; i++)
        {
            if (PetCharacter.Instance.ranchPets[i].CE > max)
            {
                pet = PetCharacter.Instance.ranchPets[i];
                max = pet.CE;
            }
        }
        if(pet!=null)
        {
            player.rank.Set(pet.CE, pet.Sprite, player.profile, pet.Name, pet.Level, player.PlayerName);
            Dictionary<byte, object> data = new Dictionary<byte, object>();
            data.Add((byte)ParameterCode.player, EncodeObject(PlayerCharacter.Instance.player.rank));
            OpCustom(OpCode.Update, data);
        }
    }

    //获取玩家游戏时间
    private void Update()
    {
        timer += Time.deltaTime;
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.SAVE_FIRST:
                SaveFirst(message as Player);
                break;
            case CharacterEvent.LOAD_PLAYER:
                LoadPlayer(path + "/Player.json");
                break;
            case CharacterEvent.SAVE_PLAYER:
                SavePlayer(message as PlayerModel, path + "/Player.json");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="initMsg"></param>
    private void SaveFirst(Player player_first)
    {
        player.Initial(player_first);
        SavePlayer(player, path + "/Player.json");
        LoadPlayer(path + "/Player.json");
    }

    private void LoadPlayer(string path)
    {
        LoadSceneMsg msg = new LoadSceneMsg(1,
        delegate ()
        {
            Dispatch(AreaCode.UI, UIEvent.OFFTIME_BENEFIR_COUNT, null);
            Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_SIMPLE, player);
            //读入宠物数据
            Dispatch(AreaCode.CHARACTER, CharacterEvent.START_GAME, null);
            //获取当前联网状态
            if (NetManager.peer.PeerState == ExitGames.Client.Photon.PeerStateValue.Connected)
                PetCharacter.Instance.state.isOnline = true;
            else
                PetCharacter.Instance.state.isOnline = false;
        });
        Dispatch(AreaCode.SCENE, SceneEvent.LOAD_SCENE, msg);
    }

    private void SavePlayer(PlayerModel player,string path)
    {
        day = (int)timer / 86400;
        hour = ((int)timer - day * 86400) / 3600;
        minute = ((int)timer - day * 86400 - hour * 3600) / 60;
        player.ChangeTime(day, hour, minute);
        string json = JsonConvert.SerializeObject(player);
        json = Regex.Unescape(json);

        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }
        if (File.Exists(path))
        {
            File.WriteAllText(path, json, Encoding.UTF8);
        }

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        SavePlayer(player, path + "/Player.json");
    }
}
