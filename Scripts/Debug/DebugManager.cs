using Battle;
using UnityEngine;

namespace DebugMode.Battle
{

    public class DebugManager : MonoBehaviour
    {
        [SerializeField] private Canvas _debugCanvas;

        private GameObject _managerGameObject;
        private BattleManager _battleManager;

        private void Start()
        {
            CanvasVisible(false);
            _managerGameObject = GameObject.Find("Manager");
            _managerGameObject.TryGetComponent(out _battleManager);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (_debugCanvas.enabled)
                {
                    CanvasVisible(false);
                    _battleManager.UIMode(false);
                }
                else
                {
                    CanvasVisible(true);
                    _battleManager.UIMode(true);
                }

            }

        }

        void CanvasVisible(bool set)
        {
            _debugCanvas.enabled = set;
        }

        public void SpawnEnemy(int spawnCount)
        {
            _battleManager.SpawnEnemy(spawnCount);
        }
    }

}