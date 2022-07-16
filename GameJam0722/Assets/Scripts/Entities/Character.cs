using System.Collections.Generic;
using Managers;
using Terrain;

namespace Entities
{
    public class Character : AbstractEntity
    {
        public static Character instance;

        public bool canPlay;
        
        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        public override void SetPath(List<Pathfinder.Node> path)
        {
            base.SetPath(path);
            canPlay = false;
        }

        public override void StartTurn()
        {
            base.StartTurn();
            UIManager.instance.AskTurn("Player Turn");
            canPlay = true;
        }

    }
}