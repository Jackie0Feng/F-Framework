using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class StateMachine : MonoBehaviour
    {
        public IState CurrentState { get; private set; }

        Dictionary<string, IState> states = new Dictionary<string, IState>();

        public Dictionary<string, IState> States { get => states; set => states = value; }



        public void Initialize(IState starting_state)
        {
            CurrentState = starting_state;
            starting_state.Enter();
        }

        public void TransitionTo(IState next_state)
        {
            CurrentState.Exit();
            CurrentState = next_state;
            CurrentState.Enter();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }
    }
}
