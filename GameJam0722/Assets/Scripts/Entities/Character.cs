using Managers;

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
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        public override void StartTurn()
        {
            base.StartTurn();
            UIManager.instance.AskTurn("Player Turn");
            canPlay = true;
        }

    }
}