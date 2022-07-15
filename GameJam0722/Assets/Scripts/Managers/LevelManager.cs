using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager instance;
    
        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(this);
                return;
            }

            instance = this;
        }
        
    }
}
