using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{

    public class ProcessingScene : MonoBehaviour, DevActionListener
    {
        private Text timeText;
        private Text actionText;
        private Button actionBtn;
        private Button closeBtn;

        private GameObject rotateImg;
        private GameObject pauseImg;
        private GameObject washImg;
        private GameObject fixingImg;

        private AudioSource audioSource;

        private DevActionsManager actionsManager;

        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Loaded ProcessScene");
            Init();
        }

        private void Init()
        {
            timeText = GameObject.Find("timeText").GetComponent<Text>();
            actionText = GameObject.Find("actionText").GetComponent<Text>();

            actionBtn = GameObject.Find("actionBtn").GetComponent<Button>();
            if (actionBtn != null)
                actionBtn.onClick.AddListener(OnStartClick);

            closeBtn = GameObject.Find("closeBtn").GetComponent<Button>();
            if (closeBtn != null)
                closeBtn.onClick.AddListener(OnStopClick);

            rotateImg = GameObject.Find("rotateImg");
            pauseImg = GameObject.Find("pauseImg");
            washImg = GameObject.Find("washImg");
            fixingImg = GameObject.Find("fixingImg");

            audioSource = GetComponent<AudioSource>();

            actionsManager = AppManager.GetInstance().ActionsManager;
            if (actionsManager != null)
                actionsManager.CreateActions();

            DevActionEventDispatcher eventDispatcher = AppManager.GetInstance().EventDispatcher;
            if (eventDispatcher != null)
                eventDispatcher.AddListener(this);
        }

        private void UpdateUI()
        {
            if (actionsManager == null)
                return;

            if (closeBtn != null)
                closeBtn.gameObject.SetActive(actionsManager.IsStarted());

            if (actionBtn != null)
                actionBtn.gameObject.SetActive(!actionsManager.IsStarted());

            if (actionsManager.IsStarted())
            {
                DevelopingAction.EType type = actionsManager.GetCurrentActionType();
                rotateImg.SetActive(type == DevelopingAction.EType.Rotate);
                pauseImg.SetActive(type == DevelopingAction.EType.Wait);
                washImg.SetActive(type == DevelopingAction.EType.Wash);
                fixingImg.SetActive(type == DevelopingAction.EType.Fixing);
                actionText.text = actionsManager.GetCurrentActionName();
            }
            else
            {
                rotateImg.SetActive(false);
                pauseImg.SetActive(false);
                washImg.SetActive(false);
                fixingImg.SetActive(false);
                actionText.text = "";
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(actionsManager.GetRemainTime());
            string timeStr = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
            if (timeText != null)
                timeText.text = timeStr;
        }


        private void Update()
        {
            if (actionsManager != null)
                actionsManager.Update();

            UpdateUI();
        }

        void OnDisable()
        {
            if (actionsManager != null)
                actionsManager.StopTimer();
        }

        private void OnStartClick()
        {
            if (actionsManager != null)
                actionsManager.StartTimer();
        }

        private void OnStopClick()
        {
            if (actionsManager != null)
            {
                actionsManager.StopTimer();
                AppManager.Reset();
            }
        }

        public void OnActionStarted()
        {
            if (audioSource != null && actionsManager.GetActionStep() == 0)
                audioSource.Play();
        }

        public void OnActionFinished()
        {
            if (audioSource != null)
                audioSource.Play();

            if (!actionsManager.IsStarted())
                AppManager.Reset();
        }

    }
}
