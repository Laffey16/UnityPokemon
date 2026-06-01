using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, VICTORY, DEFEAT }
public class BattleManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Transform moveButtonContainer;

    [SerializeField] private Button moveButton;
    private PokemonInstance enemyPokemon;
    [SerializeField] private PokemonInstance playerPokemon;


    public event Action OnBattleStart;
    public event Action OnBattleEnd;

    public BattleState gameState;


    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        gameState = BattleState.START;
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
        gameState = BattleState.PLAYERTURN;
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

        if (gameState == BattleState.VICTORY)
        {
            var yarnStorage = FindFirstObjectByType<InMemoryVariableStorage>();
            yarnStorage.SetValue("$victory", true);
        }

        if (gameState == BattleState.DEFEAT)
        {
            var yarnStorage = FindFirstObjectByType<InMemoryVariableStorage>();
            yarnStorage.SetValue("$defeat", true);
        }

        OnBattleEnd?.Invoke();
    }

    public void Attack(MoveInstance move)
    {

        StartCoroutine(AttackSequence(move));
    }

    private IEnumerator AttackSequence(MoveInstance move)
    {
        Debug.Log(move.moveData.moveName);

        if (gameState == BattleState.PLAYERTURN)
        {
            yield return StartCoroutine(DamagePokemon(playerPokemon, enemyPokemon, move));
            move.currentPP--;
            gameState = BattleState.ENEMYTURN;
            yield return StartCoroutine(GameLoop());
        }
        else if (gameState == BattleState.ENEMYTURN)
        {

            yield return StartCoroutine(DamagePokemon(enemyPokemon, playerPokemon, move));
            move.currentPP--;
            gameState = BattleState.PLAYERTURN;
            yield return StartCoroutine(GameLoop());
        }
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator GameLoop()
    {
        if (playerPokemon.isFainted)
        {
            Debug.Log("Player Pokemon fainted!");
            gameState = BattleState.DEFEAT;
        }
        else if (enemyPokemon.isFainted)
        {
            Debug.Log("Enemy Pokemon fainted!");
            gameState = BattleState.VICTORY;
            playerPokemon.GainExp(CalculateExpGain(enemyPokemon));
        }
        if (gameState.Equals(BattleState.PLAYERTURN))
        {
            //BattleUI.Instance.SetBottomPanelText($"What will {playerPokemon.baseData.pokemonName} do?");
        }
        else if (gameState.Equals(BattleState.ENEMYTURN))
        {
            // Enemy's turn logic
            yield return StartCoroutine(EnemyTurn());
        }

        if (gameState.Equals(BattleState.VICTORY))
        {
            BattleUI.Instance.SetBottomPanelText("Enemy Pokemon fainted! You won the battle!");
            yield return new WaitForSeconds(2f);
            EndBattle();
        }

        if (gameState.Equals(BattleState.DEFEAT))
        {
            BattleUI.Instance.SetBottomPanelText("Your Pokemon fainted! You lost the battle!");
            yield return new WaitForSeconds(2f);

            EndBattle();
        }
    }


    private IEnumerator DamagePokemon(PokemonInstance attacker, PokemonInstance defender, MoveInstance move)
    {
        var effectiveness = TypeEffectivenessCalculator.CalculateEffectiveness(move.moveData, defender);
        int damageCalculation = (int)(move.moveData.power * attacker.attack / (float)defender.defense * effectiveness);

        bool isCriticalHit = //6.25% chance of a critical hit
            UnityEngine.Random.Range(0, 100) < 6.25f;


        if (isCriticalHit)
        {
            //2 times damage for critical hit from Platinum.
            damageCalculation *= 2;
        }

        defender.TakeDamage(damageCalculation);
        BattleUI.Instance.CloseMovesPanel();

        BattleUI.Instance.SetBottomPanelText($"{attacker.baseData.pokemonName} used {move.moveData.moveName}!");
        BattleUI.Instance.SetDebugText(
            $"Damage done with {move.moveData.moveName} was {damageCalculation}. The attacker had {attacker.attack} attack and the defender had {defender.defense} defense. It had a effectiveness multiplier of {effectiveness}.");

        if (gameState.Equals(BattleState.PLAYERTURN))
            yield return StartCoroutine(BattleUI.Instance.LerpEnemyHPSlider(defender.currentHP));
        else if (gameState.Equals(BattleState.ENEMYTURN))
            yield return StartCoroutine(BattleUI.Instance.LerpPlayerHPSlider(defender.currentHP));
        Debug.Log($"{defender.baseData.pokemonName} is now on {defender.currentHP} HP!");
        if (effectiveness > 1)
            BattleUI.Instance.SetBottomPanelText("It was super effective!");
        else if (effectiveness < 1 && effectiveness > 0)
            BattleUI.Instance.SetBottomPanelText("It was not very effective...");
        else if (effectiveness == 0) BattleUI.Instance.SetBottomPanelText("It had no effect!");

        if (isCriticalHit)
            BattleUI.Instance.SetBottomPanelText("That was a critical hit!");
    }

    public IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.25f);
        MoveInstance enemyMove = enemyPokemon.currentMoves[UnityEngine.Random.Range(0, enemyPokemon.currentMoves.Count)];

        Attack(enemyMove);
        BattleUI.Instance.SetBottomPanelText($"{enemyPokemon.baseData.pokemonName} used {enemyMove.moveData.moveName}!");


    }

    private int CalculateExpGain(PokemonInstance playerPokemon)
    {
        return Convert.ToInt16(1 * playerPokemon.currentLevel / 7f);
    }
}