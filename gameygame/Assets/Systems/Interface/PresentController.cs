using System.Collections.Generic;
using Systems.Collectables.Events;
using Systems.GameState.States;
using UniRx;
using UnityEngine;
using Utils;

namespace Systems.Interface
{
    public class PresentController : MonoBehaviour
    {
        public List<GameObject> Presents;
        private readonly Dictionary<string, GameObject> _presentDict = 
            new Dictionary<string, GameObject>();

        private void Start()
        {
            foreach (var present in Presents)
            {
                _presentDict.Add(present.name, present);
            }

            SetAllInactive();

            MessageBroker.Default.Receive<PresentEvtCollected>()
                .Subscribe(collected => _presentDict[collected.Present.Id].SetActive(true))
                .AddTo(this);

            IoC.Game.GameStateContext.CurrentState.Where(state => state.GetType() == typeof(StartScreen))
                .Subscribe(state => SetAllInactive())
                .AddTo(this);
        }

        private void SetAllInactive()
        {
            foreach (var present in Presents)
            {
                present.SetActive(false);
            }
        }
    }
}
