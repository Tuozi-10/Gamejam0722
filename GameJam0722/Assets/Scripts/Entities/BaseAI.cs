using Managers;
using Terrain;

namespace Entities
{
    public class BaseAI : AbstractEntity
    {
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
    }
}