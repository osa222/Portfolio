using UnityEngine.UI;
using UnityEngine;
using UniRx;

namespace DebugMode.Battle
{

    public class EffectScalerView : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<Vector3> CurrentScale => _scaleRP;
        private ReactiveProperty<Vector3> _scaleRP = new ReactiveProperty<Vector3>();

        [SerializeField] private InputField _x;
        [SerializeField] private InputField _y;
        [SerializeField] private InputField _z;

        private void Start()
        {
            _scaleRP.AddTo(this);

            Observable.Merge(
                _x.OnValueChangedAsObservable(),
                _y.OnValueChangedAsObservable(),
                _z.OnValueChangedAsObservable()).
                Subscribe(_ =>
                {
                    var x = float.Parse(_x.text);
                    var y = float.Parse(_x.text);
                    var z = float.Parse(_x.text);

                    _scaleRP.Value = new Vector3(x, y, z);
                }).AddTo(this);
        }

        public void SetScale(Vector3 scale)
        {
            _x.text = scale.x.ToString();
            _y.text = scale.y.ToString();
            _z.text = scale.z.ToString();
        }
    }

}