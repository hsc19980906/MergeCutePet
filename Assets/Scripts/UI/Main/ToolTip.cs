using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolTip : UIBase
{
    private Text toolTipText;
    private Text contentText;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        Bind(UIEvent.ITEM_MSG);
        toolTipText = GetComponent<Text>();
        contentText = transform.Find("Content").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.ITEM_MSG:
                ItemMsg msg = message as ItemMsg;
                Show(msg.itemMsg, msg.position);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (canvasGroup.alpha != toolTipA)
        //{
        //    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, toolTipA, changeSpeed * Time.deltaTime);
        //    if (Mathf.Abs(canvasGroup.alpha - toolTipA) < 0.05)
        //    {
        //        canvasGroup.alpha = toolTipA;
        //    }
        //}
        if(Input.GetMouseButtonDown(0))
        {
            Hide();
        }
        //触屏 点击消失
       if(Input.touchCount == 1)
       {
            if(Input.touches[0].phase==TouchPhase.Began)
            {
                Hide();
            }
       }
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show(string text,Vector3 position)
    {
        contentText.text = text;
        toolTipText.text = text;
        transform.position = position;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
}
