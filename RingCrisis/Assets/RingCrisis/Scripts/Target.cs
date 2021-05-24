using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// ターゲットそのものの処理を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class Target : MonoBehaviour
    {
        [SerializeField]
        private int _score = 0;

        public int Score => _score;

        private void Update()
        {
            // FIXME: 一定時間経ったら自然消滅する
        }
    }
}
