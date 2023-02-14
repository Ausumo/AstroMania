using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Slider _loadingSlider;


    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsnyc(sceneIndex));
        
    }


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
