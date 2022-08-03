using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 宠物背包
/// </summary>
public class PetPanel : UIBase
{
    #region 获取面板组件和属性
    public Text[] texts;
    public Image imgPet;
    public Text TotalExp;
    public GameObject Main;

    public Button btnSetMain;
    public Button btnPutRanch;

    private string path;
    private CanvasGroup[] canvasGroup;//1是装备 2是宠物信息 0是整个宠物背包面板
    private BagPetSlot[] slots;
    private List<PetModel> bagPets;
    private BagPetSlot currentSlot;
    private PetModel pet;
    private EquipmentSlot[] equipmentSlots;
    //TODO 双击装备可卸下
    private EquipmentSlot currentequipmentSlot;
    private List<int> equipments;
    private Equipment equipment;
    private bool ishasMain = false;
    private Vector2 imgPetPos;
    #endregion

    private void Awake()
    {
        path = Application.persistentDataPath;
        equipments = new List<int>();
        Bind(UIEvent.PET_PANEL_ACTIVE,UIEvent.PET_BAG_REFRESH,UIEvent.PET_EQUIP_REFRESH,UIEvent.LEARN_SKILL_BOOK);
        slots = GetComponentsInChildren<BagPetSlot>();

        equipmentSlots = GetComponentsInChildren<EquipmentSlot>();
    }

    public void Start()
    {
        Dispatch(AreaCode.CHARACTER, CharacterEvent.PET_BAG_REFRESH, null);
        canvasGroup = GetComponentsInChildren<CanvasGroup>();

        canvasGroup[0].alpha = 0;
        canvasGroup[0].blocksRaycasts = false;
        canvasGroup[1].alpha = 0;
        canvasGroup[1].blocksRaycasts = false;
        btnSetMain.onClick.AddListener(SetMain);
        btnPutRanch.onClick.AddListener(PutRanch);

        ParseEquipJson();
        imgPetPos = imgPet.rectTransform.position;
    }

    #region UI显示
    public void PetEquip()
    {
        canvasGroup[2].alpha = 0;
        canvasGroup[2].blocksRaycasts = false;
        canvasGroup[1].alpha = 1;
        canvasGroup[1].blocksRaycasts = true;
        imgPet.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        //imgPet.rectTransform.position = new Vector2(540, 1045);
    }

    public void BackPetInfo()
    {
        TotalExp.text = "经验池：" + PetCharacter.Instance.state.TotalExp.ToString();
        canvasGroup[2].alpha = 1;
        canvasGroup[2].blocksRaycasts = true;
        canvasGroup[1].alpha = 0;
        canvasGroup[1].blocksRaycasts = false;
        imgPet.rectTransform.position = imgPetPos;
    }
    //更新宠物属性显示
    private void UpdataUI()
    {
        if(pet!=null)
        {
            if (pet.isMain)
                Main.SetActive(true);
            else
                Main.SetActive(false);
            texts[0].text = "等级：" + pet.Level;
            texts[1].text = "血量：" + pet.hp;
            texts[2].text = "蓄能槽：" + pet.mp;
            texts[3].text = "攻击力：" + pet.ack;
            texts[4].text = "防御力：" + pet.def;
            texts[5].text = "速度：" + pet.sp;
            texts[6].text = "成长：" + pet.CC;
            texts[7].text = "升级还需经验：" + pet.Exp_Up;
            texts[8].text = "战斗力：" + pet.CE;
            imgPet.sprite = Resources.Load<Sprite>(pet.Sprite);
        }
    }

    #endregion

    private void Update()
    {
        WhichSlotChoosed();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PET_PANEL_ACTIVE:
                if ((bool)message)
                {
                    TotalExp.text ="经验池："+ PetCharacter.Instance.state.TotalExp.ToString();
                    canvasGroup[0].alpha = 1;
                    canvasGroup[0].blocksRaycasts = true;
                }
                else
                {
                    canvasGroup[0].alpha = 0;
                    canvasGroup[0].blocksRaycasts = false;
                }
                break;
            case UIEvent.PET_BAG_REFRESH:
                currentSlot = null;
                bagPets = message as List<PetModel>;
                CleanUpSlots();
                if (bagPets!=null)
                {
                    for (int i = 0; i < bagPets.Count; i++)
                    {
                        slots[i].StorePet(bagPets[i]);
                    }
                }
                foreach (BagPetSlot slot in slots)
                {
                    if (slot.GetPet() != null && slot.GetPet().isMain)
                    {
                        ishasMain = true;
                        pet = slot.GetPet();
                        Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
                        UpdataUI();
                    }
                }
                if(!ishasMain)
                {
                    if (slots[0].transform.childCount > 0)
                    {
                        pet = slots[0].GetPet();
                        pet.isMain = true;
                        ishasMain = true;
                        Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
                        UpdataUI();
                    }
                }
                break;
            case UIEvent.PET_EQUIP_REFRESH:
                currentequipmentSlot = null;
                equipments.Add((int)message);
                equipment = InventoryManager.Instance.GetItemByID<Equipment>((int)message);
                StoreEquip(equipment);
                pet.WearEquip(equipment);
                break;
            case UIEvent.LEARN_SKILL_BOOK:
                SkillBook skillBook = InventoryManager.Instance.GetItemByID<SkillBook>((int)message);
                pet.LearnSkill(skillBook);
                if(pet.isMain)
                    Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
                break;
            default:
                break;
        }
    }

    private void CleanUpSlots()
    {
        foreach (BagPetSlot slot in slots)
        {
            slot.RemovePet();
        }
    }

    #region 对宠物背包进行操作
    private void PutRanch()
    {
        if (currentSlot != null && currentSlot.GetPet() != null)
        {
            PetModel pet = currentSlot.GetPet();
            if(!pet.isMain)
            {
                pet.isCarry = !pet.isCarry;
                Destroy(currentSlot.transform.GetChild(0).gameObject);
                Dispatch(AreaCode.CHARACTER, CharacterEvent.REFRESH_PET, pet);
            }
            else
            {
                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "战斗宠物不可直接放入背包！");
            }
        }
    }

    private void SetMain()
    {
        if (currentSlot != null && currentSlot.GetPet() != null)
        {
            foreach (BagPetSlot slot in slots)
            {
                if (slot.GetPet() != null)
                    slot.GetPet().isMain = false;
            }
            pet = currentSlot.GetPet();
            pet.isMain = true;
            Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
            UpdataUI();
        }
    }

    //此时战斗模块也可能在修改TotalExp 需要注意数据上锁（不需要 unity是单线程）
    //还有一键升级功能需要完善（1、长按连续升级2、点击升顶3、选择等级阶段40、60等）
    //最高升到130级
    private void LevelUp()
    {
        PetCharacter.Instance.state.TotalExp -= pet.Exp_Up;
        pet.LevelUp();
        TotalExp.text = "经验池：" + PetCharacter.Instance.state.TotalExp.ToString();
        UpdataUI();
        if (pet.isMain)
            Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
    }

    public void UpOne()
    {
        if(pet != null && pet.Exp_Up <= PetCharacter.Instance.state.TotalExp && pet.Level < 130)
         LevelUp();
    }

    public void Up40()
    {
        while (pet != null&& pet.Exp_Up <= PetCharacter.Instance.state.TotalExp && pet.Level < 40 )
            LevelUp();
    }

    public void Up60()
    {
        while (pet != null&& pet.Exp_Up <= PetCharacter.Instance.state.TotalExp && pet.Level < 60 )
            LevelUp();
    }

    public void UpMax()
    {
        while (pet != null&& pet.Exp_Up <= PetCharacter.Instance.state.TotalExp && pet.Level < 130 )
            LevelUp();
    }
    #endregion

    // 装备着装有问题 猜测和之前的宠物背包问题差不多(已解决) 添加一键穿装、一键脱装
    #region 对宠物装备进行操作

    public void TakeOffAllEquip()
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //有装备就卸下
            if (equipmentSlots[i].transform.childCount > 0)
            {
                int id = equipmentSlots[i].GetEquipId();
                equipmentSlots[i].TakeOffEquip(id);
                pet.TakeOffEquip(InventoryManager.Instance.GetItemByID<Equipment>(id));
                equipments.Remove(id);
                UpdataUI();
            }            
        }
    }

    /// <summary>
    /// 1、在背包使用 向宠物面板发消息 要显示装备
    /// 2、每次从Equip.json读入装备信息，退出保存
    /// </summary>
    private void ParseEquipJson()
    {
        if (!File.Exists(path + "/Equip.json"))
        {
            File.Create(path + "/Equip.json").Dispose();
        }
        if (File.Exists(path + "/Equip.json"))
        {
            string json = File.ReadAllText(path + "/Equip.json");
            if (json != "")
            {
                equipments = JsonConvert.DeserializeObject<List<int>>(json);
            }
        }
        //读入之后给宠物装备上
        if(equipments.Count>0)
        {
            for (int i = 0; i < equipments.Count; i++)
            {
                //获取装备
                //print(equipments[i]);
                //equipment = InventoryManager.Instance.GetItemByID(equipments[i]) as Equipment;
                equipment = InventoryManager.Instance.GetItemByID<Equipment>(equipments[i]);
                StoreEquip(equipment);
                pet.WearEquip(equipment);
                UpdataUI();
            }
        }
    }

    private void SaveEquip()
    {
        if (!File.Exists(path + "/Equip.json"))
        {
            File.Create(path + "/Equip.json").Dispose();
        }
        string json = JsonConvert.SerializeObject(equipments);
        json = Regex.Unescape(json);

        if (File.Exists(path + "/Equip.json"))
        {
            File.WriteAllText(path + "/Equip.json", json, Encoding.UTF8);
        }
    }
    
    private bool StoreEquip(Equipment equipment)
    {
        EquipmentSlot slot = FindEquipSlot(equipment);
        if (slot == null)
        {
            Debug.LogWarning("没有物品槽");
            return false;
        }
        else
        {
            slot.StoreEquip(equipment);
            return true;
        }
    }

    /// <summary>
    /// 找到装备槽 如果是空的 直接返回该槽 
    /// 不是空的 先卸下 然后再返回该槽
    /// </summary>
    /// <param name="equipment"></param>
    /// <returns></returns>
    private EquipmentSlot FindEquipSlot(Equipment equipment)
    {
        //遍历所有的装备槽
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            //该装备槽是 且玩家想要替换同类型的装备槽 对其进行操作 不是则继续看下一个槽
            if (equipmentSlots[i].equipType == equipment.Equipmentype)
            {
                //有装备则卸下
                if (equipmentSlots[i].transform.childCount > 0)
                {
                    int id = equipmentSlots[i].GetEquipId();
                    equipmentSlots[i].TakeOffEquip(id);
                    pet.TakeOffEquip(InventoryManager.Instance.GetItemByID<Equipment>(id));
                    equipments.Remove(id);
                    UpdataUI();
                }
                return equipmentSlots[i];
            }
        }
        return null;
    }
    #endregion
    private void WhichSlotChoosed()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;
        if (gameObject != null)
        {
            if (gameObject.GetComponent<BagPetSlot>() != null)
            {
                currentSlot = gameObject.GetComponent<BagPetSlot>();
                pet = currentSlot.GetPet();
                UpdataUI();
            }
            if(gameObject.GetComponent<EquipmentSlot>()!=null)
            {
                currentequipmentSlot = gameObject.GetComponent<EquipmentSlot>();
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        SaveEquip();
    }

}
