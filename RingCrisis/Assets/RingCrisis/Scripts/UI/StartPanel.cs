using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace RingCrisis
{
    /// <summary>
    /// 最初の画面
    /// </summary>
    [DisallowMultipleComponent]
    public class StartPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _playerNameInputField = null;

        [SerializeField]
        private TMP_InputField _roomNameInputField = null;

        [SerializeField]
        private GameObject _uiRoot = null;

        [SerializeField]
        private Button _startButton = null;

        [SerializeField]
        private FadeImageController _fadeImageController = null;

        [SerializeField]
        private ConnectionManager _connectionManager = null;

        [SerializeField]
        private GameManager _gameManager = null;

        private void Awake()
        {
            Assert.IsNotNull(_playerNameInputField);
            Assert.IsNotNull(_roomNameInputField);
            Assert.IsNotNull(_uiRoot);
            Assert.IsNotNull(_startButton);
            Assert.IsNotNull(_fadeImageController);
            Assert.IsNotNull(_connectionManager);
            Assert.IsNotNull(_gameManager);

            // デフォルトのプレイヤーネーム
            _playerNameInputField.text = $"player-{Random.Range(100, 1000):D03}";

            // デフォルトのルーム名
            _roomNameInputField.text = $"room-{Random.Range(100, 1000):D03}";

            _startButton.onClick.AddListener(() => OnStartButtonClicked());

            _uiRoot.SetActive(true);
        }

        private void OnStartButtonClicked()
        {
            _startButton.interactable = false;

            var nickName = _playerNameInputField.text;
            var roomName = _roomNameInputField.text;

            _connectionManager.Connect(nickName, roomName,
                () =>
                {
                    StartCoroutine(Transition());
                },
                errorMessage =>
                {
                    Debug.LogError(errorMessage);
                }
            );
        }

        private IEnumerator Transition()
        {
            yield return _fadeImageController.AnimateDissolveLevel(1, 0, 0.6f);
            yield return new WaitForSeconds(0.5f);

            _uiRoot.SetActive(false);

            // チームカラーを決定して初期化
            _gameManager.Initialize(PhotonNetwork.IsMasterClient ? TeamColor.Red : TeamColor.Blue);

            yield return _fadeImageController.AnimateDissolveLevel(0, 1, 0.6f);
        }
    }
}
