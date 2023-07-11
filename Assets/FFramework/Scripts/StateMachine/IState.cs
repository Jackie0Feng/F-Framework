using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public interface IState
    {
        public void Enter();

        // Update is called once per frame
        public void Update();

        public void Exit();
    }


    public abstract class State : IState
    {
        public abstract void Enter();

        public abstract void Update();

        public abstract void Exit();
    }
}
