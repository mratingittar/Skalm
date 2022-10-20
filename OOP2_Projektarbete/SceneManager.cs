using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;
using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Map;
using Skalm.Map.Tile;
using Skalm.Structs;
using Skalm.Utilities;
using System.Xml.Linq;

namespace Skalm
{
    internal class SceneManager
    {
        public string playerName = "";

        private MapManager _mapManager;
        private DisplayManager _displayManager;
        private EnemySpawner _enemySpawner;
        private ItemSpawner _itemSpawner;

        public List<GameObject> GameObjectsInScene { get; }
        public List<Actor> ActorsInScene { get; }
        public Player Player { get; }

        // CONSTRUCTOR I
        public SceneManager(MapManager mapManager, DisplayManager displayManager)
        {
            _mapManager = mapManager;
            _displayManager = displayManager;
            Player = new Player(mapManager, displayManager, new PlayerAttackComponent(), new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), "Nameless"), "Nameless", Vector2Int.Zero);
            ActorsInScene = new List<Actor>();
            GameObjectsInScene = new List<GameObject>();

            _enemySpawner = new EnemySpawner(mapManager, this);
            _itemSpawner = new ItemSpawner();
        }

        // INITIALIZE SCENE
        public void InitializeScene()
        {
            Vector2Int spawnPos = _mapManager.mapGenerator.PlayerFixedSpawnPosition.Equals(Vector2Int.Zero) 
                ?  _mapManager.GetRandomSpawnPosition() : _mapManager.mapGenerator.PlayerFixedSpawnPosition;
            if (playerName.Length == 0)
                playerName = "Nameless";

            Player.InitializePlayer(spawnPos, playerName, 'P', ConsoleColor.Blue);

            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);

            // ADD ENEMIES
            if (_mapManager.mapGenerator.EnemySpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.mapGenerator.EnemySpawnPositions)
                {
                    Enemy enemy = _enemySpawner.Spawn(position, 'E', ConsoleColor.Red);
                    GameObjectsInScene.Add(enemy);
                    ActorsInScene.Add(enemy);
                }
            }
            else
            {
                Enemy enemy = _enemySpawner.Spawn(_mapManager.GetRandomSpawnPosition(), 'E', ConsoleColor.Yellow);
                GameObjectsInScene.Add(enemy);
                ActorsInScene.Add(enemy);
            }

            // ADD ITEMS
            var FloorTiles = _mapManager.mapGenerator.FloorTiles;
            var itemXY = FloorTiles.ElementAt(Dice.rng.Next(0, FloorTiles.Count));

            // CREATING TEST ITEM
            ItemEquippable item1 = new ItemEquippable("Helmet of misfortune", (int)EEqSlots.Head, new StatsObject(0, 0, 2, 0, 1, 5, 0, 2));

            GameObjectsInScene.Add(_itemSpawner.Spawn(itemXY, 'o', ConsoleColor.Yellow, item1));

            ItemPickup.onItemPickup += RemoveGameObject;

            // ADD OBJECTS TO MAP
            AddObjectsToMap();
        }

        // ADD OBJECTS TO MAP
        private void AddObjectsToMap()
        {
            foreach (GameObject go in GameObjectsInScene)
            {
                if (_mapManager.TileGrid.TryGetGridObject(go.GridPosition, out BaseTile tile) && tile is IOccupiable tileOcc)
                    tileOcc.ObjectsOnTile.Push(go);
            }

            foreach (Actor actor in ActorsInScene)
            {
                if (_mapManager.TileGrid.TryGetGridObject(actor.GridPosition, out BaseTile tile) && tile is IOccupiable tileOcc)
                    tileOcc.ActorPresent = true;
            }
        }

        // RESET OBJECTS IN SCENE
        public void ResetScene()
        {
            // CLEAR GAME OBJECTS LISTS
            foreach (var go in GameObjectsInScene)
            {
                if (_mapManager.TileGrid.TryGetGridObject(go.GridPosition, out BaseTile tile) && tile is IOccupiable tileOcc)
                    tileOcc.ObjectsOnTile.Clear();
            }

            // CLEAR ACTORS LIST
            foreach (Actor actor in ActorsInScene)
            {
                if (_mapManager.TileGrid.TryGetGridObject(actor.GridPosition, out BaseTile tile) && tile is IOccupiable tileOcc)
                    tileOcc.ActorPresent = false;
            }

            ActorsInScene.Clear();
            GameObjectsInScene.Clear();
            _displayManager.ClearMessageQueue();
            _displayManager.ClearMessageSection();
            _mapManager.mapGenerator.ClearTileGrid();
        }

        // ENEMY FACTORY

        // ITEM FACTORY

        // REMOVE OBJECT FROM GAME VIEW
        public void RemoveGameObject(GameObject obj)
        {
            // REMOVE OBJECT FROM OBJECT LIST
            GameObjectsInScene.Remove(obj);

            // REMOVE OBJECT FROM TILE OBJECT LIST
            _mapManager.TileGrid.TryGetGridObject(obj.GridPosition, out BaseTile tile);
            if (tile is IOccupiable occ)
            {
                if (obj is Actor)
                    occ.ActorPresent = false;

                Stack<GameObject> objects = new Stack<GameObject>();

                while (occ.ObjectsOnTile.Count > 0 && occ.ObjectsOnTile.Peek() != obj)
                {
                    objects.Push(occ.ObjectsOnTile.Pop());
                }
                occ.ObjectsOnTile.Pop();

                while (objects.Count > 0)
                {
                    occ.ObjectsOnTile.Push(objects.Pop());
                }
            }

            // CACHE TILE POSITION FOR REDRAW
            _mapManager.mapPrinter.CacheUpdatedTile(obj.GridPosition);
        }
    }
}
