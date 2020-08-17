using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DevelopingAction
{
    private readonly int time;
    private readonly EType type;
    private bool active = false;
    private float remainTime = 0.0f;

    private DevActionEventDispatcher dispatcher;

    public enum EType
    {
        Rotate,
        Wait,
        Wash,
        Fixing
    }

    public DevelopingAction(EType _type, int _time)
    {
        type = _type;
        time = _time;

        dispatcher = AppManager.GetInstance().EventDispatcher;
    }

    public EType Type { get => type; }
    public int Time { get => time; }

    public void Start()
    {
        if (active)
            return;

        remainTime = time;
        active = true;

        if (dispatcher != null)
            dispatcher.OnActionStarted();
    }

    public void Finish()
    {
        active = false;

        if (dispatcher != null)
            dispatcher.OnActionFinished();

        remainTime = time;
    }

    public void Update(float delta)
    {
        if (!active)
            return;

        remainTime -= delta;

        if (remainTime <= 0.0f)
        {
            Finish();
        }
    }
}
