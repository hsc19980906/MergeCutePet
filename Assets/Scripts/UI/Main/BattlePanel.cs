using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattlePanel : UIBase
{
    #region 获取面板上的UI组件
    public Text playerPetName;
    public Text enemyPetName;
    public Text petKind;
    public Text enemyKind;
    public Image petImg;
    public Image enemyImg;
    public GameObject playerPet;
    public GameObject enemyPet;
    public Text reduceHp1;
    public Text reduceHp2;
    public Text Hp1;
    public Text Hp2;
    public Text Mp1;
    public Text MapName;
    public Text Exp;
    public Text Coin;
    public Text Diamond;
    public Image imgRank;

    private CanvasGroup[] canvasGroup;
    private Slider[] sliders;
    #endregion

    #region 获取属性
    //private bool hasPet = false;
    private PetModel pet;//主战宠物
    private PetModel RealPet;
    private Enemy enemy;
    private int level;
    private int num;//记录是哪个种族
    private string[] petNames = new string[5];
    private string[] NewBase = { "金波姆", "绿波姆", "水波姆", "火波姆", "土波姆" };//新手基地
    private string[] Forest = { "波光姆" , "波碧姆", "波波姆", "波炎姆", "波纳姆" };//妖精森林
    private string[] Cliff = { "金波姆王", "绿波姆王", "水波姆王", "火波姆王", "土波姆王" };//潮汐海崖
    private string[] Lode = { "金光鼠", "蟾蜍", "芙蓉", "火芒", "魔岩卵" };//巨石山脉
    private string[] Ridge = { "雷光鼠", "老爷蛙", "冰石机", "吸血蚊", "土飞蚁" };//黄金陵
    private string[] Beach = { "雷炎鼠", "紫螟蛙", "水灵", "炎凤蛋", "蚂蚁守卫" };//炙热沙滩
    private string[] Volcano = { "光驹", "弹簧蛇", "水灵王", "小炎凤", "小蚁后" };//尤玛火山
    private string[] Desert = { "黄金独角兽", "化蛇王", "欲雨蝶", "紫炎凤", "土蚁后" };//死亡沙漠
    private string[] Mirage = { "小金驹", "紫木蝎", "水仙花", "火焰蝠", "玉兔儿" };//海市蜃楼
    private string[] Ice = { "圣洁独角兽", "花叶童子", "紫仙花", "小炎狐", "波姆兔" };//冰滩
    private string[] Sea = { "黄金蝙蝠", "花语兽", "舍利猫", "三尾狐", "波姆兔王" };//海底世界
    private string[] Christmas = { "紫貘", "青冥兽", "天使喵", "六尾狐", "月亮兔" };//圣诞小屋
    private string[] Eddy = { "盗宝鼠", "青椒", "小玄武", "九尾狐", "岩兽人" }; //黑漩涡
    private string[] Boss = { "冰波姆","黄金鸟", "黄金鸟族长", "嚣张的奶牛", "贪吃蛇", "葫芦猿", "恶魔波姆", "恶魔鸟",
        "梦魇", "火灵猴", "天使波姆", "战神兔", "月饼大仙" };
    private string initExpMsg = "离线经验收益:每分钟Exp+";
    private string initCoinMsg = "离线金币收益:每分钟金币+";
    private string initDiamondMsg = "离线钻石收益:每分钟钻石+";

    private double hp1;//宠物血量
    private double hp2;//敌人血量
    private double mp1;//宠物蓄能槽
    private float timer;
    private Vector3 petPos;
    private Vector3 enemyPos;
    #endregion

    /// <summary>
    /// 暂停指定方法 指定时间
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delaySeconds"></param>
    /// <returns></returns>
    public static IEnumerator DelayFuc(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }

    private void Awake()
    {
        Bind(UIEvent.BATTLE_PANEL_ACTIVE,UIEvent.BATTLE_PET_REFRESH,UIEvent.PET_REVIVE,UIEvent.TITLE_REFRESH);
        canvasGroup = GetComponentsInChildren<CanvasGroup>();
        sliders = GetComponentsInChildren<Slider>();//0,1为玩家hp,mp 2,3为对手hp,mp
    }

    private void Start()
    {
        canvasGroup[0].alpha = 0;
        canvasGroup[0].blocksRaycasts = false;
        timer = 0;
        imgRank.enabled = false;
        petPos = petImg.rectTransform.position;
        enemyPos = enemyImg.rectTransform.position;
        if (PetCharacter.Instance.state.map == Enemy.Map.None)
        {
            canvasGroup[1].alpha = 1;
            canvasGroup[1].blocksRaycasts = true;
            canvasGroup[2].alpha = 0;
            canvasGroup[2].blocksRaycasts = false;
        }
        else
        {
            canvasGroup[1].alpha = 0;
            canvasGroup[1].blocksRaycasts = false;
            canvasGroup[2].alpha = 1;
            canvasGroup[2].blocksRaycasts = true;
            SetMap(PetCharacter.Instance.state.map);
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.BATTLE_PANEL_ACTIVE:
                if((bool)message)
                {
                    canvasGroup[0].alpha = 1;
                    canvasGroup[0].blocksRaycasts = true;                   
                }
                else
                {
                    canvasGroup[0].alpha = 0;
                    canvasGroup[0].blocksRaycasts = false;
                }
                break;
            case UIEvent.BATTLE_PET_REFRESH:
                RealPet = (message as PetModel);
                pet = new PetModel(RealPet.ID, RealPet.Name, RealPet.Quality,
                    RealPet.Sprite, RealPet.petKind)
                {
                    Level= RealPet.Level,
                    skill= RealPet.skill,
                };
                pet.ChangeCC(RealPet.CC);
                if(RealPet.isDie)
                    Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "宠物已死亡！请用复活丹复活");
                hp1 = pet.hp;
                mp1 = 0;
                ChooseEnemy();
                break;
            case UIEvent.PET_REVIVE:
                //ChangeEnemy();
                RealPet.isDie = false;
                hp1 = pet.hp;
                mp1 = pet.mp;
                ChooseEnemy();
                break;
            case UIEvent.TITLE_REFRESH:
                if(pet!=null)
                    pet.WearTitle();
                switch (PlayerCharacter.Instance.player.rank.title)
                {
                    case Common.Model.PlayerRank.Title.first:
                        imgRank.sprite = Resources.Load<Sprite>("1");
                        imgRank.enabled = true;
                        break;
                    case Common.Model.PlayerRank.Title.second:
                        imgRank.sprite = Resources.Load<Sprite>("2");
                        imgRank.enabled = true;
                        break;
                    case Common.Model.PlayerRank.Title.third:
                        imgRank.sprite = Resources.Load<Sprite>("3");
                        imgRank.enabled = true;
                        break;
                    case Common.Model.PlayerRank.Title.beforeten:
                        imgRank.sprite = Resources.Load<Sprite>("10");
                        imgRank.enabled = true;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        // 有战斗宠物并且选了战斗地图 宠物还没死 就一直选敌人战斗
        if (RealPet != null && PetCharacter.Instance.state.map != Enemy.Map.None&&!RealPet.isDie)
        {
            BattleCircle();
        }
    }

    private void ChooseEnemy()
    {
        if (petNames[0] != null)
        {
            ////没有敌人 或 上一个敌人已经死了
            //if (enemy == null || enemy.isDie || map != oldmap)
            //{
            //    oldmap = map;
                int random = new System.Random().Next(0, 1000);
                //千分之一几率出boss
                if (random == 1000)
                    enemy = new Enemy(level, PetModel.PetKind.Boss, Boss[num], "Pet/Boss/" + num);
                else
                {
                    num = UnityEngine.Random.Range(0, 4);
                    enemy = new Enemy(level, (PetModel.PetKind)System.Enum.Parse(typeof(PetModel.PetKind), num.ToString()),
                        petNames[num], PetCharacter.Instance.GetPetSpriteByName(petNames[num]));
                }
                hp1 = pet.hp;
                hp2 = enemy.hp;
                UpdateUI();
            //}
        }
    }

    //战斗循环
    private void BattleCircle()
    {
        if (RealPet != null && enemy != null&& !RealPet.isDie && !enemy.isDie)
        {
            timer += Time.deltaTime;
            if (timer >= 4.0f)
            {
                Battle();
                timer = 0;
            }
        }
    }

    /// <summary>
    /// 设置地图怪物
    /// </summary>
    private void SetMap(Enemy.Map map)
    {
        switch (map)
        {
            case Enemy.Map.NewBase:
                level = 1;
                Array.Copy(NewBase, petNames, NewBase.Length);
                MapName.text = "新手基地";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Forest:
                level = 2;
                Array.Copy(Forest, petNames, Forest.Length);
                MapName.text = "妖精森林";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Cliff:
                level = 3;
                Array.Copy(Cliff, petNames, Cliff.Length);
                MapName.text = "潮汐海崖";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Lode:
                level = 4;
                Array.Copy(Lode, petNames, Lode.Length);
                MapName.text = "巨石山脉";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Ridge:
                level = 5;
                Array.Copy(Ridge, petNames, Ridge.Length);
                MapName.text = "黄金陵";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Beach:
                level = 6;
                Array.Copy(Beach, petNames, Beach.Length);
                MapName.text = "炽热沙滩";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Volcano:
                level = 7;
                Array.Copy(Volcano, petNames, Volcano.Length);
                MapName.text = "尤玛火山";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Desert:
                level = 8;
                Array.Copy(Desert, petNames, Desert.Length);
                MapName.text = "死亡沙漠";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Mirage:
                level = 9;
                Array.Copy(Mirage, petNames, Mirage.Length);
                MapName.text = "海市蜃楼";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Ice:
                level = 10;
                Array.Copy(Ice, petNames, Ice.Length);
                MapName.text = "冰滩";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Sea:
                level = 11;
                Array.Copy(Sea, petNames, Sea.Length);
                MapName.text = "海底世界";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Christmas:
                level = 12;
                Array.Copy(Christmas, petNames, Christmas.Length);
                MapName.text = "圣诞小屋";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            case Enemy.Map.Eddy:
                level = 13;
                Array.Copy(Eddy, petNames, Eddy.Length);
                MapName.text = "黑漩涡";
                Exp.text = initExpMsg + PetCharacter.Instance.SetExpByMap();
                Coin.text = initCoinMsg + PetCharacter.Instance.SetCoinByMap();
                Diamond.text = initDiamondMsg + PetCharacter.Instance.SetDiamondByMap();
                break;
            default:
                break;
        }
        PetCharacter.Instance.state.map = map;
    }

    //点击地图进入对应战斗地图
    public void EnterBattle()
    {
        if(RealPet != null)
        {
            if (!RealPet.isDie)
            {
                GameObject gameObject = EventSystem.current.currentSelectedGameObject;
                //Enum.GetName(, Convert.ToInt32(gameObject.name));  
                PetCharacter.Instance.state.map = (Enemy.Map)System.Enum.Parse(typeof(Enemy.Map), gameObject.name);
                SetMap(PetCharacter.Instance.state.map);
                //进入新地图要重新选怪物
                ChooseEnemy();
                canvasGroup[1].alpha = 0;
                canvasGroup[1].blocksRaycasts = false;
                canvasGroup[2].alpha = 1;
                canvasGroup[2].blocksRaycasts = true;
            }
            else
            {
                //【20220803】死亡需要反复提醒玩家
                //Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "无法继续战斗！请用复活丹复活宠物");
                Dispatch(AreaCode.UI, UIEvent.ITEM_MSG, new ItemMsg() { itemMsg = "无法继续战斗！\n请用复活丹复活宠物!", position = new Vector3(Screen.width / 2 - 250, Screen.height / 2) });
            }
        }

    }

    //选择离线挂机 并退出游戏
    public void ExitGame()
    {
        PetCharacter.Instance.state.offTime = DateTime.Now;
        //弹出提示框
        Dispatch(AreaCode.UI, UIEvent.EXIT_DIALOG_ACTIVE, true);
    }

    //更新战斗图片
    private void UpdateUI()
    {
        enemyPetName.text = enemy.petName + " Lv." + enemy.Level;
        playerPetName.text = pet.Name + " Lv." + pet.Level;
        petKind.text = pet.GetPetKind();
        enemyKind.text = enemy.GetPetKind();
        petImg.sprite = Resources.Load<Sprite>(pet.Sprite);
        enemyImg.sprite = Resources.Load<Sprite>(enemy.Sprite);
    }

    //返回地图
    public void BackMap()
    {
        canvasGroup[1].alpha = 1;
        canvasGroup[1].blocksRaycasts = true;
        canvasGroup[2].alpha = 0;
        canvasGroup[2].blocksRaycasts = false;
        PetCharacter.Instance.state.map = Enemy.Map.None;
        //Initial();
    }

    //切换宠物
    public void ChangeEnemy()
    {
        int random = new System.Random().Next(0, 10000);
        //万分之一几率出boss
        if (random == 10000)
            enemy = new Enemy(level, PetModel.PetKind.Boss, Boss[num], "Pet/Boss/" + num);
        else
        {
            num = UnityEngine.Random.Range(0, 4);
            enemy = new Enemy(level, (PetModel.PetKind)System.Enum.Parse(typeof(PetModel.PetKind), num.ToString()),
                petNames[num], PetCharacter.Instance.GetPetSpriteByName(petNames[num]));
        }
        hp1 = pet.hp;
        hp2 = enemy.hp;
        UpdateUI();
    }
    /// <summary>
    /// 战斗逻辑
    /// 1、直到敌人死亡或玩家宠物死亡结束战斗
    /// 2、比较双方速度 速度快先手
    /// 3、比较双方攻击力
    /// </summary>
    private void Battle()
    {
        if (enemy.sp>=pet.sp)
        {
            EnemyAttack();
            if (hp1 <= 0)
            {
                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "宠物已死亡！请用复活丹复活！");
                RealPet.isDie = true;
                StartCoroutine(DelayFuc(() => TextMoved(reduceHp2, "宠物已死亡!"), 0.5f));
                BackMap();
            }
            else
            {
                StartCoroutine(DelayFuc(() => PetAttack(), 1.5f));
                if (hp2 <= 0)
                {
                    enemy.isDie = true;
                    switch (PetCharacter.Instance.state.map)
                    {
                        case Enemy.Map.NewBase:
                            PetCharacter.Instance.state.KillNum1++;
                            break;
                        case Enemy.Map.Forest:
                            PetCharacter.Instance.state.KillNum2++;
                            break;
                        case Enemy.Map.Cliff:
                            PetCharacter.Instance.state.KillNum3++;
                            break;
                        case Enemy.Map.Lode:
                            PetCharacter.Instance.state.KillNum4++;
                            break;
                        case Enemy.Map.Ridge:
                            PetCharacter.Instance.state.KillNum5++;
                            break;
                        case Enemy.Map.Beach:
                            PetCharacter.Instance.state.KillNum6++;
                            break;
                        case Enemy.Map.Volcano:
                            PetCharacter.Instance.state.KillNum7++;
                            break;
                        case Enemy.Map.Desert:
                            PetCharacter.Instance.state.KillNum8++;
                            break;
                        case Enemy.Map.Mirage:
                            PetCharacter.Instance.state.KillNum9++;
                            break;
                        case Enemy.Map.Ice:
                            PetCharacter.Instance.state.KillNum10++;
                            break;
                        case Enemy.Map.Sea:
                            PetCharacter.Instance.state.KillNum11++;
                            break;
                        case Enemy.Map.Christmas:
                            PetCharacter.Instance.state.KillNum12++;
                            break;
                        case Enemy.Map.Eddy:
                            PetCharacter.Instance.state.KillNum13++;
                            break;
                        default:
                            break;
                    }
                    StartCoroutine(DelayFuc(() => TextMoved(reduceHp1, "敌人已死亡!"), 0.5f));
                    if (enemy.petKind == PetModel.PetKind.Boss)
                        VictorySpecialBenefit();
                    else
                       VictoryBenefit();
                    ChooseEnemy();
                }
            }
        }
        else
        {
            PetAttack();
            if (hp2 <= 0)
            {
                enemy.isDie = true;
                switch (PetCharacter.Instance.state.map)
                {
                    case Enemy.Map.NewBase:
                        PetCharacter.Instance.state.KillNum1++;
                        break;
                    case Enemy.Map.Forest:
                        PetCharacter.Instance.state.KillNum2++;
                        break;
                    case Enemy.Map.Cliff:
                        PetCharacter.Instance.state.KillNum3++;
                        break;
                    case Enemy.Map.Lode:
                        PetCharacter.Instance.state.KillNum4++;
                        break;
                    case Enemy.Map.Ridge:
                        PetCharacter.Instance.state.KillNum5++;
                        break;
                    case Enemy.Map.Beach:
                        PetCharacter.Instance.state.KillNum6++;
                        break;
                    case Enemy.Map.Volcano:
                        PetCharacter.Instance.state.KillNum7++;
                        break;
                    case Enemy.Map.Desert:
                        PetCharacter.Instance.state.KillNum8++;
                        break;
                    case Enemy.Map.Mirage:
                        PetCharacter.Instance.state.KillNum9++;
                        break;
                    case Enemy.Map.Ice:
                        PetCharacter.Instance.state.KillNum10++;
                        break;
                    case Enemy.Map.Sea:
                        PetCharacter.Instance.state.KillNum11++;
                        break;
                    case Enemy.Map.Christmas:
                        PetCharacter.Instance.state.KillNum12++;
                        break;
                    case Enemy.Map.Eddy:
                        PetCharacter.Instance.state.KillNum13++;
                        break;
                    default:
                        break;
                }
                StartCoroutine(DelayFuc(() => TextMoved(reduceHp1, "敌人已死亡!"), 0.5f));
                if (enemy.petKind == PetModel.PetKind.Boss)
                    VictorySpecialBenefit();
                else
                    VictoryBenefit();
                ChooseEnemy();
            }
            else
            {
                StartCoroutine(DelayFuc(() => EnemyAttack(), 1.5f));
                if (hp1 <= 0)
                {
                    Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "宠物已死亡！请用复活丹复活！");
                    RealPet.isDie = true;
                    StartCoroutine(DelayFuc(() => TextMoved(reduceHp2, "宠物已死亡!"), 0.5f));
                    BackMap();
                }
            }
        }
    }

    private void VictorySpecialBenefit()
    {
        int[] benefits = { 10, 11, 12, 16, 17, 18, 40, 41, 59, 60, 93, 94 };
        int id = benefits[UnityEngine.Random.Range(0, benefits.Length - 1)];
        Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  id  });
    }

    //战斗胜利 奖励道具
    private void VictoryBenefit()
    {
        PetCharacter.Instance.state.TotalExp += enemy.Add_Exp;
        PlayerCharacter.Instance.player.ChangeMoney(enemy.Add_Coin, enemy.Add_Diamond, 0);
        Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
        int id = 0;
        int random = new System.Random().Next(0, 1000);
        int[] nums1 = { 61, 64, 67, 72, 75, 78, 81, 84 };
        int[] nums2 = { 63, 66, 69, 70, 74, 77, 80, 83, 86 };
        int[] nums3 = { 62, 65, 68, 71, 73, 76, 79, 82, 85 };
        if (random > 1000)
        {
            if (random <= 9800)
            {
                switch (PetCharacter.Instance.state.map)
                {
                    case Enemy.Map.NewBase:
                    case Enemy.Map.Forest:
                    case Enemy.Map.Cliff:
                    case Enemy.Map.Lode:
                        id = new System.Random(DateTime.Now.Millisecond).Next(19, 24);
                        break;
                    case Enemy.Map.Ridge:
                    case Enemy.Map.Beach:
                    case Enemy.Map.Volcano:
                    case Enemy.Map.Desert:
                        id = new System.Random(DateTime.Now.Millisecond).Next(24, 29);
                        break;
                    case Enemy.Map.Mirage:
                    case Enemy.Map.Ice:
                    case Enemy.Map.Sea:
                        id = new System.Random(DateTime.Now.Millisecond).Next(29, 34);
                        break;
                    case Enemy.Map.Christmas:
                        id = new System.Random(DateTime.Now.Millisecond).Next(42, 60);
                        break;
                    case Enemy.Map.Eddy:
                        id = new System.Random(DateTime.Now.Millisecond).Next(2, 19);
                        break;
                    default:
                        break;
                }
            }
            if (random > 9800 && random <= 9900)
            {
                switch (PetCharacter.Instance.state.map)
                {
                    case Enemy.Map.Forest:
                    case Enemy.Map.Lode:
                        id = nums1[new System.Random(DateTime.Now.Millisecond).Next(nums1.Length)];
                        break;
                    case Enemy.Map.Beach:
                    case Enemy.Map.Desert:
                        id = nums2[new System.Random(DateTime.Now.Millisecond).Next(nums2.Length)];
                        break;
                    case Enemy.Map.Ice:
                        id = nums3[new System.Random(DateTime.Now.Millisecond).Next(nums3.Length)];
                        break;
                    default:
                        break;
                }
            }
            if (random > 9900 && random <= 9980)
                id = 60;
            if (random > 9980 && random <= 9999)
                id = 93;
            if (random == 10000)
                id = 94;
            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  id  });
        }
    }

    //敌人攻击力大于宠物防御力 则打出实际伤害 否则攻击伤害强制为1
    private void EnemyAttack()
    {
        reduceHp2.transform.position = new Vector2(270, 1170);
        enemyImg.transform.position = enemyPos;
        if (enemy.ack > pet.def)
        {
            hp1 -= (enemy.ack - pet.def);
            mp1 += pet.sp * 2;
            ImageMoved(enemyImg);
            TextMoved(reduceHp2, "普通攻击 -" + Math.Round((enemy.ack - pet.def),2).ToString());
        }
        else
        {
            hp1--;
            ImageMoved(enemyImg);
            TextMoved(reduceHp2, "普通攻击 -1");
        }
        UpdateSlider();
    }

    private void PetAttack()
    {
        reduceHp1.transform.position =  new Vector2(800, 1170);
        petImg.transform.position = petPos;
        if (pet.ack > enemy.def)
        {
            if (mp1 >= pet.mp)
            {
                mp1 = 0;
                if (pet.skill != null)
                {
                    double attack = pet.ack;
                    double defense = pet.def;
                    double hp = pet.hp;
                    pet.ReleaseSkill();
                    hp2 -= (pet.ack - enemy.def);
                    ImageMoved(petImg);
                    TextMoved(reduceHp1, pet.skill.Name+" -" + Math.Round((pet.ack - enemy.def),2).ToString());
                    pet.StopReleaseSkill(attack, defense, hp);
                }
                else
                {
                    hp2 -= (pet.ack - enemy.def);
                    ImageMoved(petImg);
                    TextMoved(reduceHp1, "普通攻击 -" + Math.Round((pet.ack - enemy.def),2).ToString());
                }
            }
            else
            {
                mp1 += pet.sp;
                hp2 -= (pet.ack - enemy.def);
                ImageMoved(petImg);
                TextMoved(reduceHp1, "普通攻击 -" + Math.Round((pet.ack - enemy.def),2).ToString());
            }
        }
        else
        {
            hp2--;
            ImageMoved(petImg);
            TextMoved(reduceHp1, "普通攻击 -1");
        }
        UpdateSlider();
    }

    private void ImageMoved(Image image)
    {
        
        Transform transform = image.transform;
        //设置一个DOTween队列
        Sequence imgMoveSequence = DOTween.Sequence();

        Tweener imgMove01;
        Tweener imgMove02;
        //print(transform.position);
        if (image.rectTransform.position == petPos)
        {
            imgMove01 = transform.DOMoveX(petPos.x + 100, 0.3f);
            //playerImg.transform.position = playerPet.transform.position;
            imgMove02 = transform.DOMoveX(petPos.x, 0.3f);
        }
        else
        {
            imgMove01 = transform.DOMoveX(enemyPos.x - 100, 0.3f);
            //enemyImg.transform.position = enemyPet.transform.position;
            imgMove02 = transform.DOMoveX(enemyPos.x, 0.3f);
        }

        imgMoveSequence.Append(imgMove01);
        imgMoveSequence.AppendInterval(1);
        imgMoveSequence.Join(imgMove02);
    }

    //减血效果文字显示 和消失
    private void TextMoved(Text reduceHp,string hp)
    {
        //获得Text的rectTransform，和颜色，并设置颜色微透明
        RectTransform rect = reduceHp.rectTransform;

        Color color = reduceHp.color;
        reduceHp.color = new Color(color.r, color.g, color.b, 0);
        reduceHp.GetComponent<Text>().text = hp;

        //设置一个DOTween队列
        Sequence textMoveSequence = DOTween.Sequence();

        //设置Text移动和透明度的变化值
        Tweener textMove01 = rect.DOMoveY(rect.position.y + 50, 0.3f);
        Tweener textMove02 = rect.DOMoveY(rect.position.y + 100, 0.3f);
        //Tweener textMove03 = rect.DOMoveY(rect.position.y - 100, 0.5f);
        Tweener textColor01 = reduceHp.DOColor(new Color(color.r, color.g, color.b, 1), 0.3f);
        Tweener textColor02 = reduceHp.DOColor(new Color(color.r, color.g, color.b, 0), 0.3f);

        //Append 追加一个队列，Join 添加一个队列
        //中间间隔一秒
        //Append 再追加一个队列，再Join 添加一个队列
        textMoveSequence.Append(textMove01);
        textMoveSequence.Join(textColor01);
        textMoveSequence.AppendInterval(1);
        textMoveSequence.Append(textMove02);
        textMoveSequence.Join(textColor02);
    }

    //更新血条蓄能槽 以及 血量、蓄能显示
    private void UpdateSlider()
    {
        Hp1.text = hp1.ToString("f1") + "/" + pet.hp.ToString("f1");
        Mp1.text = mp1.ToString("f1") + "/" + pet.mp.ToString("f1");
        Hp2.text = hp2.ToString("f1") + "/" + enemy.hp.ToString("f1");

        sliders[0].DOValue((float)(hp1 / pet.hp), 0.3f);
        sliders[1].DOValue((float)(mp1 / pet.mp), 0.3f);
        sliders[2].DOValue((float)(hp2 / enemy.hp), 0.3f);
    }
}
