using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO 可以加一个批量售出的功能
public class BagPanel :  Inventory
{
    public InputField inputSellAmount;
    public Button btnSell;
    public InputField inputUseAmount;
    public Button btnUse;
    private Slot currentSlot;//获取当前选中的物品槽

    private int price;
    private List<BagItem> bagItems;
    private List<BagItem> mergeItems;
    private BagItem bagItem;
    private BagItem item;
    private bool isUse = true;
    private PetModel pet;
    List<int> taskPets;

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.BAG_PANEL_ACTIVE, UIEvent.BAG_ADD_REFRESH, UIEvent.TASK_FINISH, UIEvent.BAG_REMOVE_REFRESH,
          UIEvent.EVOLUTEA_FINISH, UIEvent.EVOLUTEB_FINISH, UIEvent.GET_MERGE_ITEM, UIEvent.BATTLE_PET_REFRESH);
        bagItems = new List<BagItem>();
        mergeItems = new List<BagItem>();
        taskPets = new List<int>();

        inputUseAmount.text = "1";//默认使用个数为1
    }

    //获取当前被选中的物品槽
    private void Update()
    {
        WhichSlotChoosed();
    }

    private void WhichSlotChoosed()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;
        if (gameObject != null)
        {
            if (gameObject.GetComponent<Slot>() != null)
            {
                currentSlot = gameObject.GetComponent<Slot>();
            }
        }
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.BAG_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            case UIEvent.BAG_ADD_REFRESH:
                bagItem = message as BagItem;
                //背包中没有该物体就添加 有就增加数量
                if (FindSameItem(bagItem.ItemId) == null)
                    bagItems.Add(bagItem);
                else
                    FindSameItem(bagItem.ItemId).Amount += bagItem.Amount;
                StoreItem(bagItem.ItemId, bagItem.Amount);
                break;
            case UIEvent.BAG_REMOVE_REFRESH:
                bagItem = message as BagItem;
                //看背包中有没有该物体 有则找到存放该物体的物品槽 然后进行移除
                item = FindSameItem(bagItem.ItemId);
                if (item != null)
                {
                    RemoveItem(FindSlotByItemID(bagItem.ItemId), bagItem.Amount);
                    if (item.Amount > bagItem.Amount)
                        item.Amount -= bagItem.Amount;
                    else
                        bagItems.Remove(item);
                }
                break;
            case UIEvent.TASK_FINISH:
                Task task = message as Task;
                if(task.Requires!=null&& task.Requires.Count>0)
                foreach (Task.RequireType requireType in task.Requires.Keys)
                {
                    switch (requireType)
                    {
                        case Task.RequireType.NewBase:
                            if (PetCharacter.Instance.state.KillNum1 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Forest:
                            if (PetCharacter.Instance.state.KillNum2 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Cliff:
                            if (PetCharacter.Instance.state.KillNum3 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Lode:
                            if (PetCharacter.Instance.state.KillNum4 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Ridge:
                            if (PetCharacter.Instance.state.KillNum5 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Beach:
                            if (PetCharacter.Instance.state.KillNum6 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Volcano:
                            if (PetCharacter.Instance.state.KillNum7 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Desert:
                            if (PetCharacter.Instance.state.KillNum8 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Mirage:
                            if (PetCharacter.Instance.state.KillNum9 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Ice:
                            if (PetCharacter.Instance.state.KillNum10 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Sea:
                            if (PetCharacter.Instance.state.KillNum11 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Christmas:
                            if (PetCharacter.Instance.state.KillNum12 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Eddy:
                            if (PetCharacter.Instance.state.KillNum13 >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Item:
                            foreach (int id in task.Requires[requireType].Keys)
                            {
                                item = FindSameItem(id);
                                if (item != null && item.Amount >= task.Requires[requireType][id])
                                    task.Finished = true;
                                else
                                    task.Finished = false;
                            }
                            break;
                        case Task.RequireType.Pet:
                                //TODO 任务二要求一只波姆 任务四要求一只二阶 任务六要求一只波姆和一只二阶 想不到办法
                            if(task.id==2)
                                foreach (PetModel pet in PetCharacter.Instance.bagPets)
                                {
                                    if (pet.ID >= 0 && pet.ID <= 4)
                                    {
                                        taskPets.Add(pet.Level);
                                        break;
                                    }
                                }
                            if (task.id == 4)
                                foreach (PetModel pet in PetCharacter.Instance.bagPets)
                                {
                                    if (pet.ID >= 5 && pet.ID <= 9)
                                    {
                                        taskPets.Add(pet.Level);
                                        break;
                                    }
                                }
                            if (task.id == 6)
                                {
                                    if(PetCharacter.Instance.bagPets.Count>= task.Requires[requireType].Keys.Count)
                                        foreach (PetModel pet in PetCharacter.Instance.bagPets)
                                        {
                                            if (pet.ID >= 5 && pet.ID <= 9)
                                            {
                                                taskPets.Add(pet.Level);
                                                break;
                                            }
                                        }
                                }
                                break;
                        case Task.RequireType.Level:
                            foreach (int level in taskPets)
                            {
                                if(level>= task.Requires[requireType][-1])
                                    task.Finished = true;
                                else
                                    task.Finished = false;
                            }
                            taskPets.Clear();
                            break;
                        case Task.RequireType.Merge:
                            if (PetCharacter.Instance.state.MergeNum >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Coin:
                            if (PlayerCharacter.Instance.player.Coin >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        case Task.RequireType.Diamond:
                            if (PlayerCharacter.Instance.player.Diamond >= task.Requires[requireType][-1])
                                task.Finished = true;
                            else
                                task.Finished = false;
                            break;
                        default:
                            break;
                    }
                }
                else
                    Dispatch(AreaCode.UI, UIEvent.MAIN_TASK_FINISH_TIME, task);
                break;
            case UIEvent.EVOLUTEA_FINISH:
                item = FindSameItem((int)message);
                if (item != null)
                {
                    Dispatch(AreaCode.UI, UIEvent.BAG_REMOVE_REFRESH, new BagItem() { ItemId = (int)message, Amount = 1 });
                    Dispatch(AreaCode.UI, UIEvent.EVOLUTEA, true);
                }
                else
                    Dispatch(AreaCode.UI, UIEvent.EVOLUTEA, false);
                break;
            case UIEvent.EVOLUTEB_FINISH:
                item = FindSameItem((int)message);
                if (item != null)
                {
                    Dispatch(AreaCode.UI, UIEvent.BAG_REMOVE_REFRESH, new BagItem() { ItemId = (int)message , Amount = 1 });
                    Dispatch(AreaCode.UI, UIEvent.EVOLUTEB, null);
                }
                else
                    Dispatch(AreaCode.UI, UIEvent.EVOLUTEB, false);
                break;
            case UIEvent.GET_MERGE_ITEM:
                mergeItems.Clear();
                foreach (BagItem item in bagItems)
                {
                    if (item.ItemId >= 37 && item.ItemId <= 59)
                        mergeItems.Add(item);
                }
                Dispatch(AreaCode.UI, UIEvent.MERGE_ITEM_REFRESH, mergeItems);
                break;
            case UIEvent.BATTLE_PET_REFRESH:
                pet = message as PetModel;
                break;
            default:
                break;
        }
    }

    //整理背包有问题！！ 目前看来是装备的问题 每次刷新装备都会多加一次(已解决)
    public void PackBag()
    {
        CleanUpSlots();
        InitBag();
    }

    private void InitBag()
    {
        bagItems.Sort((t1, t2) => t1.ItemId.CompareTo(t2.ItemId));
        for (int i = 0; i < bagItems.Count; i++)
        {
            StoreItem(bagItems[i].ItemId, bagItems[i].Amount);
        }
        //foreach (BagItem item in bagItems)
        //{
        //    StoreItem(item.id, item.Amount);
        //}
    }

    private void CleanUpSlots()
    {
        foreach (Slot slot in slots)
        {
            slot.RemoveItem();
        }
    }

    private BagItem FindSameItem(int id)
    {
        foreach (BagItem item in bagItems)
        {
            if (id == item.ItemId)
                return item;
        }
        return null;
    }

    public void Start()
    {
        ParseBagJson();
        InitBag();
        setPanelActive(false);
        btnSell.onClick.AddListener(SellItem);
        btnUse.onClick.AddListener(UseItem);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        SaveBag();
    }

    private void ParseBagJson()
    {
        if (!File.Exists(Application.persistentDataPath + "/Bag.json"))
        {
            File.Create(Application.persistentDataPath + "/Bag.json").Dispose();
        }
        if (File.Exists(Application.persistentDataPath + "/Bag.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/Bag.json");
            if (json != "")
            {
                bagItems = JsonConvert.DeserializeObject<List<BagItem>>(json);
            }
        }
    }

    private void SellItem()
    {
        if (currentSlot != null && currentSlot.transform.childCount > 0)
        {
            CheckSell();
        }
    }

    private void CheckSell()
    {
        PlayerModel player = PlayerCharacter.Instance.player;
        if (!string.IsNullOrEmpty(inputSellAmount.text))
        {
            if (int.TryParse(inputSellAmount.text, out int Amount))
            {
                price = currentSlot.GetItemSellPrice();
                switch (currentSlot.GetItemSellMoney())
                {
                    case Item.ItemMoney.Coin:
                        if (Amount  >= currentSlot.GetItemAmount())
                        {
                            Amount = currentSlot.GetItemAmount();
                            inputSellAmount.text = Amount.ToString();
                        }
                        player.ChangeMoney(Amount * price, 0, 0);
                        break;
                    case Item.ItemMoney.Diamond:
                        if (Amount >= currentSlot.GetItemAmount())
                        {
                            Amount = currentSlot.GetItemAmount();
                            inputSellAmount.text = Amount.ToString();
                        }
                        player.ChangeMoney(0, Amount * price, 0);
                        break;
                    case Item.ItemMoney.Gold:
                        if (Amount >= currentSlot.GetItemAmount())
                        {
                            Amount = currentSlot.GetItemAmount();
                            inputSellAmount.text = Amount.ToString();
                        }
                        player.ChangeMoney(0, 0, Amount * price);
                        break;
                    default:
                        break;
                }
                Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_SIMPLE, player);
                RemoveItem(currentSlot,Amount);

                BagItem bagItem = FindSameItem(currentSlot.GetItemID());
                bagItem.Amount -= Amount;
                if (bagItem.Amount == 0)
                    bagItems.Remove(bagItem);
            }
        }
    }

    private void UseItem()
    {
        if (currentSlot != null && currentSlot.transform.childCount > 0)
        {
            CheckUse();
        }
    }

    private void CheckUse()
    {
        if (!string.IsNullOrEmpty(inputUseAmount.text))
        {
            if (int.TryParse(inputUseAmount.text, out int Amount))
            {
                if(Amount>=currentSlot.GetItemAmount())
                {
                    Amount = currentSlot.GetItemAmount();
                    inputUseAmount.text = Amount.ToString();
                }
                switch (currentSlot.GetItemType())
                {
                    case Item.ItemType.Consumable:
                        if(currentSlot.GetItemID() == 1)
                        {
                            Dispatch(AreaCode.UI, UIEvent.PET_REVIVE, null);
                            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "复活丹使用成功！\n宠物已复活！请继续战斗！");
                            isUse = true;
                            break;
                        }
                        if(currentSlot.GetItemID()>=2&& currentSlot.GetItemID()<=12)
                        {
                            long Exp = 0;
                            if (currentSlot.GetItemID() == 2)
                                Exp = new System.Random(DateTime.Now.Millisecond).Next(100, 10000);
                            if (currentSlot.GetItemID() == 3)
                                Exp = 100000;
                            if (currentSlot.GetItemID() == 4)
                                Exp = 250000;
                            if (currentSlot.GetItemID() == 5)
                                Exp = 500000;
                            if (currentSlot.GetItemID() == 6)
                                Exp = 1000000;
                            if (currentSlot.GetItemID() == 7)
                                Exp = 1250000;
                            if (currentSlot.GetItemID() == 8)
                                Exp = 2500000;
                            if (currentSlot.GetItemID() == 9)
                                Exp = 5000000;
                            if (currentSlot.GetItemID() == 10)
                                Exp = 100000000;
                            if (currentSlot.GetItemID() == 11)
                                Exp = 1000000000;
                            if (currentSlot.GetItemID() == 12)
                                Exp = 35000000000;
                            PetCharacter.Instance.state.TotalExp += Exp;
                            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "使用" + currentSlot.GetItemName() + "，经验池Exp+" + Exp);
                            isUse = true;
                            break;
                        }
                        if(currentSlot.GetItemID() >= 13 && currentSlot.GetItemID() <= 15)
                        {
                            float ExpBuff = 1f;
                            if (currentSlot.GetItemID() == 13)
                                ExpBuff = 1.5f;
                            if (currentSlot.GetItemID() == 14)
                                ExpBuff = 2f;
                            if (currentSlot.GetItemID() == 15)
                                ExpBuff = 3f;
                            //TODO 经验卷轴换算
                            if (PetCharacter.Instance.state.ExpBuff >= ExpBuff)
                                PetCharacter.Instance.state.ExpBuffMinutes += 3600;
                            PetCharacter.Instance.state.ExpBuff = ExpBuff;
                            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "使用" + currentSlot.GetItemName() + "，离线经验收益加成" + ExpBuff+"倍"+
                                "\n注意低倍下使用高倍无效！");
                            isUse = true;
                            break;
                        }
                        if (pet != null && pet.Level >= 1)
                            if (currentSlot.GetItemID() == 88)
                            {
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  89  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 10, ItemId =  1  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 10, ItemId =  3  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 2, ItemId =  7  });
                                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "使用" + currentSlot.GetItemName() + ",获得复活丹x10,经验药水（小）x10," +
                                    "经验箱（小）x2,新手20级大礼包x1");
                                isUse = true;
                                break;
                            }
                        if (pet != null && pet.Level >= 20)
                        { 
                            if (currentSlot.GetItemID() == 89)
                            {
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  90  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  87  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 10, ItemId =  4  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 2, ItemId =  8  });
                                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "使用" + currentSlot.GetItemName() + ",获得加强攻击x1,经验药水（中）x10," +
                                    "经验箱（中）x2,新手40级大礼包x1");
                                isUse = true;
                                break;
                            }
                        }
                        else
                            isUse = false;
                        if (pet != null && pet.Level >= 40)
                        { 
                            if (currentSlot.GetItemID() == 90)
                            {
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  91 });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  60  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 10, ItemId =  5  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 2, ItemId =  9  });
                                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "使用" + currentSlot.GetItemName() + ",获得波姆蛋x1,经验药水（大）x10," +
                                    "经验箱（大）x2,新手60级大礼包x1");
                                isUse = true;
                                break;
                            }
                        }
                        else
                                isUse = false;
                        if (pet != null && pet.Level >= 60)
                        { 
                            if (currentSlot.GetItemID() == 91)
                            {
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  93  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 10, ItemId =  5  });
                                Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 2, ItemId =  9  });
                                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "使用" + currentSlot.GetItemName() + ",获得五系龙蛋x1,经验药水（特大）x10,");
                                isUse = true;
                                break;
                            }
                        }
                        else
                                isUse = false;
                        if (currentSlot.GetItemID() == 92)
                        {
                            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  61  });
                            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  64  });
                            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  67  });
                            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  72  });
                            isUse = true;
                        }                          
                        break;
                    case Item.ItemType.Equipment:
                        Dispatch(AreaCode.UI, UIEvent.PET_EQUIP_REFRESH, currentSlot.GetItemID());
                        isUse = true;
                        break;
                    case Item.ItemType.Egg:
                        for (int i = 0; i < Amount; i++)
                        {
                            PetCharacter.Instance.AddPetRandom(currentSlot.GetItemID());
                        }
                        isUse = true;
                        break;
                    case Item.ItemType.Material:
                        isUse = false;
                        break;
                    case Item.ItemType.SkillBook:
                        isUse = true;
                        Dispatch(AreaCode.UI, UIEvent.LEARN_SKILL_BOOK, currentSlot.GetItemID());
                        break;
                    default:
                        break;
                }
                if(isUse)
                {
                    BagItem bagItem = FindSameItem(currentSlot.GetItemID());
                    RemoveItem(currentSlot, Amount);
                    bagItem.Amount -= Amount;
                    if (bagItem.Amount == 0)
                        bagItems.Remove(bagItem);
                }
            }
        }
    }

    private void SaveBag()
    {
        string json = JsonConvert.SerializeObject(bagItems);
        if (File.Exists(Application.persistentDataPath + "/Bag.json"))
        {
            File.WriteAllText(Application.persistentDataPath + "/Bag.json", json, Encoding.UTF8);
        }
    }
}
