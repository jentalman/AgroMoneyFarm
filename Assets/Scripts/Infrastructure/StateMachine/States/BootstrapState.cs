﻿using System;
using Services.Input;

namespace Infrastructure.StateMachine.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter() 
        {
            RegisterServices();
            
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        private void EnterLoadLevel() 
            => _stateMachine.Enter<LoadLevelState, string>("MainScene");

        private void RegisterServices()
        {
            Game.InputService = RegisterInputService();
        }


        public void Exit()
        {
            
        }
        
        private MobileInputService RegisterInputService()
        {
            return new MobileInputService();
        }
    }
}