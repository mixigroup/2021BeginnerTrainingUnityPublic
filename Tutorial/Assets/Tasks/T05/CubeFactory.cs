using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CubeFactory : MonoBehaviour
{
    [SerializeField]
    private GameObject _cube;

    [SerializeField]
    private Fader _fader;

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private float _fadeTime = 1.0f;

    [SerializeField]
    private float _waitTime = 0.0f;

    private float _time = 0.0f;

    private bool _isFade = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 非同期処理とわかりやすいよう、秒数カウント表示
        _time += Time.deltaTime;
        _text.text = $"Time: {_time:F2}";
        _text.color = _isFade ? Color.red : Color.white;

        if (!_isFade && Input.GetMouseButtonDown(0))
        {
            _time = 0;
            StartCoroutine(FadeAndFactory());
        }
    }

    IEnumerator FadeAndFactory()
    {
        _isFade = true;
        // フェードイン開始
        yield return _fader.FadeIn(_fadeTime);

        // フェードイン終了を待って、Cubeをランダム個生成
        var cnt = Random.Range(1, 10);
        FactoryCube(cnt);

        // フェードアウト開始まで指定秒待機
        yield return _fader.FadeWait(_waitTime);

        // フェードアウト開始
        yield return _fader.FadeOut(_fadeTime);

        // フェードアウト終了
        _isFade = false;
    }

    void FactoryCube(int cnt)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject); // 子供全削除
        }

        for (int i = 0; i < cnt; i++)
        {
            var obj = Instantiate(_cube, parent: this.transform); // 後で一括削除したいので生成したオブジェクトを子供にしておく
            obj.transform.position = new Vector3(
                Random.Range(-10.0f, 10.0f),
                Random.Range(-10.0f, 10.0f),
                Random.Range(-10.0f, 10.0f)
            );
        }
    }
}
