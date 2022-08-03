
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO 进化结果显示图片要修改 合成、进化逻辑要完善 还有涅槃功能之后可以加
public class TemplePanel : UIBase
{
    #region 获取UI组件和属性
    public BagPetSlot[] evoluteSlots;
    public BagPetSlot[] mergeSlots;
    public Text EvoluteA;
    public Text EvoluteB;
    public Button btnA;
    public Button btnB;
    public Image imgA;
    public Image imgB;

    private Slot[] slots;
    private CanvasGroup[] canvasGroup;//0是总面板 1是进化 2是进化路线 3是合成
    private Dropdown[] dropdowns;
    private List<PetModel> bagPets;
    private BagPetSlot currentSlot;
    private PetModel pet;
    private PetModel mergePet1;
    private PetModel mergePet2;
    private List<PetModel> pets;
    private List<BagItem> guarditems;
    private List<BagItem> additems;
    List<Dropdown.OptionData> listOptions1;
    List<Dropdown.OptionData> listOptions2;
    List<Dropdown.OptionData> itemOptions1;
    List<Dropdown.OptionData> itemOptions2;
    private float time;
    #endregion

    private void Awake()
    {
        Bind(UIEvent.TEMPLE_PANEL_ACTIVE,UIEvent.PET_BAG_REFRESH,UIEvent.EVOLUTEA, UIEvent.EVOLUTEB,UIEvent.MERGE_ITEM_REFRESH);
        canvasGroup = GetComponentsInChildren<CanvasGroup>();
        dropdowns = GetComponentsInChildren<Dropdown>();
        listOptions1 = new List<Dropdown.OptionData>();
        listOptions2 = new List<Dropdown.OptionData>();
        itemOptions1 = new List<Dropdown.OptionData>();
        itemOptions2 = new List<Dropdown.OptionData>();
        pets = new List<PetModel>();
        guarditems = new List<BagItem>();
        additems = new List<BagItem>();
        slots = GetComponentsInChildren<Slot>();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.TEMPLE_PANEL_ACTIVE:
                if ((bool)message)
                {
                    canvasGroup[0].alpha = 1;
                    canvasGroup[0].blocksRaycasts = true;
                }
                else
                {
                    canvasGroup[0].alpha = 0;
                    canvasGroup[0].blocksRaycasts = false;
                    canvasGroup[2].alpha = 0;
                    canvasGroup[2].blocksRaycasts = false;
                }
                break;
            case UIEvent.PET_BAG_REFRESH:
                currentSlot = null;
                bagPets = message as List<PetModel>;
                if (bagPets != null)
                {
                    for (int i = 0; i < bagPets.Count; i++)
                    {
                        evoluteSlots[i].RemovePet();
                        evoluteSlots[i].StorePet(bagPets[i]);
                    }
                }
                pets.Clear();
                foreach (PetModel pet in PetCharacter.Instance.bagPets)
                {
                    if (pet.Level >= 40)
                        pets.Add(pet);
                }
                foreach (PetModel pet in PetCharacter.Instance.ranchPets)
                {
                    if (pet.Level >= 40)
                        pets.Add(pet);
                }
                SetChoosePet();
                break;
            case UIEvent.EVOLUTEA:
                if((bool)message)
                {
                    pet.Evolute(true);
                    PlayerCharacter.Instance.player.ChangeMoney(-1000, 0, 0);
                    Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
                    Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
                    Dispatch(AreaCode.UI, UIEvent.PET_BAG_REFRESH, PetCharacter.Instance.bagPets);
                    UpdateUI();
                }
                else
                    Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "没有进化之书！");
                break;
            case UIEvent.EVOLUTEB:
                if ((bool)message)
                {
                    pet.Evolute(false);
                    PlayerCharacter.Instance.player.ChangeMoney(-1000, 0, 0);
                    Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
                    Dispatch(AreaCode.UI, UIEvent.BATTLE_PET_REFRESH, pet);
                    Dispatch(AreaCode.UI, UIEvent.PET_BAG_REFRESH, PetCharacter.Instance.bagPets);
                    UpdateUI();
                }
                else
                    Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "没有高级进化之书！");
                break;
            case UIEvent.MERGE_ITEM_REFRESH:
                guarditems.Clear();
                additems.Clear();
                foreach (BagItem item in message as List<BagItem>)
                {
                    if (item.ItemId >= 37 && item.ItemId <= 41)
                        guarditems.Add(item);
                    else
                        additems.Add(item);
                }
                SetChooseItem();
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        canvasGroup[0].alpha = 0;
        dropdowns[0].onValueChanged.AddListener((int v) => ChoosePet(v-1, 0));
        dropdowns[1].onValueChanged.AddListener((int v) => ChoosePet(v-1, 1));
        dropdowns[2].onValueChanged.AddListener((int v) => ChooseItem(v-1, 0));
        dropdowns[3].onValueChanged.AddListener((int v) => ChooseItem(v-1, 1));
    }

    private void ChooseItem(int v,int num)
    {
        slots[num].RemoveItem();
        if (num == 0)
            slots[num].StoreItem(InventoryManager.Instance.GetItemByID<Item>(guarditems[v].ItemId) as Item);
        else
            slots[num].StoreItem(InventoryManager.Instance.GetItemByID<Item>(additems[v].ItemId) as Item);
    }

    //TODO 主宠选择过 副宠列表中要去除 反之同理 不选择则要添加回来
    private void ChoosePet(int v,int num)
    {
        //if(mergeSlots[num].transform.childCount>0)
        //{
        //    if (num == 0)
        //        listOptions2.Add(new Dropdown.OptionData(mergeSlots[num].GetPet().Name + "-" + mergeSlots[num].GetPet().CC));
        //    else
        //        listOptions1.Add(new Dropdown.OptionData(mergeSlots[num].GetPet().Name + "-" + mergeSlots[num].GetPet().CC));
        //    mergeSlots[num].RemovePet();
        //}
        mergeSlots[num].RemovePet();
        mergeSlots[num].StorePet(pets[v]);
        //if (num == 0)
        //    listOptions2.RemoveAt(v+1);
        //else
        //    listOptions1.RemoveAt(v+1);
        //string captionName1 = dropdowns[0].captionText.text;
        //string captionName2 = dropdowns[1].captionText.text;
        //dropdowns[0].ClearOptions();
        //dropdowns[1].ClearOptions();
        //dropdowns[0].AddOptions(listOptions1);
        //dropdowns[1].AddOptions(listOptions2);
        //dropdowns[0].captionText.text = captionName1;
        //dropdowns[1].captionText.text = captionName2;
    }

    private void Update()
    {
        WhichSlotChoosed();
    }

    private void SetChoosePet()
    {
        listOptions1.Clear();
        listOptions2.Clear();
        listOptions1.Add(new Dropdown.OptionData("请选择合成主宠"));
        listOptions2.Add(new Dropdown.OptionData("请选择合成副宠"));
        dropdowns[0].ClearOptions();
        dropdowns[1].ClearOptions();
        foreach (PetModel pet in pets)
        {
            listOptions1.Add(new Dropdown.OptionData(pet.Name + "-" + pet.CC));
        }
        foreach (PetModel pet in pets)
        {
            listOptions2.Add(new Dropdown.OptionData(pet.Name + "-" + pet.CC));
        }
        dropdowns[0].AddOptions(listOptions1);
        dropdowns[1].AddOptions(listOptions2);
    }

    private void SetChooseItem()
    {
        itemOptions1.Clear();
        itemOptions2.Clear();
        itemOptions1.Add(new Dropdown.OptionData("请选择守宠道具"));
        itemOptions2.Add(new Dropdown.OptionData("请选择增益道具"));
        dropdowns[2].ClearOptions();
        dropdowns[3].ClearOptions();
        foreach (BagItem item in guarditems)
        {
            itemOptions1.Add(new Dropdown.OptionData(InventoryManager.Instance.GetItemNameByID(item.ItemId) + "-" + item.Amount));
        }
        foreach (BagItem item in additems)
        {
            itemOptions2.Add(new Dropdown.OptionData(InventoryManager.Instance.GetItemNameByID(item.ItemId) + "-" + item.Amount));
        }
        dropdowns[2].AddOptions(itemOptions1);
        dropdowns[3].AddOptions(itemOptions2);
    }

    private void WhichSlotChoosed()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;
        if (gameObject != null)
        {
            if (gameObject.GetComponent<BagPetSlot>() != null)
            {
                currentSlot = gameObject.GetComponent<BagPetSlot>();
                UpdateUI();
            }
        }
    }

    private void UpdateUI()
    {
        canvasGroup[2].alpha = 1;
        canvasGroup[2].blocksRaycasts = true;
        if (currentSlot != null)
        {
            pet = currentSlot.GetPet();
            currentSlot.UpdateUI();
        }
        if (pet != null && pet.ID <= 59)
        {
            EvoluteA.text = "等级要求：40\n材料要求：" + InventoryManager.Instance.GetItemNameByID(34) + "\n进化结果：";
            EvoluteB.text = "等级要求：60\n材料要求：" + InventoryManager.Instance.GetItemNameByID(35) + "\n进化结果：";
            imgA.sprite = Resources.Load<Sprite>(PetCharacter.Instance.GetPetSpriteByID(pet.ID + 5));
            imgB.sprite = Resources.Load<Sprite>(PetCharacter.Instance.GetPetSpriteByID(pet.ID + 10));
        }
    }


    public void MergePet()
    {
        mergePet1 = mergeSlots[0].GetPet();
        mergePet2 = mergeSlots[1].GetPet();
        if (mergePet2.isMain)
            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "战斗宠物不可作副宠融合！");
        else
        { 
            if (PlayerCharacter.Instance.player.Coin >= 10000)
            {
                if (mergePet1 != null && mergePet2 != null)
                {
                    //不是同一只宠物
                    if (mergePet1.id_pet != mergePet2.id_pet)
                    {
                        if (mergePet1.Level >= 40 && mergePet2.Level >= 40)
                        {
                            int random = new System.Random().Next(0, 1000);
                            if (random <= 500)
                            {
                                //至尊神石使用效果 出小神龙
                                if (slots[0].GetItemID() == 40)
                                {
                                    mergePet1.ID = 70;
                                    mergePet1.Name = PetCharacter.Instance.GetPetNameByID(mergePet1.ID);
                                    mergePet1.Sprite = PetCharacter.Instance.GetPetSpriteByID(mergePet1.ID);
                                    print(mergePet1.Name);
                                }
                                //百变灵石使用效果 99%出稀有神宠 1%小神龙
                                else if (slots[0].GetItemID() == 41)
                                {
                                    random = new System.Random().Next(0, 100);
                                    if (random == 100)
                                        mergePet1.ID = 70;
                                    else
                                        mergePet1.ID = new System.Random().Next(71, 75);
                                    mergePet1.Name = PetCharacter.Instance.GetPetNameByID(mergePet1.ID);
                                    mergePet1.Sprite = PetCharacter.Instance.GetPetSpriteByID(mergePet1.ID);
                                    print(mergePet1.Name);
                                }
                                else
                                {
                                    //69以上都是神宠不需要再变了 只增加CC
                                    if (mergePet1.ID < 69)
                                    {
                                        //龙有30%可能变成神 不然就只增加CC
                                        if (mergePet2.Quality == PetModel.PetQuality.Dragon)
                                        {
                                            random = new System.Random().Next(0, 100);
                                            if (random >= 70)
                                            {
                                                mergePet1.ID += 5;
                                                mergePet1.Quality = PetModel.PetQuality.God;
                                                mergePet1.petKind = PetModel.PetKind.God;
                                                mergePet1.Name = PetCharacter.Instance.GetPetNameByID(mergePet1.ID);
                                                mergePet1.Sprite = PetCharacter.Instance.GetPetSpriteByID(mergePet1.ID);
                                                print(mergePet1.Name);
                                            }
                                        }
                                        //副宠高于六阶 有几率合成之后跳阶 但不能直接变成神 这里可以直接看Quality属性
                                        else if ((mergePet2.Quality == PetModel.PetQuality.Six && new System.Random().Next(0, 1000) >= 990) ||
                                            (mergePet2.Quality == PetModel.PetQuality.Seven && new System.Random().Next(0, 1000) >= 900) ||
                                            (mergePet2.Quality == PetModel.PetQuality.Eight && new System.Random().Next(0, 1000) >= 750) ||
                                            (mergePet2.Quality == PetModel.PetQuality.Nine && new System.Random().Next(0, 1000) >= 550) ||
                                            (mergePet2.Quality == PetModel.PetQuality.Ten && new System.Random().Next(0, 1000) >= 300) ||
                                            (mergePet2.Quality == PetModel.PetQuality.Eleven))
                                        {
                                            mergePet1.ID += 10;
                                            mergePet1.Name = PetCharacter.Instance.GetPetNameByID(mergePet1.ID);
                                            mergePet1.Sprite = PetCharacter.Instance.GetPetSpriteByID(mergePet1.ID);
                                            //print(mergePet1.Name);
                                        }
                                        else
                                        {
                                            mergePet1.ID += 5;
                                            mergePet1.Name = PetCharacter.Instance.GetPetNameByID(mergePet1.ID);
                                            mergePet1.Sprite = PetCharacter.Instance.GetPetSpriteByID(mergePet1.ID);
                                            //print(mergePet1.Name);
                                        }
                                    }
                                    mergePet1.Merge(mergePet2, slots[0].GetItemID(), slots[1].GetItemID());
                                    //升级之后等级清除
                                    mergePet1.Level = 1;
                                    mergePet1.ChangeAttris();
                                    //    }
                                    //}
                                    //InventoryManager.Instance.ShowToolTip("合成成功");
                                    Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "合成成功");
                                    PetCharacter.Instance.state.MergeNum++;
                                    //替换成新的宠物
                                    for (int i = 0; i < PetCharacter.Instance.ranchPets.Count; i++)
                                    {
                                        if (PetCharacter.Instance.ranchPets[i].id_pet == mergePet1.id_pet)
                                            PetCharacter.Instance.ranchPets[i] = mergePet1;
                                    }
                                    for (int i = 0; i < PetCharacter.Instance.bagPets.Count; i++)
                                    {
                                        if (PetCharacter.Instance.bagPets[i].id_pet == mergePet1.id_pet)
                                        {
                                            PetCharacter.Instance.bagPets[i] = mergePet1;
                                        }

                                    }
                                    //合成成功移除副宠
                                    if (PetCharacter.Instance.ranchPets.Contains(mergePet2))
                                    {
                                        PetCharacter.Instance.ranchPets.Remove(mergePet2);
                                    }
                                    if (PetCharacter.Instance.bagPets.Contains(mergePet2))
                                    {
                                        PetCharacter.Instance.bagPets.Remove(mergePet2);
                                    }
                                    pets.Remove(mergePet2);
                                }
                            }
                            else
                            {
                                //合成失败且未加道具移除副宠
                                if (slots[0].GetItemID() == -1)
                                {
                                    if (PetCharacter.Instance.ranchPets.Contains(mergePet2))
                                    {
                                        PetCharacter.Instance.ranchPets.Remove(mergePet2);
                                    }
                                    if (PetCharacter.Instance.bagPets.Contains(mergePet2))
                                    {
                                        PetCharacter.Instance.bagPets.Remove(mergePet2);
                                    }
                                    pets.Remove(mergePet2);
                                }
                                //InventoryManager.Instance.ShowToolTip("合成失败");
                                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "合成失败");
                            }
                            //如果使用了道具 则移除背包中道具
                            if (slots[0].GetItemID() != -1)
                                Dispatch(AreaCode.UI, UIEvent.BAG_REMOVE_REFRESH, new BagItem() { Amount = 1, ItemId =  slots[0].GetItemID()  });
                            if (slots[1].GetItemID() != -1)
                                Dispatch(AreaCode.UI, UIEvent.BAG_REMOVE_REFRESH, new BagItem() { Amount = 1, ItemId =  slots[1].GetItemID()  });
                            //刷新牧场和背包宠物
                            Dispatch(AreaCode.UI, UIEvent.RANCH_REFRESH, PetCharacter.Instance.ranchPets);
                            Dispatch(AreaCode.UI, UIEvent.PET_BAG_REFRESH, PetCharacter.Instance.bagPets);
                            PlayerCharacter.Instance.player.ChangeMoney(-10000, 0, 0);
                            Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
                            Dispatch(AreaCode.UI, UIEvent.GET_MERGE_ITEM, null);
                        }
                    }
                }
            }
            else
                Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "金币不足！需要10000金币！");
        }
    }

    public void EvoluteRouteA()
    {
        if (pet.Level >= 40 && PlayerCharacter.Instance.player.Coin >= 1000)
            Dispatch(AreaCode.UI, UIEvent.EVOLUTEA_FINISH, 34);
        else
            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "金币不足！需要1000金币！\n或是宠物不足40级！");
    }

    public void EvoluteRouteB()
    {
        if (pet.Level >= 60 && PlayerCharacter.Instance.player.Coin >= 1000)
            Dispatch(AreaCode.UI, UIEvent.EVOLUTEB_FINISH, 35);
        else
            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "金币不足！需要1000金币！\n或是宠物不足60级！");
    }

    public void ShowEvolute()
    {
        canvasGroup[1].alpha = 1;
        canvasGroup[1].blocksRaycasts = true;
        canvasGroup[3].alpha = 0;
        canvasGroup[3].blocksRaycasts = false;
    }

    public void ShowMerge()
    {
        canvasGroup[1].alpha = 0;
        canvasGroup[1].blocksRaycasts = false;
        canvasGroup[3].alpha = 1;
        canvasGroup[3].blocksRaycasts = true;
        Dispatch(AreaCode.UI, UIEvent.GET_MERGE_ITEM, null);
    }

}
