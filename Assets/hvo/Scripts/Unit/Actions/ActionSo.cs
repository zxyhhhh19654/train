using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
//这是一个说明
public abstract class ActionSo : ScriptableObject
{
    public Sprite Icon; //技能图标

    public String ActionName;   //技能名称

    public string Guid = System.Guid.NewGuid().ToString();
    public abstract void Execute(GameManager manager);  //执行技能
} 