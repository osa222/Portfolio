using System;
using UnityEngine;
/// <summary>
/// 参考
/// https://github.com/TORISOUP/SceneTransitionSample
/// </summary>
public static class TransitionManager
{
    private static Lazy<SceneTransitionManager> _sceneTransitionManager = new Lazy<SceneTransitionManager>(() =>
    {
        var r = Resources.Load("TransitionCotroller") as GameObject;
        var obj = GameObject.Instantiate(r) as GameObject;
        UnityEngine.Object.DontDestroyOnLoad(obj);
        return obj.GetComponent<SceneTransitionManager>();
    });


    private static SceneTransitionManager SceneTransitionManager => _sceneTransitionManager.Value;


    public static void LoadScene(string sceneName, Action action)
    {
        SceneTransitionManager.Transition(sceneName, action);
    }
}