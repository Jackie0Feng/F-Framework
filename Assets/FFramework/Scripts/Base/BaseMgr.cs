using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class BaseMgr<T> : MonoBehaviour
    {
        [SerializeField]
        Dictionary<string, T> items = new Dictionary<string, T>();

        public Dictionary<string, T> Items { get => items; set => items = value; }

    }
}
