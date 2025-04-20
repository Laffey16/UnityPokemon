using System;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class YarnSpinnerCommands : MonoBehaviour
{

    public UnityEvent OnStartBattle;
    [SerializeField] private BattleManager battleManager;
    [YarnCommand("StartBattle")]

    public void StartBattle()
    {
        OnStartBattle?.Invoke();
        battleManager.StartBattle();
    }
}
