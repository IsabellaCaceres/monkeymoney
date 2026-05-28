using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoveButton : MonoBehaviour
{
    [Header("Scene to Load")]
    public string dateSceneName = "MonkeyDateScene";

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            LoadDateScene();

        }

    }

    private void LoadDateScene()
    {
        SceneManager.LoadScene(dateSceneName);
    }
}
