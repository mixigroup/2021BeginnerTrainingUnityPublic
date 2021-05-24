using TMPro;
using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// 1チームあたりのコンポーネントをまとめるためのコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class TeamComponentHolder : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _playerNameText = null;

        [SerializeField]
        private TMP_Text _scoreText = null;

        [SerializeField]
        private ShotSpace _shotSpace = null;

        public string PlayerName
        {
            get => _playerNameText.text;
            set => _playerNameText.text = value;
        }

        public string ScoreText
        {
            get => _scoreText.text;
            set => _scoreText.text = value;
        }

        public ShotSpace ShotSpace => _shotSpace;
    }
}
