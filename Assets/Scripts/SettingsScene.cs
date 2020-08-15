using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SettingsScene: MonoBehaviour
{
    private Text timeText;
    private Text headerText;
    private Button backBtn;
    private Button applyBtn;
    private Button minusBtn;
    private Button plusBtn;

    private SettingsManager settingsManager;

    void Start()
    {
        Debug.Log("Loaded SettingsScene");
        Init();
    }

    private void Init()
    {
        timeText = GameObject.Find("timeText").GetComponent<Text>();
        headerText = GameObject.Find("headerText").GetComponent<Text>();

        backBtn = GameObject.Find("backBtn").GetComponent<Button>();
        if (backBtn != null)
            backBtn.onClick.AddListener(OnBackClick);

        applyBtn = GameObject.Find("applyBtn").GetComponent<Button>();
        if (applyBtn != null)
            applyBtn.onClick.AddListener(OnApplyClick);

        minusBtn = GameObject.Find("minusBtn").GetComponent<Button>();
        if (minusBtn != null)
            minusBtn.onClick.AddListener(OnMinusClick);

        plusBtn = GameObject.Find("plusBtn").GetComponent<Button>();
        if(plusBtn != null)
            plusBtn.onClick.AddListener(OnPlusClick);

        settingsManager = AppManager.GetInstance().SettingsManager;

        UpdateUI();
    }

    private void OnPlusClick()
    {
        if (settingsManager != null)
            settingsManager.IncreaseTime();

        UpdateUI();
    }

    private void OnMinusClick()
    {
        if (settingsManager != null)
            settingsManager.DecreaseTime();

        UpdateUI();
    }

    private void OnApplyClick()
    {
        if (settingsManager != null)
        { 
            settingsManager.GoToNextMode();
        }

        UpdateUI();
    }

    private void OnBackClick()
    {
        if (settingsManager != null)
            settingsManager.GoToPreviousMode();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (settingsManager == null)
            return;

        // timer
        TimeSpan timeSpan = TimeSpan.FromSeconds(settingsManager.GetTime());
        string timeStr = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        if (timeText != null)
            timeText.text = timeStr;

        // header
        if (headerText != null)
            headerText.text = settingsManager.GetHeader();

        // buttons
        if (backBtn != null)
            backBtn.gameObject.SetActive(settingsManager.CanShowPrevSetting());
    }
}
