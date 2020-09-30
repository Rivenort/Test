using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Ties up everything needed to load a Scene.
/// @author Dominik
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public Slider loadingBar;
    [HideInInspector]
    public DAT_Game gameData;
    [HideInInspector]
    public int saveId = -1;

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsync(SceneManager.GetSceneByName(sceneName).buildIndex));
    }

    public void LoadLevel(DAT_Game levelData)
    {
        saveId = -1;
        this.gameData = levelData;
        LoadScene(1);
    }

    public void LoadSavedGame(int id)
    {
        saveId = id;
        LoadScene(1);
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }

            yield return null;
        }

    }
}
