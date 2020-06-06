using UnityEngine;

[System.Serializable]
public class DevelopingAction
{
    public float time;
    public string type;

    private bool active = false;
    private DevActionListener listener;

    private float remainTime = 0.0f;

    public DevelopingAction()
    {
    }

    public void SetListener(DevActionListener _listener)
    {
        listener = _listener;
    }

    public void Start()
    {
        if (active)
            return;

        remainTime = time;
        active = true;

        if (listener != null)
        {
            listener.OnActionStarted();
        }
    }

    public void Reset()
    {
        active = false;
        remainTime = time;
    }

    public void Update(float delta)
    {
        if (!active)
            return;

        remainTime -= delta;

        if (remainTime <= 0.0f)
        {
            Reset();

            if (listener != null)
            {
                listener.OnActionFinished();
            }
        }
    }

    public bool IsRunning()
    {
        return active;
    }
}
