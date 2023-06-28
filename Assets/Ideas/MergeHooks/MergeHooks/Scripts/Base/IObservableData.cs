using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MergeHooks
{
    public interface IObservableData<T>
    {
        T Value { get; set; }
        UnityAction<T> OnValueChanged { get; set; }
    }
}
