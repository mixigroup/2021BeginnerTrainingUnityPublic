using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCallback : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button _button;

    // Start is called before the first frame update
    void Start()
    {
        // ボタンが押されたとき呼び出すメソッドにOnButtonClicked()を追加する
        _button.onClick.AddListener(OnButtonClicked);


        //_button.onClick.AddListener(() => OnButtonClicked()); // ラムダでも
        //_button.onClick.AddListener(() => OnButtonClicked2(Input.mousePosition));　// こんなこともできます
    }

    void OnButtonClicked()
    {
        // ここを編集
        Debug.Log("Clicked.");
    }

    // こんなこともできます
    // void OnButtonClicked2(Vector3 pos)
    // {
    //     var textComponent = _button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    //     if(textComponent != null)
    //     {
    //         textComponent.text = pos.ToString();
    //     }
    // }
}
