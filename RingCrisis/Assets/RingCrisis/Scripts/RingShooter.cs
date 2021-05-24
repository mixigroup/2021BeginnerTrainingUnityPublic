using UnityEngine;
using UnityEngine.Assertions;

namespace RingCrisis
{
    /// <summary>
    /// リングを発射する処理を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class RingShooter : MonoBehaviour
    {
        private static readonly float VelocityY = 8.0f;

        [SerializeField]
        private Ring _ringPrefab = null;

        [SerializeField]
        private ShotSpace _shotSpaceRed = null;

        [SerializeField]
        private ShotSpace _shotSpaceBlue = null;

        [SerializeField]
        private DragController _dragController = null;

        [SerializeField]
        private Transform _curveObjectTransform = null;

        [SerializeField]
        private RpcManager _rpcManager = null;

        [SerializeField]
        private GameObject _fxDisappear = null;

        [SerializeField]
        private GameManager _gameManager = null;

        private Transform[] _curveNodes;

        private TeamColor _teamColor;

        public void Initialize(TeamColor teamColor)
        {
            _teamColor = teamColor;
            GetShotSpace(_teamColor).Activate();
            GetShotSpace(_teamColor.Opponent()).Deactivate();
        }

        private void Awake()
        {
            Assert.IsNotNull(_ringPrefab);
            Assert.IsNotNull(_shotSpaceRed);
            Assert.IsNotNull(_shotSpaceBlue);
            Assert.IsNotNull(_dragController);
            Assert.IsNotNull(_curveObjectTransform);
            Assert.IsNotNull(_rpcManager);
            Assert.IsNotNull(_fxDisappear);
            Assert.IsNotNull(_gameManager);

            _curveNodes = _curveObjectTransform.GetComponentsInChildren<Transform>();

            _dragController.OnBeginDrag += () =>
            {
                // 軌道ガイドを表示
                _curveObjectTransform.gameObject.SetActive(true);
            };

            _dragController.OnDrag += delta =>
            {
                var shotOrigin = GetShotSpace(_teamColor).ShotOriginPosition;
                var velocityXY = -delta; // ひっぱりと反対方向

                // 軌道ガイドの各点の座標を更新
                for (int i = 0; i < _curveNodes.Length; i++)
                {
                    _curveNodes[i].position = shotOrigin + CalculateTrajectoryPoint(velocityXY, i, _curveNodes.Length);
                }
            };

            _dragController.OnEndDrag += delta =>
            {
                var velocityXZ = -delta; // ひっぱりと反対方向

                // リングを発射
                ShootRing(_teamColor, velocityXZ); // <--- FIXME!!!

                // 軌道ガイドを非表示に
                _curveObjectTransform.gameObject.SetActive(false);
            };
        }

        /// <summary>
        /// XZ平面の初速度、点のインデックス、点の総数から、この点の座標を計算する
        /// </summary>
        /// <param name="velocityXZ">XZ平面における初速度</param>
        /// <param name="i">計算する点の番号（0〜n-1）</param>
        /// <param name="n">点の総数</param>
        /// <returns></returns>
        private Vector3 CalculateTrajectoryPoint(Vector3 velocityXZ, int i, int n)
        {
            return velocityXZ * i / (n - 1); // <--- FIXME!!!
        }

        private void ShootRing(TeamColor teamColor, Vector3 dir)
        {
            var shotSpace = GetShotSpace(teamColor);

            var ring = Instantiate(_ringPrefab);
            ring.transform.position = shotSpace.ShotOriginPosition;

            var rigidbody = ring.GetComponent<Rigidbody>();
            rigidbody.AddForce(dir + VelocityY * Vector3.up, ForceMode.VelocityChange);

            ring.Initialize(teamColor, target =>
            {
                _gameManager.AddScore(teamColor, target.Score);
                Destroy(target.gameObject);
                Destroy(ring.gameObject);
                Instantiate(_fxDisappear, target.transform.position, Quaternion.identity);
            });
        }

        private ShotSpace GetShotSpace(TeamColor teamColor)
        {
            return teamColor == TeamColor.Red ? _shotSpaceRed : _shotSpaceBlue;
        }
    }
}
