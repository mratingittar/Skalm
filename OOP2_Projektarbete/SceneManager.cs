using Skalm.Display;
using Skalm.GameObjects;
using Skalm.GameObjects.Enemies;
using Skalm.GameObjects.Interfaces;
using Skalm.GameObjects.Items;
using Skalm.GameObjects.Stats;
using Skalm.Maps;
using Skalm.Maps.Tiles;
using Skalm.States.PlayerStates;
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
        private MonsterItemDropGen _monsterDropGen;
        private ISettings _settings;

        // CONSTRUCTOR I
        public SceneManager(ISettings settings, MapManager mapManager, DisplayManager displayManager, Player player, 
            EnemySpawner enemySpawner, ItemSpawner itemSpawner, PotionSpawner potionSpawner, KeySpawner keySpawner)
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
            _monsterDropGen = new MonsterItemDropGen(_itemSpawner, _potionSpawner, _keySpawner);

            ItemPickup.onItemPickup += RemoveGameObject;
            Enemy.OnEnemyDeath += HandleItemDrop;
            Enemy.OnEnemyDeath += RemoveGameObject;
        }

        // NEW GAME
        public void NewGame(GameManager gameManager)
        {
            _potionSpawner.ScalingMultiplier = 0;
            _enemySpawner.ScalingMultiplier = 0;
            _itemSpawner.ScalingMultiplier = 0;

            InitializePlayer();
            InitializeScene();

            Player.statsObject.OnDeath += gameManager.GameOver;
        }

        // INITIALIZE PLAYER
        public void InitializePlayer()
        {
            if (PlayerName.Length == 0)
                PlayerName = "Nameless";

            Player.InitializePlayer(_mapManager.MapGenerator.CurrentMap.PlayerSpawnPosition, PlayerName, _settings.PlayerSprite, _settings.PlayerColor);
            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);
        }

        // NEXT LEVEL
        public void NextLevel()
        {
            _displayManager.Eraser.EraseAll();
            ResetScene();

            IncrementScaling();
            _mapManager.MapGenerator.NextLevel();
            ResetPlayer();
            Player.NextFloor();
            InitializeScene();

            _displayManager.DisplayHUD();
            _mapManager.MapPrinter.DrawMap();

            Player.UpdateAllDisplays();
            Player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateMove);
        }

        // INCREMENT SCALING
        private void IncrementScaling()
        {
            _itemSpawner.ScalingMultiplier++;
            _enemySpawner.ScalingMultiplier++;
            _potionSpawner.ScalingMultiplier++;
        }

        // HANDLE ITEM DROP ON DEATH
        private void HandleItemDrop(Enemy e)
        {
            if (_monsterDropGen.DetermineItemDrop(e.statsObject.Experience))
                GameObjectsInScene.Add(_monsterDropGen.GenerateItemPickup(e.GridPosition, e.statsObject.Experience));
        }

        // RESET PLAYER
        public void ResetPlayer()
        {
            Player.SetPlayerPosition(_mapManager.MapGenerator.CurrentMap.PlayerSpawnPosition);
            Player.PlayerStateMachine.ChangeState(EPlayerStates.PlayerStateIdle);
            GameObjectsInScene.Add(Player);
            ActorsInScene.Add(Player);
        }

        // INITIALIZE SCENE
        public void InitializeScene()
        {
            SpawnObjects(_mapManager.MapGenerator.CurrentMap, EMapObjects.Enemies, _enemySpawner);
            GameObjectsInScene.Where(obj => obj is Enemy).ToList().ForEach(e => ActorsInScene.Add((Enemy)e));
            SpawnObjects(_mapManager.MapGenerator.CurrentMap, EMapObjects.Items, _itemSpawner);
            SpawnObjects(_mapManager.MapGenerator.CurrentMap, EMapObjects.Potions, _potionSpawner);
            SpawnObjects(_mapManager.MapGenerator.CurrentMap, EMapObjects.Keys, _keySpawner);
            AddObjectsToMap();
        }

        // SPAWN OBJECTS IN SCENE
        private void SpawnObjects(Map map, EMapObjects objectType, ISpawner<GameObject> spawner)
        {
            int minimum = map.ObjectsInMap[objectType].Item1;
            foreach (Vector2Int position in map.ObjectsInMap[objectType].Item2)
            {
                GameObjectsInScene.Add(spawner.Spawn(position));
                minimum--;
            }

            while (minimum > 0)
            {
                GameObjectsInScene.Add(spawner.Spawn(_mapManager.GetRandomFloorPosition()));
                minimum--;
            }
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
            var objects = GameObjectsInScene.ToArray();
            for (int i = 0; i < objects.Length; i++)
            {
                RemoveGameObject(objects[i]);
            }
            GameObjectsInScene.Clear();

            _displayManager.ClearMessageQueue();
            _displayManager.ClearMessageSection();
            _mapManager.MapGenerator.ResetGrid();
        }

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
