using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private AsyncOperation _asyncOperation = null;
    private static readonly string[] Scene = {"Back Pack", "Camp", "recruit", "Tech Tree", "Shop", "Upgrade Building", "Equips"};
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
       SceneManager.LoadSceneAsync(Scene[0], LoadSceneMode.Additive);
    }

    public void UnloadBackPack()
    {
        SceneManager.UnloadSceneAsync(Scene[0]);
    }
    
    public void LoadCamp()
    {
        SceneManager.LoadSceneAsync(Scene[1], LoadSceneMode.Additive);
    }

    public void UnloadCamp()
    {
        SceneManager.UnloadSceneAsync(Scene[1]);
    }
    
    public void LoadRecruit()
    {
        SceneManager.LoadSceneAsync(Scene[2], LoadSceneMode.Additive);
    }

    public void UnloadRecruit()
    {
        SceneManager.UnloadSceneAsync(Scene[2]);
    }
    
    public void LoadTechTree()
    {
        SceneManager.LoadSceneAsync(Scene[3], LoadSceneMode.Additive);
    }

    public void UnloadTechTree()
    {
        SceneManager.UnloadSceneAsync(Scene[3]);
    }

    public void LoadShop()
    {
        SceneManager.LoadSceneAsync(Scene[4], LoadSceneMode.Additive);
    }
    
    public void UnloadShop()
    {
        SceneManager.UnloadSceneAsync(Scene[4]);
    }
    
    public void LoadUpgrade()
    {
        SceneManager.LoadSceneAsync(Scene[5], LoadSceneMode.Additive);
    }
    
    public void UnloadUpgrade()
    {
        SceneManager.UnloadSceneAsync(Scene[5]);
    }
    
    public void LoadEquip()
    {
        SceneManager.LoadSceneAsync(Scene[6], LoadSceneMode.Additive);
    }
    
    public void UnloadEquip()
    {
        SceneManager.UnloadSceneAsync(Scene[6]);
    }
}
