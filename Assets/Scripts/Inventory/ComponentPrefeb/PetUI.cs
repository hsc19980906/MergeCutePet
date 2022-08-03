using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetUI : MonoBehaviour
{
    private PetModel pet;

    private Image petIcon;
    private Text petName;

    private Image PetIcon
    {
        get
        {
            if (petIcon == null)
            {
                petIcon = GetComponentInChildren<Image>();
            }
            return petIcon;
        }
    }

    private Text PetName
    {
        get
        {
            if (petName == null)
            {
                petName = GetComponentInChildren<Text>();
            }
            return petName;
        }
    }

    public void SetPet(PetModel pet)
    {
        this.pet = pet;
        UpdateUI();
    }

    public PetModel GetPet()
    {
        return this.pet;
    }

    public void UpdateUI()
    {
        PetIcon.sprite = Resources.Load<Sprite>(pet.Sprite);
        PetName.text = pet.Name;
    }
}
