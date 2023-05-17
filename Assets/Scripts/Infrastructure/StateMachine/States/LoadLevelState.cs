using System;
using CameraLogic;
using EnemyScripts;
using PlayerScripts;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPoint = "InitialPoint";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName) 
            => _sceneLoader.Load(sceneName, OnLoaded);

        public void Exit()
        {
            
        }

        private void OnLoaded()
        {
            var initPosition = GameObject.FindWithTag(InitialPoint);
            
            GameObject hero = Instantiate("Player/VoodooCustomCharacter", initPosition.transform.position);
            hero.GetComponent<Player>().moneyText = initPosition.GetComponent<InitialPoint>().playerMoney;
            Instantiate("UI/UI");
            CameraFollow(hero);
        
            _gameStateMachine.Enter<GenerateEnemiesState>();
        }

        private void CameraFollow(GameObject gameObject)
        {
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(gameObject);
        }

        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
        
        private static GameObject Instantiate(string path, Vector3 at)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab, at, Quaternion.identity);
        }
    }

    public class GenerateEnemiesState : IState
    {
        private const int MinInclusive = -25;
        private const int MaxInclusive = 25;
        private readonly GameStateMachine _gameStateMachine;
        private readonly EnemySettings _enemySettings;
        private ObjectPool<Enemy> _enemiesPool;
        private int currentEnemiesCount;

        public GenerateEnemiesState(GameStateMachine gameStateMachine, EnemySettings enemySettings)
        {
            _gameStateMachine = gameStateMachine;
            _enemySettings = enemySettings;
            _enemiesPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeFromPool, OnReturnToPool, OnDestroyObject, false,
                10, 100);
        }

        public void Enter()
        {
            while (currentEnemiesCount < _enemySettings.MaxEnemies)
            {
                _enemiesPool.Get();
                currentEnemiesCount++;
            }
        }

        public void Exit()
        {
            
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float randomPosX = Random.Range(MinInclusive, MaxInclusive);
            float randomPosZ = Random.Range(MinInclusive, MaxInclusive);

            return  new Vector3(randomPosX, 0, randomPosZ);
        }

        private Enemy CreateEnemy()
        {
            var enemyPrefab = Instantiate("Enemies/Enemy");

            if (enemyPrefab.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.Init(_enemySettings);
                enemy.Disable += ReturnEnemyPool;
                return enemy;
            }
            else
            {
                throw new Exception("Enemy lost script");
            }
        }

        private void OnTakeFromPool(Enemy enemy)
        {
            enemy.gameObject.SetActive(true);
            enemy.transform.position = GetRandomSpawnPosition();
        }

        private void ReturnEnemyPool(Enemy enemy)
        {
            _enemiesPool.Release(enemy);
        }

        private void OnReturnToPool(Enemy enemy)
        {
            enemy.ResetHealth();
            enemy.gameObject.SetActive(false);
            enemy.DropLoot();
            _enemiesPool.Get();
        }

        private void OnDestroyObject(Enemy enemy)
        {
            Object.Destroy(enemy);
        }

        private static GameObject Instantiate(string path)
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab);
        }
    }
}