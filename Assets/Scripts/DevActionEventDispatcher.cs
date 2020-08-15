using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DevActionEventDispatcher
{
    private List<DevActionListener> listeners = new List<DevActionListener>();

    public void AddListener(DevActionListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void OnActionStarted()
    {
        if (listeners.Count > 0)
        {
            foreach (DevActionListener listener in listeners)
            {
                listener.OnActionStarted();
            }
        }
    }

    public void OnActionFinished()
    {
        if (listeners.Count > 0)
        {
            foreach (DevActionListener listener in listeners)
            {
                listener.OnActionFinished();
            }
        }
    }
}
