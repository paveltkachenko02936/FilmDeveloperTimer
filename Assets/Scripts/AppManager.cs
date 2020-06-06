using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class AppManager : MonoBehaviour, DevActionListener
{
    private static AppManager instance;

    private static Text timeText;
    private static GameObject actionBtn;
    private static GameObject closeBtn;
    private static GameObject rotateImg;
    private static GameObject pauseImg;
    private static AudioSource audioSource;

    private static List<DevelopingAction> actions;
    private static DevelopingAction currentAction;

    private static float remainTime = 0.0f;
    private static float totalTime = 0.0f;
    private static int actionStep = 0;
    private static bool started = false;

    private AppManager() : base()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = new AppManager();
        Init();
    }

    private void Init()
    {
        CreateActions();

        timeText = GameObject.Find("timeText").GetComponent<Text>();
        actionBtn = GameObject.Find("actionBtn");
        closeBtn = GameObject.Find("closeBtn");

        rotateImg = GameObject.Find("rotateImg");
        pauseImg = GameObject.Find("pauseImg");

        audioSource = GetComponent<AudioSource>();
    }

    private void UpdateUI()
    {
        if (closeBtn != null)
            closeBtn.SetActive(started);

        if (actionBtn != null)
            actionBtn.SetActive(!started);

        if (rotateImg != null && pauseImg != null)
        {
            if (currentAction != null && currentAction.IsRunning())
            {
                string actionType = currentAction.type;
                rotateImg.SetActive(actionType == "rotate");
                pauseImg.SetActive(actionType == "calm");
            }
            else
            {
                rotateImg.SetActive(false);
                pauseImg.SetActive(false);
            }
        }
    }


    private void Update()
    {
        float delta = started ? Time.deltaTime : 0.0f;
        UpdateTime(delta);
        UpdateUI();

        if (currentAction != null)
        {
            currentAction.Update(delta);
        }
    }

    private void UpdateTime(float delta = 0.0f)
    {
        remainTime -= delta;

        if (remainTime <= 0.0f)
        {
            StopTimer();
            return;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(remainTime);
        string timeStr = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        if (timeText)
            timeText.text = timeStr;
    }

    private void CreateActions()
    {
        string filepath = "";

#if UNITY_ANDROID
        string path = "jar:file://" + Application.dataPath + "!/assets/program.json";
        WWW wwwfile = new WWW(path);
        while (!wwwfile.isDone) { }
        filepath = string.Format("{0}/{1}", Application.persistentDataPath, "program.json");
        File.WriteAllBytes(filepath, wwwfile.bytes);
#endif
        //--------- вариант для редактора---------------------
        //filepath = Path.Combine(Application.streamingAssetsPath, "program.json");
        //----------------------------------------------

        StreamReader reader = new StreamReader(filepath);
        string json = reader.ReadToEnd();
        if (json.Length > 0)
        {
            DevelopingAction[] result = JsonHelper.getJsonArray<DevelopingAction>(json);
            actions = result.ToList<DevelopingAction>();
        }

        if (actions != null && actions.Count > 0)
        {
            foreach (DevelopingAction action in actions)
            {
                action.SetListener(GetInstance());
                totalTime += action.time;
            }
        }

        remainTime = totalTime;
    }

    public static AppManager GetInstance()
    {
        return instance;
    }

    public void StartTimer()
    {
        started = true;
        StartNewAction();
    }

    void OnDisable()
    {
        StopTimer();
    }

    public void StopTimer()
    {
        started = false;
        actionStep = 0;
        remainTime = totalTime;

        foreach (DevelopingAction action in actions)
        {
            if (action != null)
                action.Reset();
        }
    }

    public void OnActionStarted()
    {
        if (audioSource != null && actionStep == 0)
            audioSource.Play();
    }

    public void OnActionFinished()
    {
        if (audioSource != null)
            audioSource.Play();

        actionStep++;
        StartNewAction();
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
}
