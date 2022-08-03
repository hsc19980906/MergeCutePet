using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RanchPanel : UIBase
{
    private PetSlot[] slots;
    private List<PetModel> ranchPets;
    private Button btnPutPetBag;
    private PetSlot currentSlot;

    private void Awake()
    {
        Bind(UIEvent.RANCH_PANEL_ACTIVE,UIEvent.RANCH_REFRESH);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.RANCH_PANEL_ACTIVE:
                setPanelActive((bool)message);
                break;
            case UIEvent.RANCH_REFRESH:
                currentSlot = null;
                ranchPets = message as List<PetModel>;
                CleanUpSlots();
                if(ranchPets!=null)
                {
                    for (int i = 0; i < ranchPets.Count; i++)
                    {
                        if (i < slots.Length)
                            slots[i].StorePet(ranchPets[i]);
                        else
                        {
                            Dispatch(AreaCode.UI, UIEvent.SYSTEM_MSG, "牧场已满！");
                            ranchPets[i].isCarry = !ranchPets[i].isCarry;
                            Dispatch(AreaCode.CHARACTER, CharacterEvent.REFRESH_PET, ranchPets[i]);
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public void Start()
    {
        //获取所有的物品槽
        slots = GetComponentsInChildren<PetSlot>();
        btnPutPetBag = transform.Find("btnPutPetBag").GetComponent<Button>();

        btnPutPetBag.onClick.AddListener(PutPetBag);

        Dispatch(AreaCode.CHARACTER, CharacterEvent.RANCH_REFRESH, null);
        setPanelActive(false);
    }

    private void Update()
    {
        WhichSlotChoosed();
    }

    private void WhichSlotChoosed()
    {
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;
        if(gameObject!=null)
        {
            if (gameObject.GetComponent<PetSlot>() != null)
            {
                currentSlot = gameObject.GetComponent<PetSlot>();
            }
        }
    }

    //之前之所以有问题 譬如 槽里已经放了三个宠物 然后我移除的时候 是按移除后列表数量来移除 这样导致最后一个没被删掉
    private void CleanUpSlots()
    {
        foreach (PetSlot slot in slots)
        {
            slot.RemovePet();
        }
    }

    private void PutPetBag()
    {
        if (currentSlot != null && currentSlot.GetPet() != null)
        {
            PetModel pet = currentSlot.GetPet();
            pet.isCarry = !pet.isCarry;
            Destroy(currentSlot.transform.GetChild(0).gameObject);
            Dispatch(AreaCode.CHARACTER, CharacterEvent.REFRESH_PET, pet);
        }
    }

    //丢弃宠物
    public void ThrowPet()
    {
        ranchPets.Remove(currentSlot.GetPet());
        currentSlot.RemovePet();
    }

}
