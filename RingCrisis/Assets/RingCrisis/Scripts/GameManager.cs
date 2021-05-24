using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace RingCrisis
{
    /// <summary>
    /// ゲームの全体的な管理を担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        private static readonly int ScoreToWin = 5;

        [SerializeField]
        private RoomInfoView _roomInfoView = null;

        [SerializeField]
        private Button _startGameButton = null;

        [SerializeField]
        private ConnectionManager _connectionManager = null;

        [SerializeField]
        private RpcManager _rpcManager = null;

        [SerializeField]
        private TeamComponentHolder _redTeamComponentHolder = null;

        [SerializeField]
        private TeamComponentHolder _blueTeamComponentHolder = null;

        [SerializeField]
        private RingShooter _ringShooter = null;

        [SerializeField]
        private TargetManager _targetManager = null;

        [SerializeField]
        private ResultView _resultView = null;

        private TeamColor _myTeamColor;
        private GameData _redGameData = new GameData(TeamColor.Red);
        private GameData _blueGameData = new GameData(TeamColor.Blue);

        public void Initialize(TeamColor myTeamColor)
        {
            _myTeamColor = myTeamColor;
            _ringShooter.Initialize(_myTeamColor);
        }

        public void AddScore(TeamColor teamColor, int score)
        {
            var gameData = GetTeamGameData(teamColor);
            gameData.Score += score;
            GetTeamComponentHolder(teamColor).ScoreText = gameData.Score.ToString();

            if (gameData.Score == ScoreToWin)
            {
                if (teamColor == _myTeamColor)
                {
                    _resultView.ShowWin();
                }
                else
                {
                    _resultView.ShowLose();
                }
                _targetManager.DeactivateSpawn();
            }
        }

        public GameData GetTeamGameData(TeamColor teamColor)
        {
            return teamColor == TeamColor.Red ? _redGameData : _blueGameData;
        }

        public TeamComponentHolder GetTeamComponentHolder(TeamColor teamColor)
        {
            return teamColor == TeamColor.Red ? _redTeamComponentHolder : _blueTeamComponentHolder;
        }

        private void UpdatePlayerList()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.IsMasterClient)
                {
                    // マスタークライアントはRed決め打ち
                    _roomInfoView.RedPlayerName = player.NickName;
                }
                else
                {
                    // ゲストクライアントはBlue決め打ち
                    _roomInfoView.BluePlayerName = player.NickName;
                }
            }
        }

        private void StartGame()
        {
            _startGameButton.gameObject.SetActive(false);
            _targetManager.ActivateSpawn();
        }

        private void Start()
        {
            _connectionManager.OnJoinedRoomEvent += room =>
            {
                _roomInfoView.RoomName = room.Name;
                UpdatePlayerList();

                if (PhotonNetwork.IsMasterClient)
                {
                    _startGameButton.gameObject.SetActive(true);
                    _startGameButton.onClick.AddListener(() => _rpcManager.SendStartGame());
                }
            };
            _connectionManager.OnPlayerEnteredEvent += player =>
            {
                UpdatePlayerList();
            };
            _connectionManager.OnPlayerLeftEvent += player =>
            {
                UpdatePlayerList();
            };

            _rpcManager.OnReceiveStartGame += () => StartGame();
        }
    }
}
