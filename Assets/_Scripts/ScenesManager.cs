using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{

    public static ScenesManager Instance;

    public enum TypeScene
    {
        Home, SelectLevel, Main
    }

    [System.Serializable]
    public struct Scenes
    {
        public TypeScene type;
    }

    void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _currentScenes = 0;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && _currentScenes == 0)
        {
            Application.Quit();
        }
    }

    public Scenes[] _secenes;
    public int _currentScenes;

    public void GoToScene(TypeScene typeScene, UnityAction actionLoadScenesDone = null)
    {
        StartCoroutine(GoToSceneHandel(typeScene, actionLoadScenesDone));
    }

    public void GoToSceneSelectLevel()
    {
        GoToScene(TypeScene.SelectLevel);
    }

    private IEnumerator GoToSceneHandel(TypeScene typeScene, UnityAction actionLoadScenesDone = null)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitUntil(() => 2 == 2);

        for (int i = 0; i < _secenes.Length; i++)
        {
            if (_secenes[i].type == typeScene)
            {
                _currentScenes = i;
                var _sceneLoading = SceneManager.LoadSceneAsync(_currentScenes, LoadSceneMode.Additive);
                yield return _sceneLoading;
                Scene scene = SceneManager.GetSceneByBuildIndex(_currentScenes);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                SceneManager.SetActiveScene(scene);
                break;
            }
        }

        if (actionLoadScenesDone != null)
            actionLoadScenesDone();

        yield return new WaitForSeconds(0.02f);
        //Fade.Instance.EndFade();
    }
}
