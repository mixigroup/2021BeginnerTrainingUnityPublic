using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryMoveTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject _moveTarget;

    // 最大生成数
    public int _maxCreateCnt = 10;
    // 生成間隔
    public float _createInterval = 1.0f;

    private float _factoryTime;
    private float _factoryCnt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_factoryCnt > _maxCreateCnt)
            return;

        _factoryTime += Time.deltaTime;
        if (_factoryTime < _createInterval)
            return;

        var obj = Instantiate(_moveTarget);
        // obj : 生成したGameObject
        // [ヒント]このobjに対してｺﾞﾆｮｺﾞﾆｮすると…
        _factoryTime = 0;
        _factoryCnt++;
    }
}
