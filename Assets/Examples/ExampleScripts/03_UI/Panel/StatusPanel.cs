﻿using UnityEngine.UI;
using Cosmos.UI;
public class StatusPanel : UGUIUIForm
{
    protected override void Awake()
    {
        GetUILable<Button>("BtnQuit").onClick.AddListener(QuitClick);
    }
    void QuitClick()
    {
        UIManager.DeactiveUIForm(UIFormName);
    }
}
