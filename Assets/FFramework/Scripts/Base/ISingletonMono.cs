using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public interface ISingletonMono<T> where T : MonoBehaviour
    {
        //static Ty instance;

        public static T Instance;

    }
}
