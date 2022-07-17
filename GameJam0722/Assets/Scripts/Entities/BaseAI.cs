using System.Collections;
using DG.Tweening;
using Managers;
using Terrain;
using UnityEngine;

namespace Entities
{
    public class BaseAI : AbstractEntity {
        public override void StartTurn()
        {
            base.StartTurn();
            UIManager.instance.AskTurn("Enemy Turn");

            GeneratePath();
        }

        private void GeneratePath()
        {
            (int x, int y) posTo = (Character.instance.pos.x, Character.instance.pos.y);

            foreach (var leurre in Leurre.leurres)
            {
                var distance = Vector2Int.Distance(pos, new Vector2Int(leurre.pos.pox, leurre.pos.poxy));
                if(distance <= leurre.radiusDetection)
                {
                    posTo = leurre.pos;
                }
            }
            
            var ground = TerrainManager.instance.GetAvailableArray(TerrainManager.instance.diceTerrainlsit[pos.x, pos.y].diceData.isWall);
            SetPath(Pathfinder.GetPath(pos.x, pos.y, posTo.x, posTo.y, ground));
        }

        /// <summary>
        /// End the turn of the Enemy
        /// </summary>
        public override void EndTurn() {
            if (Character.instance.pos == pos) {
                Character.instance.transform.parent = this.transform;

                StartCoroutine(throwPlayer());
                return;
            }
            
            base.EndTurn();
        }

        /// <summary>
        /// Throw the player in the water
        /// </summary>
        /// <returns></returns>
        private IEnumerator throwPlayer() {
            yield return new WaitForSeconds(2);
            transform.DOLocalMove(-transform.forward * 15, 1.5f);
            LevelManager.instance.ReloadLevel();
            LevelManager.instance.Defeat();
        }
    }
}