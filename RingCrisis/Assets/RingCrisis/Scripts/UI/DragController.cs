using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RingCrisis
{
    /// <summary>
    /// スクリーン（UI）上のドラッグを検知し、XZ平面上のワールド座標に変換してイベントを発行するコンポーネント
    /// </summary>
    [DisallowMultipleComponent]
    public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action OnBeginDrag;

        public event Action<Vector3> OnDrag;

        public event Action<Vector3> OnEndDrag;

        private Vector3 _startPosition;

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = GetGroundPoint(eventData.position);
            OnBeginDrag?.Invoke();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag?.Invoke(GetGroundPoint(eventData.position) - _startPosition);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag?.Invoke(GetGroundPoint(eventData.position) - _startPosition);
        }

        /// <summary>
        /// スクリーン座標からゲームのフィールド（Y=0のXZ平面）上のワールド座標に変換する
        /// </summary>
        /// <param name="screenPoint">スクリーン座標</param>
        /// <returns>XZ平面Y=0上のワールド座標</returns>
        private Vector3 GetGroundPoint(Vector2 screenPoint)
        {
            var ray = RectTransformUtility.ScreenPointToRay(Camera.main, screenPoint);
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out var d))
            {
                return ray.GetPoint(d);
            }
            return Vector3.zero;
        }
    }
}
