using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Settings _settings;

    public void Transition(string sceneName, Action action)
    {
        var token = this.GetCancellationTokenOnDestroy();
        TransitionAsync(sceneName, action, token).Forget();
    }

    private async UniTaskVoid TransitionAsync(string sceneName, Action action, CancellationToken token)
    {

        await FadeInAsync(token);
        await SceneManager.LoadSceneAsync(sceneName).ToUniTask(cancellationToken: token);
        await FadeOutAsync(token);
        // クリックイベントのブロック解除
        _image.raycastTarget = false;
        action();
    }

    private async UniTask FadeInAsync(CancellationToken token)
    {
        float current = 0;
        while (current <= _settings.FadeInDuration)
        {
            current += Time.deltaTime;
            var color = _image.color;
            color.a = current / _settings.FadeInDuration;
            _image.color = color;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    private async UniTask FadeOutAsync(CancellationToken token)
    {
        float current = 0;
        while (current <= _settings.FadeOutDuration)
        {
            current += Time.deltaTime;
            var color = _image.color;
            color.a = 1 - (current / _settings.FadeOutDuration);
            _image.color = color;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    public void Transition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    [Serializable]
    public class Settings
    {
        public Sprite Cover;
        public float FadeInDuration = 1.0f;
        public float FadeOutDuration = 1.0f;
    }
}
