using Systems;
using UnityEngine;
using Utils;

namespace SystemBase
{
    public class GameComponent : MonoBehaviour, IGameComponent
    {
        public virtual IGameSystem System { get; set; }

        public void RegisterToGame()
        {
            IoC.Resolve<Game>().RegisterComponent(this);
        }

        protected void Start()
        {
            RegisterToGame();
        }
    }

    public class SemanticGameComponent<TGameComponent> : GameComponent where TGameComponent : IGameComponent
    {
        public TGameComponent dependency;
        public TGameComponent Dependency
        {
            get
            {
                if (dependency != null) return dependency;
                else return GetComponent<TGameComponent>();
            }
            set
            {
                dependency = value;
            }
        }
    }
}
