using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class InventoryManager : ManagerBase
{
    public static InventoryManager Instance;
    private Dictionary<int, Consumable> consumeItems;//消耗型物品
    private Dictionary<int, Equipment> equipments;//装备
    private Dictionary<int, Egg> eggs;//宠物蛋
    private Dictionary<int, ItemMaterial> itemMaterials;//材料
    private Dictionary<int, SkillBook> skillBooks;//技能书


    private void Awake()
    {
        Instance = this;
        ParseItemJson();
    }

    //解析
    private void ParseItemJson()
    {
        //itemList = new Dictionary<int, Item>();
        consumeItems=new Dictionary<int, Consumable>();
        equipments = new Dictionary<int, Equipment>();
        eggs = new Dictionary<int, Egg>();
        itemMaterials = new Dictionary<int, ItemMaterial>();
        skillBooks = new Dictionary<int, SkillBook>();  
        //pets = new List<PetModel>();
        TextAsset itemText = Resources.Load<TextAsset>("Item");
        string itemsJson = itemText.text;
        JArray array = JArray.Parse(itemsJson);
        foreach (var temp in array)
        {
            //获取item类型 转成枚举
            string typeStr = (string)temp["Type"];
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), typeStr);

            //公有属性
            int id = (int)(temp["ID"]);
            string name = (string)temp["Name"];
            Item.ItemQuality quality = (Item.ItemQuality)System.Enum.Parse(typeof(Item.ItemQuality), (string)temp["Quality"]);
            string description = (string)temp["Description"];
            int capacity = (int)(temp["Capacity"]);
            int buyPrice = (int)(temp["BuyPrice"]);
            int sellPrice = (int)(temp["SellPrice"]);
            string sprite = (string)temp["Sprite"];
            Item.ItemMoney buymoney = (Item.ItemMoney)System.Enum.Parse(typeof(Item.ItemMoney), (string)temp["BuyMoney"]);
            Item.ItemMoney sellmoney = (Item.ItemMoney)System.Enum.Parse(typeof(Item.ItemMoney), (string)temp["SellMoney"]);
            //PetModel pet = null;
            switch (type)
            {
                case Item.ItemType.Consumable:
                    consumeItems[id] = new Consumable(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, buymoney, sellmoney);
                    break;
                case Item.ItemType.Equipment:
                    double atk = (double)temp["Attack"];
                    double def = (double)temp["Defense"];
                    double hp = (double)temp["Hp"];
                    double mp = (double)temp["Mp"];
                    double sp = (double)temp["Speed"];
                    int level = (int)temp["Level"];
                    Equipment.EquipmentType equipmentType = (Equipment.EquipmentType)System.Enum.Parse(typeof(Equipment.EquipmentType), (string)temp["EquipmentType"]);

                    equipments[id]= new Equipment(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, atk, def, hp, mp, sp, equipmentType, buymoney, sellmoney, level);
                    break;
                case Item.ItemType.Egg:
                    eggs[id] = new Egg(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, buymoney, sellmoney);
                    break;
                case Item.ItemType.Material:
                    itemMaterials[id] = new ItemMaterial(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, buymoney, sellmoney);
                    break;
                case Item.ItemType.SkillBook:
                    atk = (double)temp["Attack"];
                    def = (double)temp["Defense"];
                    hp = (double)temp["Hp"];
                    skillBooks[id] = new SkillBook(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, buymoney, sellmoney, atk, def, hp);
                    break;
                default:
                    break;
            }
            //pets.Add(pet);
        }
    }

    public T GetItemByID<T>(int id)where T : Item   
    {

        //if(typeof(T)==typeof( Consumable))
        {
            if (consumeItems.ContainsKey(id))
                return (T)(object)consumeItems[id];
        }
        //if (typeof(T) == typeof(Equipment))
        {
            if (equipments.ContainsKey(id))
                return (T)(object)equipments[id];
        }
        //if (typeof(T) == typeof(Egg))
        {
            if (eggs.ContainsKey(id))
                return (T)(object)eggs[id];
        }
        //if (typeof(T) == typeof(ItemMaterial))
        {
            if (itemMaterials.ContainsKey(id))
                return (T)(object)itemMaterials[id];
        }
        //if (typeof(T) == typeof(SkillBook))
        {
            if (skillBooks.ContainsKey(id))
                return (T)(object)skillBooks[id];
        }
        return null;
    }

    public string GetItemNameByID(int id)
    {
        return (GetItemByID<Item>(id)).Name;
    }

    //public void ShowToolTip(string content,Vector3 position)
    //{
    //    toolTip.Show(content, position);
    //}
    //public void HideToolTip()
    //{
    //    toolTip.Hide();
    //}
}
