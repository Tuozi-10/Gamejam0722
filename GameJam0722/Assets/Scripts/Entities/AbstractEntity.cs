using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
      public abstract class AbstractEntity : MonoBehaviour
      {
            private int m_hp;
            public int hp => m_hp;

            public void Hit(int hitValue)
            {
                  m_hp -= hitValue;
                  if (m_hp <= 0)
                  {
                        m_hp = 0;
                        Die();
                  }
            }

            protected virtual void Die()
            {
                  
            }

            public virtual void StartTurn()
            {
                  
            }

            public virtual void SetPath(List<(int x, int y)> path)
            {
                  
            }
      }
}