using System.Collections.Generic;
using SystemBase;
using Systems.GameState.States;
using Systems.Interface.Actions;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.Interface
{
    [GameSystem]
    public class ScreenSystem : GameSystem<FullScreenComponent>
    {
        private readonly Dictionary<string, FullScreenComponent> _screens = 
            new Dictionary<string, FullScreenComponent>();

        public override void Init()
        {
            base.Init();
            IoC.Game.GameStateContext.CurrentState
                .Where(state => state.GetType() == typeof(Running))
                .Subscribe(state =>
                {
                    MessageBroker.Default.Publish(new InterfaceActHideScreen { Name = "StartScreen" });
                    MessageBroker.Default.Publish(new InterfaceActShowScreen { Name = "GUI" });
                });

            IoC.Game.GameStateContext.CurrentState
                .Where(state => state.GetType() == typeof(GameOver))
                .Subscribe(state =>
                {
                    MessageBroker.Default.Publish(new InterfaceActHideScreen { Name = "GUI" });
                    MessageBroker.Default.Publish(new InterfaceActShowScreen { Name = "GameOver" });
                });

            IoC.Game.GameStateContext.CurrentState
                .Where(state => state.GetType() == typeof(Loading) || 
                                state.GetType() == typeof(StartScreen))
                .Subscribe(state =>
                {
                    MessageBroker.Default.Publish(new InterfaceActHideScreen { Name = "GUI" });
                    MessageBroker.Default.Publish(new InterfaceActHideScreen { Name = "GameOver" });
                    MessageBroker.Default.Publish(new InterfaceActShowScreen { Name = "StartScreen" });
                });
        }

        public override void Register(FullScreenComponent component)
        {
            _screens.Add(component.Name, component);

            MessageBroker.Default.Receive<InterfaceActShowScreen>()
                .Subscribe(screen => _screens[screen.Name].CanvasToHide.enabled = true)
                .AddTo(component);

            MessageBroker.Default.Receive<InterfaceActHideScreen>()
                .Subscribe(screen => _screens[screen.Name].CanvasToHide.enabled = false)
                .AddTo(component);
        }
    }
}
