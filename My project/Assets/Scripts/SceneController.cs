using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private AsyncOperation _asyncOperation = null;
    private static string[] scene = {"Back Pack", "Camp", "recruit", "Tech Tree", "shop"};
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
    
    public void LoadCamp()
    {
        SceneManager.LoadSceneAsync(scene[1], LoadSceneMode.Additive);
    }

    public void UnloadCamp()
    {
        SceneManager.UnloadSceneAsync(scene[1]);
    }
    
    public void LoadRecruit()
    {
        SceneManager.LoadSceneAsync(scene[2], LoadSceneMode.Additive);
    }

    public void UnloadRecruit()
    {
        SceneManager.UnloadSceneAsync(scene[2]);
    }
    
    public void LoadTechTree()
    {
        SceneManager.LoadSceneAsync(scene[3], LoadSceneMode.Additive);
    }

    public void UnloadTechTree()
    {
        SceneManager.UnloadSceneAsync(scene[3]);
    }

    public void LoadShop()
    {
        SceneManager.LoadSceneAsync(scene[4]);
    }
    
    public void UnloadShop()
    {
        SceneManager.UnloadSceneAsync(scene[4]);
    }
}
