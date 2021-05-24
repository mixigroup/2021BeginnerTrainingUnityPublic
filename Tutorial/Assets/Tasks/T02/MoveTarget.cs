using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    void Awake()
    {
        // オブジェクト初期化時に1度呼ばれる
    }

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクト生成時に1度呼ばれる
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトが存在している間毎フレーム呼ばれる
        var x = Mathf.Sin(Time.time) * 3.0f;
        var pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }
}
