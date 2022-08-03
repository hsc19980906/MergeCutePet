using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : UIBase
{
    public Equipment.EquipmentType equipType;

    public GameObject ItemPrefab;
    protected Button btnShow;

    protected Button BtnShow
    {
        get
        {
            if (btnShow == null)
            {
                btnShow = GetComponent<Button>();
            }
            return btnShow;
        }
    }

    public virtual void Start()
    {
        BtnShow.onClick.AddListener(ShowMessage);
    }

    private void ShowMessage()
    {
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<EquipUI>().Equipment.GetToolTipText();
            //InventoryManager.Instance.ShowToolTip(toolTipText, transform.localPosition);
            Dispatch(AreaCode.UI, UIEvent.ITEM_MSG, new ItemMsg() { itemMsg = toolTipText, position = new Vector3(Screen.width / 2 - 250, Screen.height / 2) });
        }
    }

    public void StoreEquip(Equipment equip)
    {
        if (transform.childCount == 0)
        {
            GameObject gameObject = Instantiate(ItemPrefab) as GameObject;
            gameObject.transform.SetParent(this.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.GetComponent<EquipUI>().SetItem(equip);
        }
    }

    //脱下装备
    public void TakeOffEquip(int id)
    {
        if(transform.childCount>0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId = id });
        }
    }

    public int GetEquipId()
    {
        return transform.GetChild(0).GetComponent<EquipUI>().Equipment.ID;
    }
}
