using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MergeHooks
{
    public class TouchPanel : SingletonMono<TouchPanel>
    {
        public UnityAction<PointerEventData> OnDragInit;
        public UnityAction<PointerEventData> OnDragBegin;
        public UnityAction<PointerEventData> OnDrag;
        public UnityAction<PointerEventData> OnDragEnd;
        public UnityAction<PointerEventData> OnPointUp;

        public void DragInit(BaseEventData base_event_data)
        {
            PointerEventData pointer_event_data = base_event_data as PointerEventData;
            OnDragInit?.Invoke(pointer_event_data);
        }

        public void DragBegin(BaseEventData base_event_data)
        {
            PointerEventData pointer_event_data = base_event_data as PointerEventData;
            OnDragBegin?.Invoke(pointer_event_data);
        }

        public void Drag(BaseEventData base_event_data)
        {
            PointerEventData pointer_event_data = base_event_data as PointerEventData;
            OnDrag?.Invoke(pointer_event_data);
        }

        public void DragEnd(BaseEventData base_event_data)
        {
            PointerEventData pointer_event_data = base_event_data as PointerEventData;
            OnDragEnd?.Invoke(pointer_event_data);
        }

        public void PointUp(BaseEventData base_event_data)
        {
            PointerEventData pointer_event_data = base_event_data as PointerEventData;
            OnPointUp?.Invoke(pointer_event_data);
        }
    }
}
