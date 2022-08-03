using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetSlot:Slot
{
    public override void Start()
    {
        BtnShow.onClick.AddListener(ShowMessage);
    }

    private void ShowMessage()
    {
        if(transform.childCount>0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<PetUI>().GetPet().GetToolTipText();
            //InventoryManager.Instance.ShowToolTip(toolTipText, transform.localPosition);
            Dispatch(AreaCode.UI, UIEvent.ITEM_MSG, new ItemMsg() { itemMsg = toolTipText, position = new Vector3(Screen.width / 2 - 250, Screen.height / 2) });
        }
    }

    public void StorePet(PetModel pet)
    {
        GameObject gameObject = Instantiate(ItemPrefab) as GameObject;
        gameObject.transform.SetParent(this.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<PetUI>().SetPet(pet);
    }

    public PetModel GetPet()
    {
        if (transform.childCount > 0)
            return transform.GetChild(0).GetComponent<PetUI>().GetPet();
        else
            return null;
    }

    public void RemovePet()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
