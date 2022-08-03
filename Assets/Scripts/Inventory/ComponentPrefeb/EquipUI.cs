using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUI : MonoBehaviour
{
    public Equipment Equipment;
    private Image itemIcon;

    private Image ItemIcon
    {
        get
        {
            if (itemIcon == null)
            {
                itemIcon = GetComponent<Image>();
            }
            return itemIcon;
        }
    }

    public void SetItem(Equipment equipment)
    {
        this.Equipment = equipment;
        UpdateUI();
    }

    private void UpdateUI()
    {
        ItemIcon.sprite = Resources.Load<Sprite>(Equipment.Sprite);
    }

}
