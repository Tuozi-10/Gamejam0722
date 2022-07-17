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
            var ground = TerrainManager.instance.GetAvailableArray();
            SetPath(Pathfinder.GetPath(pos.x, pos.y, Character.instance.pos.x, Character.instance.pos.y, ground));
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
        }
    }
}