using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntryPoint
{
    private static GameEntryPoint _instance;
    private Coroutines _coroutines;
    private UIViewRoot _uiRoot;

    private GameEntryPoint()
    {
        _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        Object.DontDestroyOnLoad(_coroutines.gameObject);

        //var prefabUIRoot = Resources.Load<UIViewRoot>("Prefabs/UI/UIRoot");
        //_uiRoot = Object.Instantiate(prefabUIRoot);
        //Object.DontDestroyOnLoad(prefabUIRoot.gameObject);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutoStartGame()
    {
        _instance = new GameEntryPoint();
        _instance.RunGame();
    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == Scenes.ENTRY_SCENE)
        {
            return;
        }

        if (sceneName == Scenes.BOOT)
        {
            _coroutines.StartCoroutine(LoadAndStartGameplay());
            return;
        }
#endif  
        _coroutines.StartCoroutine(LoadAndStartGameplay());
    }

    private IEnumerator LoadAndStartGameplay()
    {
        //_uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.ENTRY_SCENE);

        //_uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
