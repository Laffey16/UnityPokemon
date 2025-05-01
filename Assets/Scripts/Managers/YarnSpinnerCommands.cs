using System;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class YarnSpinnerCommands : MonoBehaviour
{

    public UnityEvent OnStartBattle;
    [SerializeField] private BattleManager battleManager;

    private void OnEnable()

    {
        battleManager.OnBattleEnd += OnBattleEnd;
    }

    [YarnCommand("StartBattle")]
    public void StartBattle()
    {
        OnStartBattle?.Invoke();
        battleManager.StartBattle();
    }

    private void OnDestroy()
    {
        battleManager.OnBattleEnd -= OnBattleEnd;
    }

    private void OnDisable()
    {
        battleManager.OnBattleEnd -= OnBattleEnd;
    }
    private void OnBattleEnd()
    {
        DialogueRunner dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        dialogueRunner.Stop();
    }
}
