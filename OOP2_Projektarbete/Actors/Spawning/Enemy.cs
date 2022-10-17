using Skalm.Actors.Tile;
using Skalm.Map;
using Skalm.Structs;
using Skalm.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skalm.Actors.Spawning
{
    internal class Enemy : Actor
    {
        GameManager gameManager;
        private Queue<BaseTile> path;
        public Enemy(GameManager gameManager, Vector2Int gridPosition, char sprite, ConsoleColor color) : base(gridPosition, sprite, color)
        {
            this.gameManager = gameManager;
            path = new Queue<BaseTile>();
            Player.playerTurn += MoveEnemy;
            FindPathToPlayer();
        }

        public void MoveEnemy()
        {
            if (path.Count > 0)
            {
                Vector2Int dir = Vector2Int.DirectionFromTo(GridPosition, path.Dequeue().GridPosition);
                base.Move(dir);
            }
            FindPathToPlayer();
        }

        private void FindPathToPlayer()
        {
            gameManager.MapManager.TileGrid.TryGetGridObject(GridPosition, out BaseTile startTile);
            gameManager.MapManager.TileGrid.TryGetGridObject(gameManager.player.GridPosition, out BaseTile targetTile);
            path = gameManager.MapManager.pathfinder.FindPath(startTile, targetTile);
        }

        private Vector2Int RandomDirection(Direction dir) => dir switch
        {
            Direction.Up => new Vector2Int(0, -1),
            Direction.Right => new Vector2Int(1, 0),
            Direction.Down => new Vector2Int(0, 1),
            Direction.Left => new Vector2Int(-1, 0),
            _ => throw new InvalidOperationException("Not a direction"),
        };
	
		
	}

}

