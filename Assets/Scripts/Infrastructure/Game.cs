using System;
using EnemyScripts;
using Infrastructure.StateMachine;
using Services.Input;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public GameStateMachine StateMachine;
        public EnemiesGenerator EnemiesGenerator;

        public Game(ICoroutineRunner coroutineRunner, EnemySettings baseEnemySettings)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), baseEnemySettings);
        }
    }

    public class EnemiesGenerator : MonoBehaviour
    {



    }
}