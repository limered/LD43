using System;
using UniRx;

namespace SystemBase.StateMachineBase
{
    public interface IStateContext<TState, T> where TState : IState<T>
    {
        ReactiveProperty<TState> CurrentState { get; }
        ReactiveCommand<Tuple<TState, TState>> BevoreStateChange { get; }
        ReactiveCommand<Tuple<TState, TState>> AfterStateChange { get; }

        void Start(TState initialState);

        bool GoToState(TState state);
    }
}