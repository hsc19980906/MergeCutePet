using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//专门显示提示信息
public class PromptPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.PROMPT_MSG);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.PROMPT_MSG:
                {
                    PromptMsg msg = message as PromptMsg;
                    PromptMessage(msg.Text,msg.Color);
                }
                break;
            default:
                break;
        }
    }

    private Text txt;
    private CanvasGroup cg;

    [SerializeField]
    [Range(0,3)]
    private float showTime = 1f;
    private float timer;

    
    // Start is called before the first frame update
    void Start()
    {
        txt = transform.Find("Text").GetComponent<Text>();
        cg = transform.Find("Text").GetComponent<CanvasGroup>();

        cg.alpha = 0;
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="color"></param>
    private void PromptMessage(string text,Color color)
    {
        txt.text = text;
        txt.color = color;
        cg.alpha = 0;
        timer = 0;
        //做动画显示
        StartCoroutine(PromptAnim());
    }

    private IEnumerator PromptAnim()
    {
        //逐渐显示提示
        while(cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();//等待一帧 
        }
        //显示一会儿提示 由showtime控制显示时间
        while(timer<showTime)
        {
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();//等待一帧 
        }
        //逐渐移除提示
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();//等待一帧 
        }
    }
}
