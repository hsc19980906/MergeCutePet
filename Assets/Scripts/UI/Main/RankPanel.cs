using Common.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankPanel : UIBase
{
    public GameObject rankGameObject;
    public GameObject parent;
    private CanvasGroup canvasGroup;
    private List<PlayerRank> playerRanks;
    private void Awake()
    {
        Bind(UIEvent.RANK_PANEL_ACTIVE,UIEvent.RANK_REFRESH);
        playerRanks = new List<PlayerRank>();
        OpCustom(Common.OpCode.Rank, null);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.RANK_PANEL_ACTIVE:
                if((bool)message)
                {
                    canvasGroup.alpha = 1;
                    canvasGroup.blocksRaycasts = true;
                }
                else
                {
                    canvasGroup.alpha = 0;
                    canvasGroup.blocksRaycasts = false;
                }
                break;
            case UIEvent.RANK_REFRESH:
                playerRanks = message as List<PlayerRank>;
                foreach (PlayerRank rank in playerRanks)
                {
                    GameObject gameObject = Instantiate(rankGameObject) as GameObject;
                    gameObject.transform.SetParent(parent.transform);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.GetComponent<PlayerRankMsg>().UpdateUI(rank);
                }
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
