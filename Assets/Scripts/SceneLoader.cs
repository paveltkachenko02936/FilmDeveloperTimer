
using UnityEngine;
using UnityEngine.SceneManagement;

class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        AppManager.Init();
        ChangeScene("SettingsScene");
    }

    public static void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
