using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MergeHooks
{
    [Serializable]
    public class ObservableData<T> : IObservableData<T>
    {
        protected T value;
        protected UnityAction<T> onValueChanged;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }

        public UnityAction<T> OnValueChanged { get => onValueChanged; set => onValueChanged = value; }
    }
}
