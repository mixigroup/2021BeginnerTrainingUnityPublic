using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// 1チームに対する視点・発射方向を管理するためのコンポーネント
    /// このゲームはチームによってフィールドのどちらかの端からリングを投げ入れるので、カメラと発射位置をチームごとに用意している
    /// </summary>
    [DisallowMultipleComponent]
    public class ShotSpace : MonoBehaviour
    {
        [SerializeField]
        private Camera _mainCamera = null;

        [SerializeField]
        private Transform _shotOrigin = null;

        public Vector3 ShotOriginPosition => _shotOrigin.position;

        public void Activate()
        {
            _mainCamera.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _mainCamera.gameObject.SetActive(false);
        }
    }
}
