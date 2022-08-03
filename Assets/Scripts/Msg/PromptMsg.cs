using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PromptMsg
{
    public string Text;
    public Color Color;

    public PromptMsg()
    {

    }

    public PromptMsg(string txt,Color color)
    {
        this.Text = txt;
        this.Color = color;
    }

    /// <summary>
    /// 避免频繁new对象
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="color"></param>
    public void Change(string txt, Color color)
    {
        this.Text = txt;
        this.Color = color;
    }
}
