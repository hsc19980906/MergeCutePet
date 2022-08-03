using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBar : UIBase
{
    private Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(TownClick);
        buttons[1].onClick.AddListener(BagClick);
        buttons[2].onClick.AddListener(BattleClick);
        buttons[3].onClick.AddListener(PetClick);
        buttons[4].onClick.AddListener(TaskClick);
    }

    private void TaskClick()
    {
        Dispatch(AreaCode.UI, UIEvent.BAG_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.BATTLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PET_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PLAY_INFO_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TASK_PANEL_ACTIVE, true);
        Dispatch(AreaCode.UI, UIEvent.TOWN_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.SHOP_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TEMPLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANCH_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANK_PANEL_ACTIVE, false);
    }

    private void PetClick()
    {
        //Dispatch(AreaCode.CHARACTER, CharacterEvent.PET_BAG_REFRESH, null);
        Dispatch(AreaCode.UI, UIEvent.BAG_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.BATTLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PET_PANEL_ACTIVE, true);
        Dispatch(AreaCode.UI, UIEvent.PLAY_INFO_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TASK_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TOWN_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.SHOP_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TEMPLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANCH_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANK_PANEL_ACTIVE, false);
        //InventoryManager.Instance.HideToolTip();
    }

    private void BattleClick()
    {
        Dispatch(AreaCode.UI, UIEvent.BAG_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.BATTLE_PANEL_ACTIVE, true);
        Dispatch(AreaCode.UI, UIEvent.PET_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PLAY_INFO_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TASK_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TOWN_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.SHOP_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TEMPLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANCH_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANK_PANEL_ACTIVE, false);
        //InventoryManager.Instance.HideToolTip();
    }

    private void BagClick()
    {
        Dispatch(AreaCode.UI, UIEvent.BAG_PANEL_ACTIVE, true);
        Dispatch(AreaCode.UI, UIEvent.BATTLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PET_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PLAY_INFO_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TASK_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TOWN_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.SHOP_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TEMPLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANCH_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANK_PANEL_ACTIVE, false);
        //InventoryManager.Instance.HideToolTip();
    }

    private void TownClick()
    {
        Dispatch(AreaCode.UI, UIEvent.BAG_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.BATTLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PET_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.PLAY_INFO_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TASK_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TOWN_PANEL_ACTIVE, true);
        Dispatch(AreaCode.UI, UIEvent.SHOP_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.TEMPLE_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANCH_PANEL_ACTIVE, false);
        Dispatch(AreaCode.UI, UIEvent.RANK_PANEL_ACTIVE, false);
        //InventoryManager.Instance.HideToolTip();
    }
}
