using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FFramework
{
    /// <summary>
    /// put the SwipeBegin,Swiping,SwipeEnd methon on the right place and add a listener on  OnSwipeGestureTakeEffect
    /// </summary>
    public class SwipeGesture : MonoBehaviour, IPointerUpHandler, IDragHandler, IBeginDragHandler
    {
        public enum SwipeDirection
        {
            UP, DOWN, LEFT, RIGHT
        }

        public SwipeGesture(float take_effect_distance)
        {
            this.Take_effect_distance = take_effect_distance;
        }

        public UnityAction<SwipeDirection> OnSwipeGestureTakeEffect;

        [SerializeField]
        float take_effect_distance;
        Vector2 init_pos;
        Vector2 last_direction;

        public float Take_effect_distance { get => take_effect_distance; set => take_effect_distance = value; }

        public void SwipeBegin(PointerEventData pointer_event_data)
        {
            init_pos = pointer_event_data.position;
        }

        public void Swiping(PointerEventData pointer_event_data)
        {
            //变向则重置init_pos
            if (Vector2.Dot(pointer_event_data.delta, last_direction) < 0)
            {
                init_pos = pointer_event_data.position;
            }
            last_direction = pointer_event_data.delta;

            Vector2 swiping_direction = pointer_event_data.position - init_pos;

            if (Mathf.Abs(swiping_direction.x) > Take_effect_distance || Mathf.Abs(swiping_direction.y) > Take_effect_distance)
            {
                //重置锚定位置
                init_pos = pointer_event_data.position;

                //判断水平还是垂直
                if (Mathf.Abs(swiping_direction.x) >= Mathf.Abs(swiping_direction.y))
                {
                    //判断正负方向
                    if (swiping_direction.x > 0)
                    {
                        OnSwipeGestureTakeEffect?.Invoke(SwipeDirection.RIGHT);
                    }
                    else
                    {
                        OnSwipeGestureTakeEffect?.Invoke(SwipeDirection.LEFT);
                    }
                }
                else
                {
                    //判断正负方向
                    if (swiping_direction.y > 0)
                    {
                        OnSwipeGestureTakeEffect?.Invoke(SwipeDirection.UP);
                    }
                    else
                    {
                        OnSwipeGestureTakeEffect?.Invoke(SwipeDirection.DOWN);
                    }
                }

            }
        }

        public void SwipeEnd(PointerEventData pointer_event_data)
        {
            init_pos = Vector2.zero;
            last_direction = Vector2.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SwipeBegin(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Swiping(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SwipeEnd(eventData);
        }
    }
}
