using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace RingCrisis
{
    /// <summary>
    /// WIN/LOSEのアニメーション演出を管理するコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class ResultView : MonoBehaviour
    {
        [SerializeField]
        private Graphic _backImage = null; // GraphicクラスはImageやTMP_TextといったUIの抽象クラス

        [SerializeField]
        private Graphic _winText = null;

        [SerializeField]
        private Graphic _loseText = null;

        [SerializeField]
        private GameObject _fxWin = null;

        [SerializeField]
        private GameObject _fxLose = null;

        public void ShowWin()
        {
            _fxWin.SetActive(true);
            StartCoroutine(AnimateWin());
        }

        public void ShowLose()
        {
            _fxLose.SetActive(true);
            StartCoroutine(AnimateLose());
        }

        private IEnumerator AnimateWin()
        {
            return Animate(_winText, 0.5f);
        }

        private IEnumerator AnimateLose()
        {
            return Animate(_loseText, 0.5f);
        }

        private IEnumerator Animate(Graphic textObject, float duration)
        {
            _backImage.gameObject.SetActive(true);
            textObject.gameObject.SetActive(true);

            var t = 0.0f;
            while (t < duration)
            {
                t = Mathf.Min(t + Time.deltaTime, duration);
                {
                    var c = textObject.color;
                    c.a = Mathf.Lerp(0, 1, t / duration);
                    textObject.color = c;
                }
                {
                    var c = _backImage.color;
                    c.a = Mathf.Lerp(0, 0.8f, t / duration);
                    _backImage.color = c;
                }
                yield return null;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(_backImage);
            Assert.IsNotNull(_winText);
            Assert.IsNotNull(_loseText);
        }
    }
}
