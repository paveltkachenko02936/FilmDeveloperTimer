using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    public void OnStartBtnClick()
    {
        AppManager.GetInstance().StartTimer();
    }

    public void OnCloseBtnClick()
    {
        AppManager.GetInstance().StopTimer();
    }
}