using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Managers;
using UnityEngine;
using static Terrain.Pathfinder;

namespace Entities
{
      public abstract class AbstractEntity : MonoBehaviour
      {
            private int m_hp;
            public int hp => m_hp;

            public Vector2Int pos;

            [SerializeField] float durationMove = 0.75f;

            public int pm = 1;
            
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

            public virtual void SetPath(List<Node> path)
            {
                 StartCoroutine(GoThroughPath(path));
            }
            
            private IEnumerator GoThroughPath(List<Node> path)
            {
                  // Remove departure cell
                  path.RemoveAt(0);
                  int pmUsed = 0;
                  
                  while (path.Count > 0 && pmUsed >= pm)
                  {
                        pmUsed++;
                        transform.DOMove(GetPosFromCoord(path[0].posX, path[0].posY), durationMove);
                        pos = new Vector2Int(path[0].posX, path[0].posY);
                        path.RemoveAt(0);
                        yield return new WaitForSeconds(durationMove+0.05f);
                  }
                  LevelManager.instance.EndTurn();
            }

            public static Vector3 GetPosFromCoord(int x, int y)
            {
                  var diceTransform = TerrainManager.instance.diceTerrainlsit[x, y].transform;
                   return new Vector3(diceTransform.position.x, diceTransform.position.y + MouseManager.instance.cursorHeight, diceTransform.position.z);
            }
            
      }
}