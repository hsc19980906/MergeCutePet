using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using static Enemy;

public class PetCharacter : CharacterBase
{
    public static PetCharacter Instance = null;
    public List<PetModel> pets;
    public List<PetModel> bagPets;//宠物面板中的宠物 最多三只 战斗、合成用
    public List<PetModel> ranchPets;//牧场中的宠物 获得宠物蛋 使用后放在牧场 可携带

    public State state;
    private string path;
    private PetModel pet;

    private void Awake()
    {
        Instance = this;
        path = Application.persistentDataPath;
        //print(path);
        if (!Directory.Exists((path)))
        {
            Directory.CreateDirectory(path);
        }

        state = new State();
        pets = new List<PetModel>();
        ranchPets = new List<PetModel>();
        bagPets = new List<PetModel>();
        //enemies = new List<Enemy>();

        //print("解析文件");
        ParsePetJson();//读入所有宠物基础属性


        Bind(CharacterEvent.REFRESH_PET, CharacterEvent.RANCH_REFRESH,CharacterEvent.PET_BAG_REFRESH,
            CharacterEvent.ADD_PET_BY_NAME,CharacterEvent.SAVE_MAP,CharacterEvent.START_GAME);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case CharacterEvent.ADD_PET_BY_NAME:
                AddPetByName(message as string);
                break;
            case CharacterEvent.REFRESH_PET:
                RefreshPet(message as PetModel);
                break;
            case CharacterEvent.RANCH_REFRESH:
                Dispatch(AreaCode.UI, UIEvent.RANCH_REFRESH, ranchPets);
                break;
            case CharacterEvent.PET_BAG_REFRESH:
                Dispatch(AreaCode.UI, UIEvent.PET_BAG_REFRESH, bagPets);
                break;
            case CharacterEvent.SAVE_MAP:
                state.map = (Enemy.Map)message;
                break;
            case CharacterEvent.START_GAME:
                ParseStateJson();//读入一些全局状态量 玩家的宠物总数：作宠物id
                ParsePetBagRanchJson();//读入玩家宠物 分别放在宠物背包和宠物牧场
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 根据名字 向背包和牧场添加宠物
    /// </summary>
    /// <param name="petname"></param>
    private void AddPetByName(string petname)
    {
        pet = GetPetByName(petname);
        state.PetCount++;
        pet.id_pet = state.PetCount;
        if(pet.id_pet==1)
        {
            pet.isCarry = true;
        }
        if (pet.isCarry)
            bagPets.Add(pet);
        else
            ranchPets.Add(pet);
        PlayerCharacter.Instance.CountMaxCE();
    }
    
    public void AddPetRandom(int id)
    {
        GetPetRandom(id);
        PetModel randomPet = new PetModel(pet.ID,pet.Name, pet.Quality, pet.Sprite, pet.petKind);
        state.PetCount++;
        randomPet.id_pet = state.PetCount;
        randomPet.GetCCByQuality();
        randomPet.ChangeAttris();
        ranchPets.Add(randomPet);
    }

    //移动背包和牧场中的宠物 重新加载界面
    private void RefreshPet(PetModel pet)
    {
        if (pet.isCarry)
        {
            if (ranchPets.Contains(pet))
                ranchPets.Remove(pet);
            bagPets.Add(pet);
        }
        else
        {
            if (bagPets.Contains(pet))
                bagPets.Remove(pet);
            ranchPets.Add(pet);
        }
        Dispatch(AreaCode.UI, UIEvent.RANCH_REFRESH, ranchPets);
        Dispatch(AreaCode.UI, UIEvent.PET_BAG_REFRESH, bagPets);
    }

    //解析敌人 一开始读入只读基础属性
    //private void ParseEnemyJson(string jsonName)
    //{
    //    TextAsset petText = Resources.Load<TextAsset>(jsonName);
    //    string petsJson = petText.text;
    //    JArray array = JArray.Parse(petsJson);
    //    foreach (var temp in array)
    //    {
    //        string name = (string)temp["petName"];
    //        string sprite = (string)temp["Sprite"];
    //        PetModel.PetKind petKind = (PetModel.PetKind)System.Enum.Parse(typeof(PetModel.PetKind), (string)temp["petKind"]);
    //        int level = (int)temp["Level"];
    //        enemy = new Enemy(level, petKind,name,sprite);
    //        enemies.Add(enemy);
    //    }
    //}

    private void ParseStateJson()
    {
        if (!File.Exists(path + "/State.json"))
        {
            File.Create(path + "/State.json").Dispose();
        }
        if (File.Exists(path + "/State.json"))
        {
            string json = File.ReadAllText(path + "/State.json");
            if (json != "")
            {
                state = JsonConvert.DeserializeObject<State>(json);
            }
        }
    }

    private void ParsePetJson()
    {
        TextAsset petText = Resources.Load<TextAsset>("Pet");
        string petsJson = petText.text;
        JArray array = JArray.Parse(petsJson);
        foreach (var temp in array)
        {
            int id = (int)(temp["id"]);
            string name = (string)temp["Name"];
            PetModel.PetQuality quality = (PetModel.PetQuality)System.Enum.Parse(typeof(PetModel.PetQuality), (string)temp["Quality"]);
            string sprite = (string)temp["Sprite"];
            PetModel.PetKind petKind = (PetModel.PetKind)System.Enum.Parse(typeof(PetModel.PetKind), (string)temp["PetKind"]);
            pet = new PetModel(id, name, quality, sprite, petKind);
            pet.GetCCByQuality();
            pet.ChangeAttris();
            pets.Add(pet);
        }
    }

    private void ParsePetBagRanchJson()
    {
        if (!File.Exists(path + "/Pet_Bag.json"))
        {
            File.Create(path + "/Pet_Bag.json").Dispose();
        }
        if (!File.Exists(path + "/Pet_Ranch.json"))
        {
            File.Create(path + "/Pet_Ranch.json").Dispose();
        }
        if (File.Exists(path + "/Pet_Bag.json"))
        {
            string jsonbag = File.ReadAllText(path + "/Pet_Bag.json");
            if (jsonbag != "")
                bagPets = JsonConvert.DeserializeObject<List<PetModel>>(jsonbag);
            string jsonranch = File.ReadAllText(path + "/Pet_Ranch.json");
            if (jsonranch != "")
                ranchPets = JsonConvert.DeserializeObject<List<PetModel>>(jsonranch);
        }
    }

    //保存宠物背包和牧场中的宠物
    private void SavePet()
    {
        if (!File.Exists(path + "/Pet_Bag.json"))
        {
            File.Create(path + "/Pet_Bag.json").Dispose();
        }
        if (!File.Exists(path + "/Pet_Ranch.json"))
        {
            File.Create(path + "/Pet_Ranch.json").Dispose();
        }
        string json_bag= JsonConvert.SerializeObject(bagPets);
        string json_ranch = JsonConvert.SerializeObject(ranchPets);
        json_bag = Regex.Unescape(json_bag);
        json_ranch = Regex.Unescape(json_ranch);
 
        if (File.Exists(path + "/Pet_Bag.json"))
        {
            File.WriteAllText(path + "/Pet_Bag.json", json_bag, Encoding.UTF8);
        }

        if (File.Exists(path + "/Pet_Ranch.json"))
        {
            File.WriteAllText(path + "/Pet_Ranch.json", json_ranch, Encoding.UTF8);
        }
    }

    public PetModel GetPetByName(string petname)
    {
        foreach (PetModel pet in pets)
        {
            if (pet.Name == petname)
                return pet;
        }
        return null;
    }

    public PetModel GetPetByID(int id)
    {
        foreach (PetModel pet in pets)
        {
            if (pet.ID == id)
                return pet;
        }
        return null;
    }

    public string GetPetSpriteByID(int id)
    {
        if (id <= pets.Count)
            return pets[id].Sprite;
        else
            return "";
    }

    public string GetPetSpriteByName(string name)
    {
        foreach (PetModel pet in pets)
        {
            if (pet.Name == name)
                return pet.Sprite;
        }
        return "";
    }

    public string GetPetNameByID(int id)
    {
        if (id <= pets.Count)
            return pets[id].Name;
        else
            return "";
    }

    public void GetPetRandom(int id)
    {
        if (id == 60)
            pet = pets[new System.Random(GetRandomSeed()).Next(0, 5)];
        if (id == 93)
            pet = pets[new System.Random(GetRandomSeed()).Next(65, 70)];
        if (id == 94)
            pet = pets[new System.Random(GetRandomSeed()).Next(70, 76)];
    }

    /// <summary>
    /// 使用RNGCryptoServiceProvider生成种子
    /// </summary>
    /// <returns></returns>
    static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);

    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        SavePet();
        state.offTime = DateTime.Now;
        SaveState();
    }

    //根据地图获取每只怪的经验值
    public int SetExpByMap()
    {
        switch (state.map)
        {
            case Enemy.Map.NewBase:
                return 200;
            case Enemy.Map.Forest:
                return 600;
            case Enemy.Map.Cliff:
                return 1000;
            case Enemy.Map.Lode:
                return 1400;
            case Enemy.Map.Ridge:
                return 1800;
            case Enemy.Map.Beach:
                return 2200;
            case Enemy.Map.Volcano:
                return 2600;
            case Enemy.Map.Desert:
                return 3000;
            case Enemy.Map.Mirage:
                return 3500;
            case Enemy.Map.Ice:
                return 4000;
            case Enemy.Map.Sea:
                return 4500;
            case Enemy.Map.Christmas:
                return 5000;
            case Enemy.Map.Eddy:
                return 6000;
            default:
                return 0;
        }
    }

    //根据地图获取每只怪的经验值
    public int SetCoinByMap()
    {
        switch (state.map)
        {
            case Enemy.Map.NewBase:
                return 65;
            case Enemy.Map.Forest:
                return 200;
            case Enemy.Map.Cliff:
                return 340;
            case Enemy.Map.Lode:
                return 470;
            case Enemy.Map.Ridge:
                return 600;
            case Enemy.Map.Beach:
                return 740;
            case Enemy.Map.Volcano:
                return 870;
            case Enemy.Map.Desert:
                return 910;
            case Enemy.Map.Mirage:
                return 1040;
            case Enemy.Map.Ice:
                return 1180;
            case Enemy.Map.Sea:
                return 1310;
            case Enemy.Map.Christmas:
                return 1500;
            case Enemy.Map.Eddy:
                return 1800;
            default:
                return 0;
        }
    }

    public int SetDiamondByMap()
    {
        switch (state.map)
        {
            case Enemy.Map.NewBase:
                return 6;
            case Enemy.Map.Forest:
                return 20;
            case Enemy.Map.Cliff:
                return 34;
            case Enemy.Map.Lode:
                return 47;
            case Enemy.Map.Ridge:
                return 60;
            case Enemy.Map.Beach:
                return 74;
            case Enemy.Map.Volcano:
                return 87;
            case Enemy.Map.Desert:
                return 91;
            case Enemy.Map.Mirage:
                return 104;
            case Enemy.Map.Ice:
                return 118;
            case Enemy.Map.Sea:
                return 131;
            case Enemy.Map.Christmas:
                return 150;
            case Enemy.Map.Eddy:
                return 180;
            default:
                return 0;
        }
    }

    private void SaveState()
    {
        if (!File.Exists(path + "/State.json"))
        {
            File.Create(path + "/State.json").Dispose();
        }
        string json = JsonConvert.SerializeObject(state);
        json = Regex.Unescape(json);

        if (File.Exists(path + "/State.json"))
        {
            File.WriteAllText(path + "/State.json", json, Encoding.UTF8);
        }
    }
}
