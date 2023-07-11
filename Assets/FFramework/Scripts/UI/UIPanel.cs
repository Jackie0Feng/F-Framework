﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class UIPanel : MonoBehaviour
    {
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
