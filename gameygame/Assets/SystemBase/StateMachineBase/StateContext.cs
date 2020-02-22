using System;
using UniRx;

namespace SystemBase.StateMachineBase
{
    public class StateContext<T> : IStateContext<BaseState<T>, T>
    {
        private readonly ReactiveCommand<Tuple<BaseState<T>, BaseState<T>>> _afterStateChange;
        private readonly ReactiveCommand<Tuple<BaseState<T>, BaseState<T>>> _bevoreStateChange;

        public StateContext()
        {
            _bevoreStateChange = new ReactiveCommand<Tuple<BaseState<T>, BaseState<T>>>();
            _afterStateChange = new ReactiveCommand<Tuple<BaseState<T>, BaseState<T>>>();
        }

        public ReactiveCommand<Tuple<BaseState<T>, BaseState<T>>> AfterStateChange
        {
            get { return _afterStateChange; }
        }

        public ReactiveCommand<Tuple<BaseState<T>, BaseState<T>>> BevoreStateChange
        {
            get { return _bevoreStateChange; }
        }

        public ReactiveProperty<BaseState<T>> CurrentState { get; private set; }

        public bool GoToState(BaseState<T> state)
        {
            if (!CurrentState.Value.ValidNextStates.Contains(state.GetType()) ||
                !CurrentState.Value.Exit())
            {
                return false;
            }

            _bevoreStateChange.Execute(new Tuple<BaseState<T>, BaseState<T>>(CurrentState.Value, state));

            var lastState = CurrentState.Value;
            CurrentState.Value = state;
            CurrentState.Value.Enter(this);

            _afterStateChange.Execute(new Tuple<BaseState<T>, BaseState<T>>(lastState, CurrentState.Value));

            return true;
        }

        public void Start(BaseState<T> initialState)
        {
            CurrentState = new ReactiveProperty<BaseState<T>>(initialState);
            CurrentState.Value.Enter(this);
        }
    }
}