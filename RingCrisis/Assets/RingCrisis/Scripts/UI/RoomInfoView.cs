using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace RingCrisis
{
    /// <summary>
    /// 左上のルーム名・プレイヤー名の表示を管理するコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class RoomInfoView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _roomNameText = null;

        [SerializeField]
        private TMP_Text _redPlayerNameText = null;

        [SerializeField]
        private TMP_Text _bluePlayerNameText = null;

        public string RoomName
        {
            get => _roomNameText.text;
            set => _roomNameText.text = value;
        }

        public string RedPlayerName
        {
            get => _redPlayerNameText.text;
            set => _redPlayerNameText.text = value;
        }

        public string BluePlayerName
        {
            get => _bluePlayerNameText.text;
            set => _bluePlayerNameText.text = value;
        }

        private void Awake()
        {
            Assert.IsNotNull(_roomNameText);
            Assert.IsNotNull(_redPlayerNameText);
            Assert.IsNotNull(_bluePlayerNameText);
        }
    }
}
