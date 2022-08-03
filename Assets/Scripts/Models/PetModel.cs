using System;
using UnityEngine;

public class PetModel
{
    public int ID;
    public string Name;
    public PetQuality Quality;
    public string Sprite;
    public int id_pet;
    public int Level = 1;
    public int Exp_Up;//升级所需经验数
    public double CC;
    public bool isCarry = false;//是在宠物栏还是牧场中 默认在牧场
    public bool isDie = false;//宠物是否死亡 死亡需要复活药复活
    public bool isMain = false;//是否是主战 主战需要去战斗

    public PetKind petKind;

    public double ack = 200;//实际攻击力
    public double def = 20;//实际防御力
    public double mp = 2000;
    public double hp = 2000;
    public double sp = 50;

    public double CE = 0;

    public Skill skill;

    /// <summary>
    /// Pet构造函数
    /// </summary>
    /// <param name="attris">依次为：攻击力、防御力、蓄能、血量、速度，这里传入的是属性成长</param>
    public PetModel(int id, string name, PetQuality quality, string sprite, PetKind petKind)
    {
        ID = id;
        Name = name;
        Quality = quality;
        Sprite = sprite;
        this.petKind = petKind;
    }

    //根据宠物品质确定成长值
    public void GetCCByQuality()
    {
        int max = 150, min = 100;
        switch (Quality)
        {
            case PetQuality.One:
                min *= 1;//1
                max *= 1;//1.5
                break;
            case PetQuality.Two:
                min *= 2;//1
                max *= 2;//1.5
                break;
            case PetQuality.Three:
                min *= 3;//1
                max *= 3;//1.5
                break;
            case PetQuality.Four:
                min *= 4;//1
                max *= 4;//1.5
                break;
            case PetQuality.Five:
                min *= 5;//1
                max *= 5;//1.5
                break;
            case PetQuality.Six:
                min *= 6;//1
                max *= 6;//1.5
                break;
            case PetQuality.Seven:
                min *= 7;//1
                max *= 7;//1.5
                break;
            case PetQuality.Eight:
                min *= 8;//1
                max *= 8;//1.5
                break;
            case PetQuality.Nine:
                min *= 9;//1
                max *= 9;//1.5
                break;
            case PetQuality.Ten:
                min *= 10;//1
                max *= 10;//1.5
                break;
            case PetQuality.Eleven:
                min *= 11;//1
                max *= 11;//1.5
                break;
            case PetQuality.Twelve:
                min *= 12;//1
                max *= 12;//1.5
                break;
            case PetQuality.Thirteen:
                min *= 13;//1
                max *= 13;//1.5
                break;
            case PetQuality.Dragon:
                min *= 14;//1
                max *= 14;//1.5
                break;
            case PetQuality.God:
                min *= 20;//1
                max *= 20;//1.5
                break;
            default:
                break;
        }
        CC = new System.Random(GetRandomSeed()).Next(min, max) * 0.01;
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

    //根据种族确定属性成长
    public double[] ChangeAttrisByKind()
    {
        switch (petKind)
        {
            case PetKind.Gold:
                return new double[] { 15, 10, 0.1, 50, 5 };
            case PetKind.Wood:
                return (new double[] { 15, 10, 0.1, 48, 6 });
            case PetKind.Water:
                return (new double[] { 15, 10, 0.15, 48, 5 });
            case PetKind.Fire:
                return (new double[] { 16, 9, 0.15, 48, 4 });
            case PetKind.Soil:
                return (new double[] { 14, 11, 0.1, 50, 4 });
            case PetKind.Boss:
                return (new double[] { 16, 11, 0.15, 50, 6 });
            case PetKind.God:
                return (new double[] { 20, 15, 0.3, 60, 10 });
            default:
                return new double[] { };
        }
    }

    //根据属性成长、等级、成长值cc确定宠物具体属性
    public void ChangeAttris()
    {
        double[] attris = ChangeAttrisByKind();
        ack = Math.Round((200 + attris[0] * CC * Level),2);
        def = Math.Round((20 + attris[1] * CC * Level),2);
        if (mp > 1)
        {
            mp = Math.Round((2000 - attris[2] * CC * Level), 2);
            if (mp <= 1)
                mp = 1;
        }
        hp = Math.Round((2000 + attris[3] * CC * Level),2);
        sp = Math.Round((50 + attris[4] * CC * Level),2);
        GetUpExp();
        UpdateCE();
    }

    public void UpdateCE()
    {
        CE = Math.Round(((ack + def + hp + sp) / mp) * CC * Level, 2);
    }

    private void ChangeAttris(double Attack,double Defense,double Hp,double Mp,double Speed,bool isDivi)
    {
        if(isDivi)
        {
            ack = Math.Round(ack / Attack,2);
            def = Math.Round(def / Defense,2);
            if (mp > 1)
            {
                mp = Math.Round(mp * Mp, 2);
                if (mp <= 1)
                    mp = 1;
            }
            hp = Math.Round(hp / Hp,2);
            sp = Math.Round(sp / Speed,2);
        }
        else
        {
            ack = Math.Round(ack * Attack, 2);
            def = Math.Round(def * Defense, 2);
            if (mp > 1)
            {
                mp = Math.Round(mp / Mp, 2);
                if (mp <= 1)
                    mp = 1;
            }
            hp = Math.Round(hp * Hp, 2);
            sp = Math.Round(sp * Speed, 2);
        }
        UpdateCE();
    }

    //穿上装备属性增长
    public void WearEquip(Equipment equipment)
    {
        if (equipment.Attack < 1)
            ack = Math.Round(ack * (1 + equipment.Attack),2);
        else
            ack += equipment.Attack;
        if (equipment.Defense < 1)
            def = Math.Round(def * (1 + equipment.Defense), 2);
        else
            def += equipment.Defense;
        if (equipment.Hp < 1)
            hp = Math.Round(hp * (1 + equipment.Hp), 2);
        else
            hp += equipment.Hp;
        if (equipment.Mp < 1)
        {
            mp = Math.Round(mp / (1 + equipment.Mp), 2);
            if (mp <= 1)
                mp = 1;
        }
        else
            mp -= equipment.Mp;
        if (equipment.Speed < 1)
            sp = Math.Round(sp * (1 + equipment.Speed), 2);
        else
            sp += equipment.Speed;
        UpdateCE();
    }

    //脱下装备属性恢复
    public void TakeOffEquip(Equipment equipment)
    {
        if (equipment.Attack < 1)
            ack = Math.Round(ack / (1 + equipment.Attack), 2);
        else
            ack -= equipment.Attack;
        if (equipment.Defense < 1)
            def = Math.Round(def / (1 + equipment.Defense), 2);
        else
            def -= equipment.Defense;
        if (equipment.Hp < 1)
            hp = Math.Round(hp / (1 + equipment.Hp), 2);
        else
            hp -= equipment.Hp;
        if (equipment.Mp < 1)
        {
            mp = Math.Round(mp * (1 + equipment.Mp), 2);
            if (mp <= 1)
                mp = 1;
        }
        else
            mp += equipment.Mp;
        if (equipment.Speed < 1)
            sp = Math.Round(sp / (1 + equipment.Speed), 2);
        else
            sp -= equipment.Speed;
        UpdateCE();
    }

    public void LearnSkill(SkillBook skillbook)
    {
        skill = new Skill
        {
            Attack = skillbook.Attack,
            Defense = skillbook.Defense,
            Hp=skillbook.Hp,
            Name = skillbook.Name
        };
    }

    public void ReleaseSkill()
    {
        if (skill.Attack < 0)
            ack = Math.Round(ack * (1 + skill.Attack),2);
        else
            ack += skill.Attack;
        if (skill.Defense < 0)
            def = Math.Round(def * (1 + skill.Defense),2);
        else
            def += skill.Defense;
        if (skill.Hp < 0)
            hp = Math.Round(hp * (1 + skill.Hp),2);
        else
            hp += skill.Hp;
    }

    public void StopReleaseSkill(double atk,double def,double hp)
    {
        this.ack = atk;
        this.def = def;
        this.hp = hp;
    }

    public void WearTitle()
    {
        switch (PlayerCharacter.Instance.player.rank.title)
        {
            case Common.Model.PlayerRank.Title.first:
                ChangeAttris(2f, 2f, 2f, 2f, 2f, false);
                break;
            case Common.Model.PlayerRank.Title.second:
                ChangeAttris(1.8f, 1.8f, 1.8f, 1.8f, 1.8f, false);
                break;
            case Common.Model.PlayerRank.Title.third:
                ChangeAttris(1.5f, 1.5f, 1.5f, 1.5f, 1.5f, false);
                break;
            case Common.Model.PlayerRank.Title.beforeten:
                ChangeAttris(1.1f, 1.1f, 1.1f, 1.1f, 1.1f, false);
                break;
            default:
                break;
        }
    }

    public void TakeOffTitle()
    {
        switch (PlayerCharacter.Instance.player.rank.title)
        {
            case Common.Model.PlayerRank.Title.first:
                ChangeAttris(2f, 2f, 2f, 2f, 2f,true);
                break;
            case Common.Model.PlayerRank.Title.second:
                ChangeAttris(1.8f, 1.8f, 1.8f, 1.8f, 1.8f, true);
                break;
            case Common.Model.PlayerRank.Title.third:
                ChangeAttris(1.5f, 1.5f, 1.5f, 1.5f, 1.5f, true);
                break;
            case Common.Model.PlayerRank.Title.beforeten:
                ChangeAttris(1.1f, 1.1f, 1.1f, 1.1f, 1.1f, true);
                break;
            default:
                break;
        }
    }

    //升级 提升属性
    public void LevelUp()
    {
        Level++;
        ChangeAttris();     
    }

    public void ChangeCC(double cc)
    {
        CC = Math.Round(cc, 2);
        ChangeAttris();
    }

    public void Evolute(bool isA)
    {
        if(isA)
        {
            ID += 5;
            Sprite = PetCharacter.Instance.GetPetSpriteByID(ID);
            Debug.Log(Sprite);
            Name = PetCharacter.Instance.GetPetNameByID(ID);
            Debug.Log(Name);
            Quality++;
            ChangeCC(CC + new System.Random().Next(100, 300) * 0.01);
        }
        else
        {
            ID += 10;
            Sprite = PetCharacter.Instance.GetPetSpriteByID(ID);
            Name = PetCharacter.Instance.GetPetNameByID(ID);
            Quality += 2;
            ChangeCC(CC + new System.Random().Next(200, 400) * 0.01);
        }
    }

    /// <summary>
    /// 合成公式
    /// 1、主副宠CC不同范围抽不同比例 
    /// 2、等级范围不同抽不同比例
    /// 3、道具加成
    /// </summary>
    /// <param name="pet">副宠</param>
    /// <param name="itemID1"></param>
    /// <param name="itemID2"></param>
    public void Merge(PetModel pet,int itemID1, int itemID2)
    {
        float percent1 = CountMergePercentByCC(CC, true) + CountMergePercentByLevel(Level, true);
        float percent2 = CountMergePercentByCC(pet.CC, false) + CountMergePercentByLevel(pet.Level, false);
        float percent3 = CountMergePercentByitemID(itemID1);
        ChangeCC((CC * percent1 + pet.CC * percent2) * percent3);
        ChangeMergeByItem(itemID2);
    }

    private void ChangeMergeByItem(int itemID)
    {
        if (itemID == 57)
            ChangeCC(CC * 1.03);
        else if (itemID == 58)
            ChangeCC(CC * 1.05);
        else if (itemID == 59)
            ChangeCC(CC * 1.1);
        else
        {
            float[] attris = CountAttrisAddByitemID(itemID);
            ChangeAttris(attris[0], attris[1], attris[2], attris[3], attris[4], false);
        }
    }

    //根据主副宠的cc判断抽取cc的范围
    private float CountMergePercentByCC(double cc,bool isMain)
    {
        if (!isMain)
            return 0.1f;
        if (cc >= 1 && cc < 5)
            if (isMain)
                return 1.5f;//1.5-7.5
            //else
            //    return 0.3f;
        if (cc >= 5 && cc < 10)
            if (isMain)
                return 1.1f;//5.5-11
            //else
            //    return 0.25f;
        if (cc >= 10 && cc < 20)
            if (isMain)
                return 1.05f;//10.5-21
            //else
            //    return 0.2f;
        if (cc >= 20 && cc < 40)
            if (isMain)
                return 1.02f;//20.4-40.8
            //else
            //    return 0.15f;
        if (cc >= 40 && cc < 60)
            if (isMain)
                return 1.01f;//40.4-60.6
            //else
            //    return 0.1f;
        if (cc >= 60 && cc < 100)
            if (isMain)
                return 1.005f;//60.3-100.5
            //else
            //    return 0.06f;
        return 1f;
    }

    private float CountMergePercentByLevel(int level,bool isMain)
    {
        if (level >= 40 && level < 60)
            if (isMain)
                return 0;
            else
                return 0.03f;
        if (level >= 60 && level < 90)
            if (isMain)
                return 0.02f;
            else
                return 0.06f;
        if (level >= 90 && level < 100)
            if (isMain)
                return 0.07f;
            else
                return 0.1f;
        if (level >= 100 && level < 120)
            if (isMain)
                return 0.07f;
            else
                return 0.08f;
        if (level >= 120 && level < 130)
            if (isMain)
                return 0.09f;
            else
                return 0.1f;
        return 0;
    }

    private float CountMergePercentByitemID(int itemID)
    {
        if (itemID == 37)
            return 1f;
        if (itemID == 38)
            return 1.05f;
        if (itemID == 39)
            return 1.12f;
        if (itemID == 40)
            return 1.2f;
        if (itemID == 41)
            return 1.1f;
        return 1f;
    }

    private float[] CountAttrisAddByitemID(int itemID)
    {
        float[] attris = new float[] { 1f, 1f, 1f, 1f, 1f };
        if (itemID == 42)
            attris[2] = 1.1f;
        if (itemID == 43)
            attris[2] = 1.2f;
        if (itemID == 44)
            attris[2] = 1.3f;
        if (itemID == 45)
            attris[3] = 1.1f;
        if (itemID == 46)
            attris[3] = 1.2f;
        if (itemID == 47)
            attris[3] = 1.3f;
        if (itemID == 48)
            attris[0] = 1.1f;
        if (itemID == 49)
            attris[0] = 1.2f;
        if (itemID == 50)
            attris[0] = 1.3f;
        if (itemID == 51)
            attris[1] = 1.1f;
        if (itemID == 52)
            attris[1] = 1.2f;
        if (itemID == 53)
            attris[1] = 1.3f;
        if (itemID == 54)
            attris[4] = 1.1f;
        if (itemID == 55)
            attris[4] = 1.2f;
        if (itemID == 56)
            attris[4] = 1.3f;
        return attris;
    }

    //计算升级所需经验
    public void GetUpExp()
    {
        Exp_Up = Level * (Level + 5 + Level / 2) * 100;
    }

    //将枚举类型的宠物种类解析成string
    public string GetPetKind()
    {
        switch (petKind)
        {
            case PetKind.Gold:
                return "金";
            case PetKind.Wood:
                return "木";
            case PetKind.Water:
                return "水";
            case PetKind.Fire:
                return "火";
            case PetKind.Soil:
                return "土";
            default:
                return "你的宠物什么都不是";
        }
    }


    /// <summary> 
    /// 得到提示面板应该显示什么样的内容
    /// </summary>
    /// <returns></returns>
    public string GetToolTipText()
    {
        string colorKind = "";
        string petkind = GetPetKind();
        switch (petKind)
        {
            case PetKind.Gold:
                colorKind = "yellow";
                break;
            case PetKind.Wood:
                colorKind = "green";
                break;
            case PetKind.Water:
                colorKind = "blue";
                break;
            case PetKind.Fire:
                colorKind = "red";
                break;
            case PetKind.Soil:
                colorKind = "brown";
                break;
            default:
                break;
        }
        string text = string.Format("<size=50>{0}\n<color={8}>种族：{6}</color>\n<color=green>等级：{1} </color>\n" +
            "血量：{2}\n蓄能槽：{3}\n攻击力：{4}\n防御力：{5}\n速度：{7}\n<color=orange>成长：{9}</color></size>",
            Name, Level, hp, mp, ack, def, petkind, sp, colorKind, CC);
        return text;
    }

    //宠物种族
    public enum PetKind
    {
        Gold,
        Wood,
        Water,
        Fire,
        Soil,
        Boss,
        God
    }

    //宠物品阶
    public enum PetQuality
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Eleven,
        Twelve,
        Thirteen,
        Dragon,
        God
    }
}

public class Skill
{
    public string Name;
    public double Attack;
    public double Defense;
    public double Hp;
}

