using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    private Image _fadeImage;

    // Start is called before the first frame update
    void Start()
    {
        _fadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeIn(float duration)
    {
        yield return Fade(0, 1, duration); // IEnumeratorが返り値の処理が完了するまで待つ
    }

    public IEnumerator FadeOut(float duration)
    {
        yield return Fade(1, 0, duration); // IEnumeratorが返り値の処理が完了するまで待つ
    }
    public IEnumerator FadeWait(float wait)
    {
        yield return new WaitForSeconds(wait); // 指定秒数待つ
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        var t = 0.0f;
        while (t < duration)
        {
            t = Mathf.Min(t + Time.deltaTime, duration);
            var c = _fadeImage.color;
            c.a = Mathf.Lerp(from, to, t / duration);
            _fadeImage.color = c;
            yield return null; // 次のフレームまで待つ。whileと組み合わせることで実質Updateと同じ挙動。
        }
    }
}
