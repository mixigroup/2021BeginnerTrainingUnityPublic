using System;
using Photon.Pun;
using UnityEngine;

namespace RingCrisis
{
    /// <summary>
    /// PhotonのRPC送受信を担うコンポーネント
    /// RequireComponent属性を使うと他のコンポーネントが必須であることを示すことができ、アタッチ時にその存在がチェックされるようになる
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
    public class RpcManager : MonoBehaviour
    {
        public event Action OnReceiveStartGame;

        private PhotonView _photonView;

        public void SendStartGame()
        {
            // "StartGame" と書く代わりに nameof(StartGame) とすると、メソッド名の変更に対して保守性が高まります
            // https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/nameof
            _photonView.RPC(nameof(StartGame), RpcTarget.AllViaServer);
        }

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        [PunRPC]
        private void StartGame()
        {
            OnReceiveStartGame?.Invoke();
        }
    }
}
