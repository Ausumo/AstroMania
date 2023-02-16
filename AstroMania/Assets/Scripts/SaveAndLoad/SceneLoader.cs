using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Slider _loadingSlider;

    /// <summary>
    /// Führt eine Coroutine aus die die nächste Scene lädt
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsnyc(sceneIndex));
    }

    /// <summary>
    /// Solange die nächste Scene geladen wird wird die Loading Bar den Fortschritt anzeigen
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    IEnumerator LoadAsnyc(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            _loadingSlider.value = progress;

            yield return null;
        }
    }
}
