using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace RingCrisis
{
    /// <summary>
    /// リングそのものの処理を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class Ring : MonoBehaviour
    {
        [SerializeField]
        private Material _redMaterial = null;

        [SerializeField]
        private Material _blueMaterial = null;

        [SerializeField]
        private Renderer _renderer = null;

        [SerializeField]
        private GameObject _fxDisappear = null;

        // Rangeを使うとインスペクタ上で値の範囲制限ができる上、UIがスライダーになります
        [SerializeField, Range(0, 10)]
        private float _lifeTime = 5;

        private float _elapsedTime;
        private Action<Target> _onHitAction;

        public void Initialize(TeamColor teamColor, Action<Target> onHitAction)
        {
            if (teamColor == TeamColor.Red)
            {
                _renderer.material = _redMaterial;
            }
            else
            {
                _renderer.material = _blueMaterial;
            }

            _onHitAction = onHitAction;
        }

        private void Awake()
        {
            Assert.IsNotNull(_redMaterial);
            Assert.IsNotNull(_blueMaterial);
            Assert.IsNotNull(_renderer);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > _lifeTime)
            {
                Destroy(gameObject);
                Instantiate(_fxDisappear, transform.position, transform.rotation);
            }
        }

        // このヒエラルキー以下にある Is Trigger なColliderに別のColliderが進入したときに呼び出される
        private void OnTriggerEnter(Collider collider)
        {
            // 予めターゲットの根本にレイヤーをTargetにしたColliderオブジェクトを設置している
            if (collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                var target = collider.GetComponentInParent<Target>();
                _onHitAction?.Invoke(target);
            }
        }
    }
}
