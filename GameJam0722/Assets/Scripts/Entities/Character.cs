namespace Entities
{
    public class Character : AbstractEntity
    {
        public static Character instance;

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

        
    }
}