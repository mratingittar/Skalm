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

namespace Skalm
{
    internal class SceneManager
    {
        public string playerName = "";

        private MapManager _mapManager;
        private DisplayManager _displayManager;
        private EnemySpawner _enemySpawner;
        private ItemSpawner _itemSpawner;
        private List<Item> _items;

        public List<GameObject> GameObjectsInScene { get; }
        public List<Actor> ActorsInScene { get; }
        public Player Player { get; }


        // MOVE TO SETTINGS.TXT
        private const char _playerChar = '©';
        private const char _enemyChar = 'ⱺ';
        private const char _keyChar = 'Ⱡ';
        private const char _potionChar = '♥';
        private const char _itemChar = '☼';
        private const ConsoleColor _playerColor = ConsoleColor.Blue;
        private const ConsoleColor _enemyColor = ConsoleColor.DarkRed;
        private const ConsoleColor _itemColor = ConsoleColor.Yellow;
        private const ConsoleColor _potionColor = ConsoleColor.Red;
        private const ConsoleColor _keyColor = ConsoleColor.White;

        // CONSTRUCTOR I
        public SceneManager(MapManager mapManager, DisplayManager displayManager)
        {
            _mapManager = mapManager;
            _displayManager = displayManager;
            Player = new Player(mapManager, displayManager, new PlayerAttackComponent(), new ActorStatsObject(new StatsObject(5, 5, 5, 5, 5, 10, 1, 0), "Nameless"), "Nameless", Vector2Int.Zero);
            ActorsInScene = new List<Actor>();
            GameObjectsInScene = new List<GameObject>();
            _items = new List<Item>();
            _enemySpawner = new EnemySpawner(mapManager, this);
            _itemSpawner = new ItemSpawner();

            ItemPickup.onItemPickup += RemoveGameObject;
        }

        public void LevelComplete()
        {
            _displayManager.Eraser.EraseAll();
            ResetScene();

            _mapManager.mapGenerator.CreateMap();
            ResetPlayer();
            InitializeScene(); // SCALE ENEMIES OVER TIME
            Player.NextFloor();

            _displayManager.DisplayHUD();
            _mapManager.mapPrinter.DrawMap();

            Player.SendStatsToDisplay();
        }

        public void ResetPlayer()
        {
            Vector2Int spawnPos = _mapManager.mapGenerator.PlayerFixedSpawnPosition.Equals(Vector2Int.Zero)
                ? _mapManager.GetRandomPosition() : _mapManager.mapGenerator.PlayerFixedSpawnPosition;

            Player.SetPlayerPosition(spawnPos);
            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);
        }

        public void InitializePlayer()
        {
            Vector2Int spawnPos = _mapManager.mapGenerator.PlayerFixedSpawnPosition.Equals(Vector2Int.Zero)
                ? _mapManager.GetRandomPosition() : _mapManager.mapGenerator.PlayerFixedSpawnPosition;
            if (playerName.Length == 0)
                playerName = "Nameless";

            Player.InitializePlayer(spawnPos, playerName, _playerChar, _playerColor);
            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);
        }

        // INITIALIZE SCENE
        public void InitializeScene()
        {
            // ADD ENEMIES
            if (_mapManager.mapGenerator.EnemySpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.mapGenerator.EnemySpawnPositions)
                {
                    Enemy enemy = _enemySpawner.Spawn(position, _enemyChar, _enemyColor);
                    GameObjectsInScene.Add(enemy);
                    ActorsInScene.Add(enemy);
                }
            }
            else
            {
                Enemy enemy = _enemySpawner.Spawn(_mapManager.GetRandomPosition(), _enemyChar, _enemyColor);
                GameObjectsInScene.Add(enemy);
                ActorsInScene.Add(enemy);
            }

            // ADD ITEMS
            if (_mapManager.mapGenerator.ItemSpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.mapGenerator.ItemSpawnPositions)
                {
                    GameObjectsInScene.Add(_itemSpawner.Spawn(position, _itemChar, _itemColor, ItemGen.GetRandomEquippable()));
                }
            }
            else
            {
                int items = Dice.Roll(3);
                for (int i = 0; i < items; i++)
                {
                    GameObjectsInScene.Add(_itemSpawner.Spawn(_mapManager.GetRandomPosition(), _itemChar, _itemColor, ItemGen.GetRandomEquippable()));
                }
            }

            //// CREATING TEST ITEM
            //var FloorTiles = _mapManager.mapGenerator.FloorTiles;
            //var itemXY = FloorTiles.ElementAt(Dice.rng.Next(0, FloorTiles.Count));
            //ItemEquippable item1 = new ItemEquippable("Helmet of misfortune", (int)EEqSlots.Head, new StatsObject(0, 0, 2, 0, 1, 5, 0, 2));
            //GameObjectsInScene.Add(_itemSpawner.Spawn(itemXY, 'o', ConsoleColor.Yellow, item1));

            // ADD POTIONS
            int potions = Dice.Roll(2);
            for (int i = 0; i < potions; i++)
            {
                GameObjectsInScene.Add(_itemSpawner.Spawn(_mapManager.GetRandomPosition(), _potionChar, _potionColor, ItemGen.GetRandomPotion()));
            }


            // ADD KEYS
            if (_mapManager.mapGenerator.KeySpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.mapGenerator.KeySpawnPositions)
                {
                    GameObjectsInScene.Add(_itemSpawner.Spawn(position, _keyChar, _keyColor, new Key()));
                }
            }
            else
            {
                foreach(var door in _mapManager.mapGenerator.Doors)
                {
                    GameObjectsInScene.Add(_itemSpawner.Spawn(_mapManager.GetRandomPosition(), _keyChar, _keyColor, new Key()));
                }
            } 
                

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
            int actors = ActorsInScene.Count;
            for (int i = 0; i < actors; i++)
            {
                if (_mapManager.TileGrid.TryGetGridObject(ActorsInScene[i].GridPosition, out BaseTile tile) && tile is IOccupiable tileOcc)
                    tileOcc.ActorPresent = false;
                if (ActorsInScene[i] is Enemy enemy)
                    enemy.Remove();
            }

            ActorsInScene.Clear();
            GameObjectsInScene.Clear();
            _displayManager.ClearMessageQueue();
            _displayManager.ClearMessageSection();
            _mapManager.mapGenerator.ResetMap();
        }

        // ENEMY FACTORY

        // ITEM FACTORY

        // REMOVE OBJECT FROM GAME VIEW
        public void RemoveGameObject(GameObject obj)
        {
            // REMOVE FROM OBJECT LIST
            GameObjectsInScene.Remove(obj);

            // REMOVE FROM ACTOR LIST IF ACTOR
            if (obj is Actor actor)
            {
                ActorsInScene.Remove(actor);
                if (actor is Enemy enemy)
                    enemy.Remove();
            }

            // REMOVE FROM TILE OBJECT LIST
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
                occ.ObjectsOnTile.TryPop(out GameObject? _);

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
