using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class DevActionsManager : DevActionListener
{
    private List<DevelopingAction> actions = new List<DevelopingAction>();
    private DevelopingAction currentAction;

    private float remainTime = 0.0f;
    private int actionStep = 0;
    private bool started = false;

    private int[] aggitationScheme = { 50, 10, 10, 50 };

    public DevActionsManager()
    {
        DevActionEventDispatcher eventDispatcher = AppManager.GetInstance().EventDispatcher;
        if (eventDispatcher != null)
            eventDispatcher.AddListener(this);
    }

    public void CreateActions()
    {
        SettingsManager settingsManager = AppManager.GetInstance().SettingsManager;
        if (settingsManager == null)
            return;

        int developingTime = settingsManager.GetTime(SettingsManager.ESettingMode.Develop);
        int washingTime = settingsManager.GetTime(SettingsManager.ESettingMode.Washing);
        int fixingTime = settingsManager.GetTime(SettingsManager.ESettingMode.Fixing);

        remainTime = developingTime;

        DevelopingAction.EType currentType = DevelopingAction.EType.Rotate;
        int indexInScheme = 0;
        while (developingTime > 0)
        {
            if (indexInScheme == aggitationScheme.Length)
                indexInScheme = aggitationScheme.Length - 2;

            DevelopingAction action = new DevelopingAction(currentType, Math.Min(developingTime, aggitationScheme[indexInScheme]));
            actions.Add(action);

            developingTime -= action.Time;

            if (currentType == DevelopingAction.EType.Rotate)
                currentType = DevelopingAction.EType.Wait;
            else
                currentType = DevelopingAction.EType.Rotate;

            indexInScheme++;
        }

        actions.Add(new DevelopingAction(DevelopingAction.EType.Wash, washingTime));
        actions.Add(new DevelopingAction(DevelopingAction.EType.Fixing, fixingTime));
    }

    public void Update()
    {
        if (!started)
            return;

        float delta = started ? Time.deltaTime : 0.0f;
        remainTime -= delta;

        if (remainTime <= 0.0f && actionStep == actions.Count - 1)
        {
            StopTimer();
            return;
        }

        if (currentAction != null)
        {
            currentAction.Update(delta);
        }
    }

    public void OnActionStarted()
    {
        if (currentAction != null)
        {
            if (currentAction.Type == DevelopingAction.EType.Wash
                || currentAction.Type == DevelopingAction.EType.Fixing)
                remainTime = currentAction.Time;
        }
    }

    public void OnActionFinished()
    {
        actionStep++;

        if (started)
            StartNewAction();
    }

    public void StartTimer()
    {
        if (actions.Count == 0)
            return;

        started = true;
        StartNewAction();
    }

    public bool IsStarted()
    {
        return started;
    }

    public void StopTimer()
    {
        started = false;
        actionStep = 0;

        foreach (DevelopingAction action in actions)
        {
            if (action != null)
                action.Reset();
        }
    }


    private void StartNewAction()
    {
        if (actionStep > actions.Count - 1)
        {
            StopTimer();
            return;
        }

        currentAction = actions[actionStep];

        if (currentAction != null)
            currentAction.Start();
    }

    public float GetRemainTime()
    {
        return remainTime;
    }

    public int GetActionStep()
    {
        return actionStep;
    }

    public DevelopingAction.EType GetCurrentActionType()
    {
        return (currentAction != null) ? currentAction.Type : DevelopingAction.EType.Wait;
    }

    public String GetCurrentActionName()
    {
        if (currentAction != null)
        {
            switch (currentAction.Type)
            {
                case DevelopingAction.EType.Rotate: return "Rotate";
                case DevelopingAction.EType.Wait: return "Wait...";
                case DevelopingAction.EType.Wash: return "Washing";
                case DevelopingAction.EType.Fixing: return "Fixing";
            }
        }

        return "";
    }
}




