using System;
using UnityEngine;
using UnityEngine.UI;

public class TownPanel : UIBase
{
    private Button[] buttons;
    private DateTime time;
    int id = 0;
    int random;
    string Tip = "离线收益提示：\n在勇士离线期间,\n你的宠物仍旧在奋斗!\n为你带来了以下收益：\n";

    private void Awake()
    {
        Bind(UIEvent.TOWN_PANEL_ACTIVE, UIEvent.OFFTIME_BENEFIR_COUNT);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.TOWN_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            case UIEvent.OFFTIME_BENEFIR_COUNT:
                if (PetCharacter.Instance.state.map != Enemy.Map.None)
                    CountOffTimeReward();
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(ShopClick);
        buttons[1].onClick.AddListener(TempleClick);
        buttons[2].onClick.AddListener(RanchClick);
        buttons[3].onClick.AddListener(RankClick);
        
    }

    private void RankClick()
    {
        Dispatch(AreaCode.UI, UIEvent.RANK_PANEL_ACTIVE, true);
        setPanelActive(false);
    }

    //TODO 收益计算 需要改进
    private void CountOffTimeReward()
    {
        if (PetCharacter.Instance.state.offTime != DateTime.MinValue && PetCharacter.Instance.state.map!=Enemy.Map.None)
        {
            int Exp = PetCharacter.Instance.SetExpByMap();
            int Coin = PetCharacter.Instance.SetCoinByMap();
            int Diamond = PetCharacter.Instance.SetDiamondByMap();
            time = DateTime.Now;
            TimeSpan timeSpan = (time - PetCharacter.Instance.state.offTime);
            int Amount = timeSpan.Minutes;
            if(timeSpan.Minutes < PetCharacter.Instance.state.ExpBuffMinutes)
            {
                if (timeSpan.Hours > 8)
                {
                    Amount = 3600 * 8;
                }
                PetCharacter.Instance.state.TotalExp += Convert.ToInt64(Amount * Exp * PetCharacter.Instance.state.ExpBuff);
                Tip += "Exp+" + Convert.ToInt64(Amount * Exp * PetCharacter.Instance.state.ExpBuff);
                PlayerCharacter.Instance.player.ChangeMoney(Amount * Coin, Amount * Diamond, 0);
                Tip += "\n金币+" + Amount * Coin + " 钻石+" + Amount * Diamond;
                Amount /= 15;
                while (Amount > 0)
                {
                    Amount--;
                    OffTimeReward();
                }
                PetCharacter.Instance.state.ExpBuffMinutes -= timeSpan.Minutes;
            }
            else
            {
                if (timeSpan.Hours > 8)
                {
                    Amount = 3600 * 8;
                }
                PetCharacter.Instance.state.TotalExp += Convert.ToInt64(
                    PetCharacter.Instance.state.ExpBuffMinutes * Exp * PetCharacter.Instance.state.ExpBuff
                    + (Amount * 10 - PetCharacter.Instance.state.ExpBuffMinutes) * Exp);
                Tip += "Exp+" + Convert.ToInt64(
                    PetCharacter.Instance.state.ExpBuffMinutes * Exp * PetCharacter.Instance.state.ExpBuff
                    + (Amount * 10 - PetCharacter.Instance.state.ExpBuffMinutes) * Exp);
                PlayerCharacter.Instance.player.ChangeMoney(Amount * Coin, Amount * Diamond, 0);
                Tip += "\n金币+" + Amount * Coin + " 钻石+" + Amount * Diamond;
                Amount /= 15;
                while (Amount > 0)
                {
                    Amount--;
                    OffTimeReward();
                }
                PetCharacter.Instance.state.ExpBuffMinutes = 0;
                PetCharacter.Instance.state.ExpBuff = 1f;
            }
            Dispatch(AreaCode.UI, UIEvent.REFRESH_PLAYER_MONEY, PlayerCharacter.Instance.player);
            Dispatch(AreaCode.UI, UIEvent.ITEM_MSG, new ItemMsg() { itemMsg=Tip,position= new Vector3(Screen.width / 2 -350, Screen.height / 2) });
        }
    }
  
    //根据地图定掉落
    private void OffTimeReward()
    {
        random = new System.Random().Next(0, 10000);
        int[] nums1 = { 61, 64, 67, 72, 75, 78, 81, 84 };
        int[] nums2 = { 63, 66, 69, 70, 74, 77, 80, 83, 86 };
        int[] nums3 = { 62, 65, 68, 71, 73, 76, 79, 82, 85 };
        if (random > 1000)
        {
            if (random > 1000 && random <= 9800)
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
            if (random ==10000)
                id = 94;
            Dispatch(AreaCode.UI, UIEvent.BAG_ADD_REFRESH, new BagItem() { Amount = 1, ItemId =  id });
        }
    }

    private void RanchClick()
    {
        Dispatch(AreaCode.CHARACTER, CharacterEvent.RANCH_REFRESH, null);
        Dispatch(AreaCode.UI, UIEvent.RANCH_PANEL_ACTIVE, true);
        setPanelActive(false);
    }

    private void TempleClick()
    {
        Dispatch(AreaCode.CHARACTER, CharacterEvent.PET_BAG_REFRESH, null);
        Dispatch(AreaCode.UI, UIEvent.TEMPLE_PANEL_ACTIVE, true);
        setPanelActive(false);
    }

    private void ShopClick()
    {
        Dispatch(AreaCode.UI, UIEvent.SHOP_PANEL_ACTIVE, true);
        setPanelActive(false);
    }
}
