using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform moveButtonContainer;

    [SerializeField] private Button moveButton;
    private PokemonInstance enemyPokemon;
    [SerializeField] private PokemonInstance playerPokemon;


    public event Action OnBattleStart;
    public event Action OnBattleEnd;



    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        playerPokemon = GetComponent<PlayerPokemon>().firstPokemon;
        enemyPokemon = new PokemonInstance();

        BattleUI.Instance.HardResetBottomPanelText(playerPokemon.baseData.pokemonName);

        BattleUI.Instance.PlayerPokemon = playerPokemon;
        BattleUI.Instance.EnemyPokemon = enemyPokemon;
        BattleUI.Instance.InitialiseHUD();
        foreach (var move in playerPokemon.currentMoves)
        {
            var newButton = Instantiate(moveButton, moveButtonContainer);
            newButton.GetComponentInChildren<TMP_Text>().text = move.moveData.moveName;
            newButton.onClick.AddListener(() => Attack(move));
        }
    }

    public void StartBattle(PokemonInstance enemyPokemon)
    {
        Debug.Log($"Player has thrown out the Pokemon known as {playerPokemon.baseData.pokemonName}!");
        OnBattleStart?.Invoke();
    }

    public void StartBattle()
    {
        Debug.Log($"Player has thrown out the Pokemon known as {playerPokemon.baseData.pokemonName}!");
        BattleUI.Instance.ToggleUI();
        OnBattleStart?.Invoke();
    }

    public void EndBattle()
    {
        BattleUI.Instance.ToggleUI();
        OnBattleEnd?.Invoke();
    }

    public void Attack(MoveInstance move)
    {
        Debug.Log(move.moveData.moveName);

        // Example battle logic
        DamagePokemon(playerPokemon, enemyPokemon, move);
        move.currentPP--;
    }


    private void DamagePokemon(PokemonInstance attacker, PokemonInstance defender, MoveInstance move)
    {
        var effectiveness = TypeEffectivenessCalculator.CalculateEffectiveness(move.moveData, defender);
        var damageCalculation = (int)(move.moveData.power * attacker.attack / (float)defender.defense * effectiveness);

        defender.TakeDamage(damageCalculation);
        BattleUI.Instance.CloseMovesPanel();

        BattleUI.Instance.SetBottomPanelText($"{attacker.baseData.pokemonName} used {move.moveData.moveName}!");
        BattleUI.Instance.SetDebugText(
            $"Damage done with {move.moveData.moveName} was {damageCalculation}. The attacker had {attacker.attack} attack and the defender had {defender.defense} defense. It had a effectiveness multiplier of {effectiveness}.");
        StartCoroutine(BattleUI.Instance.LerpEnemyHPSlider(defender.currentHP));
        Debug.Log($"{defender.baseData.pokemonName} is now on {defender.currentHP} HP!");
        if (effectiveness > 1)
            BattleUI.Instance.SetBottomPanelText("It was super effective!");
        else if (effectiveness < 1 && effectiveness > 0)
            BattleUI.Instance.SetBottomPanelText("It was not very effective...");
        else if (effectiveness == 0) BattleUI.Instance.SetBottomPanelText("It had no effect!");
    }

    private int CalculateExpGain(PokemonInstance playerPokemon)
    {
        // Implement experience calculation formula
        return 0; // Placeholder
    }
}