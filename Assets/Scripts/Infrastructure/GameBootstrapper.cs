using EnemyScripts;
using Infrastructure.StateMachine.States;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private Game _game;

        [SerializeField] private EnemySettings baseEnemySettings;

        private void Awake()
        {
            _game = new Game(this, baseEnemySettings);
            
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(this);
        }
        
    }
}
