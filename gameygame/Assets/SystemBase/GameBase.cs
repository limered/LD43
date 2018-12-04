using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace SystemBase
{
    public class GameBase : MonoBehaviour, IGameSystem
    {
        public StringReactiveProperty DebugMainFrameCallback = new StringReactiveProperty();
        private readonly Dictionary<Type, IGameSystem> _gameSystemDict = new Dictionary<Type, IGameSystem>();
        private readonly Dictionary<Type, List<IGameSystem>> _systemToComponentMapper = new Dictionary<Type, List<IGameSystem>>();
        private readonly Dictionary<Type, int> _inDegrees = new Dictionary<Type, int>();
        private List<IGameSystem> _gameSystems = new List<IGameSystem>();
        public Type[] ComponentsToRegister { get { return new Type[0]; } }

        public virtual void Init()
        {
            MapAllSystemsComponents();

            DebugMainFrameCallback.ObserveOnMainThread().Subscribe(OnDebugCallbackCalled);

            DontDestroyOnLoad(this);
        }

        public void RegisterComponent(GameComponent component)
        {
            List<IGameSystem> systemsToRegisterTo;
            if (!_systemToComponentMapper.TryGetValue(component.GetType(), out systemsToRegisterTo)) return;

            foreach (var system in systemsToRegisterTo)
            {
                system.RegisterComponent(component);
            }
        }

        public T System<T>() where T : IGameSystem
        {
            IGameSystem system;
            if (_gameSystemDict.TryGetValue(typeof(T), out system))
                return (T)system;
            throw new ArgumentException("System: " + typeof(T) + " not registered!");
        }

        protected virtual void OnDebugCallbackCalled(string s)
        {
            print(s);
        }

        protected void RegisterSystem<T>(T system) where T : IGameSystem
        {
            _gameSystems.Add(system);
            _gameSystemDict.Add(system.GetType(), system);
            _inDegrees.Add(system.GetType(), 0);
        }

        private void MapAllSystemsComponents()
        {
            _gameSystems = SortSystems();

            PrintDependencyList(_gameSystems);

            foreach (var system in _gameSystems)
            {
                foreach (var componentType in system.ComponentsToRegister)
                {
                    MapSystemToComponent(system, componentType);
                }

                system.Init();
            }
        }

        private GameSystemAttribute GetAttribute(Type t)
        {
            return (GameSystemAttribute)Attribute.GetCustomAttribute(t, typeof(GameSystemAttribute));
        }

        private List<IGameSystem> SortSystems()
        {
            foreach (var system in _gameSystems)
            {
                foreach (var dependency in GetAttribute(system.GetType()).Dependencies)
                {
                    _inDegrees[dependency]++;
                }
            }

            var result = new List<IGameSystem>();
            var q = new Queue<IGameSystem>(_gameSystems
                .Where(system => _inDegrees[system.GetType()] == 0));

            while (q.Any())
            {
                var system = q.Dequeue();
                result.Add(system);
                foreach (var dependency in GetAttribute(system.GetType()).Dependencies)
                {
                    _inDegrees[dependency]--;
                    if (_inDegrees[dependency] == 0)
                    {
                        q.Enqueue(_gameSystemDict[dependency]);
                    }
                }
            }
            result.Reverse();
            if (_gameSystems.Count == result.Count) return result;
            var circ = _gameSystems.First(s => !result.Contains(s));
            throw new ArgumentException("Circular dependency in GameSystem registration! System: " + circ.GetType());
        }

        private void PrintDependencyList(IEnumerable<IGameSystem> systems)
        {
            Debug.Log("System List:\n" + systems.Aggregate("", (current, system) => current + (system.GetType() + "\n")));
        }

        private void MapSystemToComponent(IGameSystem system, Type componentType)
        {
            if (!_systemToComponentMapper.ContainsKey(componentType))
            {
                _systemToComponentMapper.Add(componentType, new List<IGameSystem>());
            }
            _systemToComponentMapper[componentType].Add(system);
        }
    }
}