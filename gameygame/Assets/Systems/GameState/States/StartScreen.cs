using SystemBase.StateMachineBase;
using Systems.GameState.Messages;
using UniRx;

namespace Systems.GameState.States
{
    [NextValidStates(typeof(Running))]
    public class StartScreen : BaseState<Game>
    {
        public override void Enter(StateContext<Game> context)
        {
            MessageBroker.Default.Receive<GameMsgStart>()
                .Subscribe(start => context.GoToState(new Running()))
                .AddTo(this);
        }
    }
}