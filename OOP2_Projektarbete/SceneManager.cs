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

        public List<GameObject> GameObjectsInScene { get; }
        public List<Actor> ActorsInScene { get; }
        public Player Player { get; }
        public string PlayerName { get; set; } = "";

        private MapManager _mapManager;
        private DisplayManager _displayManager;
        private EnemySpawner _enemySpawner;
        private ItemSpawner _itemSpawner;
        private PotionSpawner _potionSpawner;
        private KeySpawner _keySpawner;
        private ISettings _settings;

        // CONSTRUCTOR I
        public SceneManager(ISettings settings, MapManager mapManager, DisplayManager displayManager, Player player, EnemySpawner enemySpawner, ItemSpawner itemSpawner, PotionSpawner potionSpawner, KeySpawner keySpawner)
        {
            ActorsInScene = new List<Actor>();
            GameObjectsInScene = new List<GameObject>();

            _settings = settings;
            _mapManager = mapManager;
            _displayManager = displayManager;
            Player = player;
            _enemySpawner = enemySpawner;
            _itemSpawner = itemSpawner;
            _potionSpawner = potionSpawner;
            _keySpawner = keySpawner;

            ItemPickup.onItemPickup += RemoveGameObject;
            Enemy.OnEnemyDeath += RemoveGameObject;
        }

        public void NewGame()
        {
            _potionSpawner.ScalingMultiplier = 0;
            _enemySpawner.ScalingMultiplier = 0;
            _itemSpawner.ScalingMultiplier = 0;

            InitializePlayer();
            InitializeScene();
        }

        public void InitializePlayer()
        {
            Vector2Int spawnPos = _mapManager.MapGenerator.PlayerFixedSpawnPosition.Equals(Vector2Int.Zero)
                ? _mapManager.GetRandomFloorPosition() : _mapManager.MapGenerator.PlayerFixedSpawnPosition;
            if (PlayerName.Length == 0)
                PlayerName = "Nameless";

            Player.InitializePlayer(spawnPos, PlayerName, _settings.PlayerSprite, _settings.PlayerColor);
            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);
        }

        public void NextLevel()
        {
            _displayManager.Eraser.EraseAll();
            ResetScene();

            _mapManager.MapGenerator.CreateMap();
            ResetPlayer();
            IncrementScaling();
            InitializeScene();
            Player.NextFloor();

            _displayManager.DisplayHUD();
            _mapManager.MapPrinter.DrawMap();

            Player.UpdateAllDisplays();
        }

        private void IncrementScaling()
        {
            _itemSpawner.ScalingMultiplier++;
            _enemySpawner.ScalingMultiplier++;
            _potionSpawner.ScalingMultiplier++;
        }

        public void ResetPlayer()
        {
            Vector2Int spawnPos = _mapManager.MapGenerator.PlayerFixedSpawnPosition.Equals(Vector2Int.Zero)
                ? _mapManager.GetRandomFloorPosition() : _mapManager.MapGenerator.PlayerFixedSpawnPosition;

            Player.SetPlayerPosition(spawnPos);
            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);
        }


        // INITIALIZE SCENE
        public void InitializeScene()
        {
            // ADD ENEMIES
            if (_mapManager.MapGenerator.EnemySpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.MapGenerator.EnemySpawnPositions)
                {
                    Enemy enemy = _enemySpawner.Spawn(position, _settings.EnemySprite, _settings.EnemyColor);
                    GameObjectsInScene.Add(enemy);
                    ActorsInScene.Add(enemy);
                }
            }
            else
            {
                Enemy enemy = _enemySpawner.Spawn(_mapManager.GetRandomFloorPosition(), _settings.EnemySprite, _settings.EnemyColor);
                GameObjectsInScene.Add(enemy);
                ActorsInScene.Add(enemy);
            }

            // ADD ITEMS
            if (_mapManager.MapGenerator.ItemSpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.MapGenerator.ItemSpawnPositions)
                {
                    GameObjectsInScene.Add(_itemSpawner.Spawn(position, _settings.ItemSprite, _settings.ItemColor));
                }
            }
            else
            {
                int items = Dice.Roll(3);
                for (int i = 0; i < items; i++)
                {
                    GameObjectsInScene.Add(_itemSpawner.Spawn(_mapManager.GetRandomFloorPosition(), _settings.ItemSprite, _settings.ItemColor));
                }
            }

            // ADD POTIONS
            if (_mapManager.MapGenerator.PotionSpawnPositions.Count() > 0)
            {
                foreach (Vector2Int position in _mapManager.MapGenerator.PotionSpawnPositions)
                {
                    GameObjectsInScene.Add(_potionSpawner.Spawn(position, _settings.PotionSprite, _settings.PotionColor));
                }
            }
            else
            {
                int potions = Dice.Roll(2);
                for (int i = 0; i < potions; i++)
                {
                    GameObjectsInScene.Add(_potionSpawner.Spawn(_mapManager.GetRandomFloorPosition(), _settings.PotionSprite, _settings.PotionColor));
                }
            }


            // ADD KEYS
            if (_mapManager.MapGenerator.KeySpawnPositions.Count > 0)
            {
                foreach (Vector2Int position in _mapManager.MapGenerator.KeySpawnPositions)
                {
                    GameObjectsInScene.Add(_keySpawner.Spawn(position, _settings.KeySprite, _settings.KeyColor));
                }
            }
            else
            {
                foreach (var door in _mapManager.MapGenerator.Doors)
                {
                    GameObjectsInScene.Add(_keySpawner.Spawn(_mapManager.GetRandomFloorPosition(), _settings.KeySprite, _settings.KeyColor));
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
            _mapManager.MapGenerator.ResetMap();
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
            _mapManager.MapPrinter.CacheUpdatedTile(obj.GridPosition);
        }
    }
}
