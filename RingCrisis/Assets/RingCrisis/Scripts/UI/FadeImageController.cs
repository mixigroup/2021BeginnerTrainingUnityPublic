using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace RingCrisis
{
    [DisallowMultipleComponent]
    public class FadeImageController : MonoBehaviour
    {
        [SerializeField]
        private Image _image = null;

        [SerializeField, Range(0, 1)]
        private float _dissolveLevel = 0;

        private int _shaderPropertyDissolveLevel;

        public float DissolveLevel
        {
            get => _dissolveLevel;
            set => _dissolveLevel = Mathf.Clamp01(value);
        }

        public IEnumerator AnimateDissolveLevel(float from, float to, float duration)
        {
            var time = 0.0f;
            while (time < duration)
            {
                time = Mathf.Min(time + Time.deltaTime, duration);
                DissolveLevel = Mathf.Lerp(from, to, time / duration);
                yield return null;
            }
        }

        private void Awake()
        {
            Assert.IsNotNull(_image);

            // シェーダープロパティにアクセスする際、プロパティ名でも可能だが事前に数値IDにしておくとちょっｔ効率が良い
            _shaderPropertyDissolveLevel = Shader.PropertyToID("_DissolveLevel");
        }

        private void Update()
        {
            _image.material.SetFloat(_shaderPropertyDissolveLevel, _dissolveLevel);
        }
    }
}
