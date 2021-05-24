using UnityEngine;
using UnityEngine.Assertions;

namespace RingCrisis
{
    /// <summary>
    /// ターゲットの生成を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class TargetManager : MonoBehaviour
    {
        [SerializeField]
        private RpcManager _rpcManager = null;

        [SerializeField]
        private Target _targetPrefab = null;

        [SerializeField]
        private GameObject _fxSpawn = null;

        private bool _activated;

        public void ActivateSpawn()
        {
            _activated = true;

            SpawnTarget();
        }

        public void DeactivateSpawn()
        {
            _activated = false;
        }

        private void Awake()
        {
            Assert.IsNotNull(_rpcManager);
            Assert.IsNotNull(_targetPrefab);
            Assert.IsNotNull(_fxSpawn);
        }

        private void Update()
        {
            if (!_activated)
            {
                return;
            }

            // FIXME: 一定時間ごとにターゲットを生成する
        }

        private void SpawnTarget()
        {
            // FIXME!!!
            SpawnTargetLocal(new Vector3(0, 0, 0));
        }

        private void SpawnTargetLocal(Vector3 worldPosition)
        {
            Instantiate(_targetPrefab, worldPosition, Quaternion.identity);
            Instantiate(_fxSpawn, worldPosition, Quaternion.identity);
        }
    }
}
