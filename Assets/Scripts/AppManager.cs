using UnityEngine.SceneManagement;

public class AppManager
{
    private static AppManager instance;
    private static DevActionEventDispatcher dispatcher;
    private static SettingsManager settingsManager;
    private static DevActionsManager actionsManager;

    private AppManager() { }

    public static void Init()
    {
        instance = new AppManager();
        dispatcher = new DevActionEventDispatcher();
        settingsManager = new SettingsManager();
        actionsManager = new DevActionsManager();
    }

    public static AppManager GetInstance()
    {
        return instance;
    }

    public static void Reset()
    {
        actionsManager = null;
        settingsManager = null;
        dispatcher = null;
        instance = null;

        Init();
        SceneManager.LoadScene("SettingsScene");
    }

    public SettingsManager SettingsManager { get => settingsManager; }
    public DevActionsManager ActionsManager { get => actionsManager; }
    public DevActionEventDispatcher EventDispatcher { get => dispatcher; }
}
