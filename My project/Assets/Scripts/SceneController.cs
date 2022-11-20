using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private AsyncOperation _asyncOperation = null;
    private static string[] scene = {"Back Pack"};
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }

    private void ShowProgress()
    {
        // while (!_asyncOperation.isDone)
        // {
        //     if (_asyncOperation.progress < 0.9f)
        //     {
        //         Debug.Log((int)_asyncOperation.progress * 100 + "%");
        //     }
        //     else
        //     {
        //         Debug.Log("100%");
        //     }
        // }
        
        
    }

    public void LoadBackPack()
    {
       SceneManager.LoadSceneAsync(scene[0], LoadSceneMode.Additive);
    }

    public void UnloadBackPack()
    {
        SceneManager.UnloadSceneAsync(scene[0]);
    }
}
