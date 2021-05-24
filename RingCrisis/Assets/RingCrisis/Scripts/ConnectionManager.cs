using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// Photonの接続周りの処理をコールバックを担うコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class ConnectionManager : MonoBehaviourPunCallbacks
    {
        public event Action<Room> OnJoinedRoomEvent;
        public event Action<Player> OnPlayerEnteredEvent;
        public event Action<Player> OnPlayerLeftEvent;

        private string _roomName;
        private Action _onSuccess;
        private Action<string> _onError;

        public void Connect(string nickName, string roomName, Action onSuccess, Action<string> onError)
        {
            _roomName = roomName;
            _onSuccess = onSuccess;
            _onError = onError;

            PhotonNetwork.NickName = nickName;

            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                JoinOrCreateRoom();
            }
        }

        private void JoinOrCreateRoom()
        {
            PhotonNetwork.JoinOrCreateRoom(_roomName, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
        }

        public override void OnConnectedToMaster()
        {
            JoinOrCreateRoom();
        }

        public override void OnJoinedRoom()
        {
            _onSuccess?.Invoke();
            OnJoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _onError?.Invoke($"CreateRoomFailed: {message} ({returnCode})");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _onError?.Invoke($"JoinRoomFailed: {message} ({returnCode})");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if (cause != DisconnectCause.None)
            {
                _onError?.Invoke($"Disconnected: {cause}");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerEnteredEvent?.Invoke(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPlayerLeftEvent?.Invoke(otherPlayer);
        }

        private void OnDestroy()
        {
            Debug.Log("Disconnect");
            PhotonNetwork.Disconnect();
        }
    }
}
