using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPetSlot : MonoBehaviour
{
    public GameObject ItemPrefab;

    public void StorePet(PetModel pet)
    {
        GameObject gameObject = Instantiate(ItemPrefab) as GameObject;
        gameObject.transform.SetParent(this.transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.GetComponent<PetUI>().SetPet(pet);
    }

    public void RemovePet()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public PetModel GetPet()
    {
        if (transform.childCount > 0)
            return transform.GetChild(0).GetComponent<PetUI>().GetPet();
        else
            return null; 
    }

    public void ChangePet(PetModel pet)
    {
        if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<PetUI>().SetPet(pet);
    }

    public void UpdateUI()
    {
        if (transform.childCount > 0)
            transform.GetChild(0).GetComponent<PetUI>().UpdateUI();
    }
}
