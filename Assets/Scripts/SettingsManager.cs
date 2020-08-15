using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SettingsManager
{
    private const int timeDelta = 10;

    private int developingTime = 0;
    private int washingTime = 0;
    private int fixingTime = 0;

    public enum ESettingMode
    {
        Develop,
        Washing,
        Fixing
    }

    private ESettingMode settingMode;

    public SettingsManager()
    {
        settingMode = ESettingMode.Develop;
    }

    public int GetTime()
    {
        return GetTime(settingMode);
    }

    public int GetTime(ESettingMode mode)
    {
        switch (mode)
        {
            case ESettingMode.Develop: return developingTime;
            case ESettingMode.Washing: return washingTime;
            case ESettingMode.Fixing: return fixingTime;
        }

        return 0;
    }

    public string GetHeader()
    {
        switch (settingMode)
        {
            case ESettingMode.Develop: return "Set developing time";
            case ESettingMode.Washing: return "Set washing time";
            case ESettingMode.Fixing: return "Set fixing time";
        }

        return "";
    }

    public void IncreaseTime()
    {
        switch (settingMode)
        {
            case ESettingMode.Develop: developingTime += timeDelta; break;
            case ESettingMode.Washing: washingTime += timeDelta; break;
            case ESettingMode.Fixing: fixingTime += timeDelta; break;
        }
    }

    public void DecreaseTime()
    {
        switch (settingMode)
        {
            case ESettingMode.Develop:
                developingTime -= timeDelta;
                if (developingTime <= 0)
                {
                    developingTime = 0;
                }
                break;

            case ESettingMode.Washing:
                washingTime -= timeDelta;
                if (washingTime <= 0)
                {
                    washingTime = 0;
                }
                break;

            case ESettingMode.Fixing:
                fixingTime -= timeDelta;
                if (fixingTime <= 0)
                {
                    fixingTime = 0;
                }
                break;
        }
    }

    public void GoToNextMode()
    {
        if (settingMode == ESettingMode.Fixing)
        {
            SceneLoader.ChangeScene("ProcessingScene");
            return;
        }

        settingMode += 1;
    }

    public void GoToPreviousMode()
    {
        settingMode -= 1;
    }

    public bool CanShowPrevSetting()
    {
        return settingMode > ESettingMode.Develop;
    }
}

