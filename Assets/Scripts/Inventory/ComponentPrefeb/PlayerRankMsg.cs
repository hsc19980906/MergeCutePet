using Common.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRankMsg : MonoBehaviour
{
    public Image profile;
    public Image imgPet;
    public Text playerName;
    public Text Title;
    public Text petName;
    public Text Level;
    public Text CE;

    public void UpdateUI(PlayerRank playerRank)
    {
        profile.sprite = Resources.Load<Sprite>(playerRank.imgPlayer);
        imgPet.sprite = Resources.Load<Sprite>(playerRank.imgPet);
        playerName.text = playerRank.playerName;
        Title.text = playerRank.WhichTitle();
        petName.text = playerRank.petName;
        Level.text = "Lv." + playerRank.Level;
        CE.text = "战力" + playerRank.Max_CE;    
    }

}
