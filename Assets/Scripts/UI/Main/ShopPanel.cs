using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopPanel : Inventory
{
    public InputField inputBuyAmount;
    public Button btnBuy;
    private Slot currentSlot;//获取当前选中的物品槽
    private int price;

    protected override void Awake()
    {
        base.Awake();
        Bind(UIEvent.SHOP_PANEL_ACTIVE);
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
            case UIEvent.SHOP_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            default:
                break;
        }
    }

    public void Start()
    {
        setPanelActive(false);
        btnBuy.onClick.AddListener(BuyItem);
        InitShop();
    }

    private void InitShop()
    {
        StoreItem(1,99);
        StoreItem(60, 99);
        StoreItem(93, 99);
        StoreItem(94, 99);
        StoreItem(37, 99);
        StoreItem(59, 99);
        StoreItem(34, 99);
        StoreItem(35, 99);
    }


    private void BuyItem()
    {
        if (currentSlot != null && currentSlot.transform.childCount > 0)
        {
            CheckAmount();
        }
        //PlayerCharacter.Instance.player.ChangeMoney()
    }

    private void CheckAmount()
    {
        PlayerModel player = PlayerCharacter.Instance.player;
        if (!string.IsNullOrEmpty(inputBuyAmount.text))
        {
            if (int.TryParse(inputBuyAmount.text, out int Amount))
            {
                price = currentSlot.GetItemBuyPrice();
                switch (currentSlot.GetItemBuyMoney())
                {
                    case Item.ItemMoney.Coin:
                        if (Amount * price >= player.Coin)
                        {
                            Amount = player.Coin / price;
                            inputBuyAmount.text = Amount.ToString();
                        }
                        player.ChangeMoney(-Amount * price, 0, 0);
                        break;
                    case Item.ItemMoney.Diamond:
                        if (Amount * price >= player.Diamond)
                        {
                            Amount = player.Diamond / price;
                            inputBuyAmount.text = Amount.ToString();
                        }
                        player.ChangeMoney(0, -Amount * price, 0);
                        break;
                    case Item.ItemMoney.Gold:
                        if (Amount * price >= player.Gold)
                        {
                            Amount = player.Gold / price;
                            inputBuyAmount.text = Amount.ToString();
                        }
                        player.ChangeMoney(0, 0, -Amount * price);
                        break;
                    default:
                        break;
                }
                Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_SIMPLE, player);
                if (Amount != 0)
                    Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = Amount, ItemId = currentSlot.GetItemID() });
            }
        }
    }
}
