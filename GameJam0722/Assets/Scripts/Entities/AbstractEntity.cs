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
            public Vector2Int startPos;

            [SerializeField] float durationMove = 0.75f;
            [SerializeField] public Transform Pivot;
            [SerializeField] private float heightWalk = 0.5f;
            [SerializeField] private float intensityImpactWalk = 0.175f;
            [SerializeField] private int radiusImpactWalk = 2;
            
            
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

            public virtual void EndTurn() {
                  LevelManager.instance.EndTurn();
            }
            
            private IEnumerator GoThroughPath(List<Node> path)
            {
                  // Remove departure cell
                  path.RemoveAt(0);
                  int pmUsed = 0;
                  
                  while (path.Count > 0 && pmUsed < pm)
                  {
                        pmUsed++;
                        transform.DOMove(GetPosFromCoord(path[0].posX, path[0].posY), durationMove).SetEase(Ease.InOutQuad);
                        
                        Pivot.DOLocalMoveY(heightWalk, durationMove / 2f)
                              .OnComplete(()=>Pivot.DOLocalMoveY(0, durationMove / 2f));
                        var toLook = GetAngle(pos.x, pos.y, path[0].posX, path[0].posY);
              
                        transform.DORotate(new Vector3(0,toLook,0), 0.25f);
                        pos = new Vector2Int(path[0].posX, path[0].posY);
                        path.RemoveAt(0);
                        
                        yield return new WaitForSeconds(durationMove/2f);
                        TerrainManager.instance.SetImpactAt(pos.x, pos.y, radiusImpactWalk, intensityImpactWalk);
                        yield return new WaitForSeconds(durationMove/2f);
                  }

                  EndTurn();
            }

            private float GetAngle(int posX, int posY, int toX, int toY)
            {
                  if (posX > toX) return 90; // left
                  if (posX < toX) return -90; // right
                  if (posY > toY) return 0; // up
                  return 180;
            }
            
            public static Vector3 GetPosFromCoord(int x, int y)
            {
                  var diceTransform = TerrainManager.instance.diceTerrainlsit[x, y].transform;
                   return new Vector3(diceTransform.position.x, diceTransform.position.y + 1, diceTransform.position.z);
            }
            
      }
}