﻿using System;
using System.Collections.Generic;
using System.Linq;
using SystemBase;
using SystemBase.StateMachineBase;
using Systems.GameState.Messages;
using Systems.GameState.States;
using Systems.Interface.Actions;
using UniRx;
using UnityEngine.SceneManagement;
using Utils;

namespace Systems
{
    public class Game : GameBase
    {
        public readonly StateContext<Game> GameStateContext = new StateContext<Game>();

        public void StartGame()
        {
            MessageBroker.Default.Publish(new GameMsgStart());
        }

        public void Restart()
        {
            MessageBroker.Default.Publish(new GameMsgRestart());
        }

        private static IEnumerable<Type> CollectAllSystems()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes(), (ass, type) => new { ass, type })
                .Where(assemblyType => Attribute.IsDefined(assemblyType.type, typeof(GameSystemAttribute)))
                .Select(assemblyType => assemblyType.type);
        }

        private void Awake()
        {
            IoC.RegisterSingleton(this);

            GameStateContext.Start(new Loading());

            foreach (var systemType in CollectAllSystems())
            {
                RegisterSystem(Activator.CreateInstance(systemType) as IGameSystem);
            }

            Init();

            MessageBroker.Default.Publish(new GameMsgFinishedLoading());
        }
    }
}