using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Texts")] [SerializeField] private TMP_Text playerPokemonText;

    [SerializeField] private TMP_Text enemyPokemonText;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private TMP_Text enemyHPText;
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private TMP_Text enemyLevelText;

    [Header("Output Text")] [SerializeField]
    private TMP_Text bottomPanelText;

    [SerializeField] private TMP_Text debugText;

    [Header("Speed Settings")] [SerializeField]
    private float typeWriterSpeed = 0.05f;

    [SerializeField] private float textPauseTime = 1f;

    [Header("Sliders")] [SerializeField] private Slider playerHPSlider;

    [SerializeField] private Slider enemyHPSlider;
    [SerializeField] private float sliderLerpSpeed;

    [Header("Panels")] [SerializeField] private GameObject movesPanel;

    [SerializeField] private GameObject bottomPanel;

    [Header("Images")] [SerializeField] private Image playerPokemonImage;

    [SerializeField] private Image enemyPokemonImage;

    public static BattleUI Instance { get; private set; }
    public PokemonInstance PlayerPokemon { get; set; }

    public PokemonInstance EnemyPokemon { get; set; }

    private void Awake()
    {
        // If doesnt have an instance of AudioManager get one
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!Debug.isDebugBuild && !Application.isEditor) debugText.gameObject.SetActive(false);
    }

    public void InitialiseHUD()
    {
        try
        {
            playerPokemonText.text = PlayerPokemon.baseData.pokemonName;
            enemyPokemonText.text = EnemyPokemon.baseData.pokemonName;
            playerHPSlider.maxValue = PlayerPokemon.maxHP;
            enemyHPSlider.maxValue = EnemyPokemon.maxHP;
            playerHPSlider.value = PlayerPokemon.currentHP;
            enemyHPSlider.value = EnemyPokemon.currentHP;

            playerHPText.text = $"{PlayerPokemon.currentHP}/{PlayerPokemon.maxHP}";
            enemyHPText.text = $"{EnemyPokemon.currentHP}/{EnemyPokemon.maxHP}";
            playerLevelText.text = $"Lv {PlayerPokemon.currentLevel}";
            enemyLevelText.text = $"Lv {EnemyPokemon.currentLevel}";
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Failed to setup HUD");
        }

        try
        {
            playerPokemonImage.sprite = PlayerPokemon.baseData.pokemonBackSprite;
            enemyPokemonImage.sprite = EnemyPokemon.baseData.pokemonSprite;
        }
        catch (NullReferenceException)
        {
            Debug.LogError("Failed to setup Pokemon Images");
        }
    }

    public IEnumerator LerpPlayerHPSlider(float newHP)
    {
        var startHP = playerHPSlider.value;
        var elapsedTime = 0f;
        var duration = Mathf.Abs(startHP - newHP) / sliderLerpSpeed; // Adjust speed based on change

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            playerHPSlider.value = Mathf.Lerp(startHP, newHP, elapsedTime / duration);
            playerHPText.text = $"{Mathf.Round(playerHPSlider.value)}/{playerHPSlider.maxValue}";
            if (playerHPSlider.value <= 0)
            {
                playerHPSlider.value = 0;
                break;
            }

            yield return null; // Wait for the next frame
        }

        Debug.Log("Finished slider lerp");
        // Ensure final value is set correctly
        playerHPSlider.value = newHP;
        playerHPText.text = $"{newHP}/{playerHPSlider.maxValue}";
    }

    public IEnumerator LerpEnemyHPSlider(float newHP)
    {
        var startHP = enemyHPSlider.value;
        var elapsedTime = 0f;
        var duration = Mathf.Abs(startHP - newHP) / sliderLerpSpeed; // Adjust speed based on change

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            enemyHPSlider.value = Mathf.Lerp(startHP, newHP, elapsedTime / duration);
            enemyHPText.text = $"{Mathf.Round(enemyHPSlider.value)}/{enemyHPSlider.maxValue}";
            if (enemyHPSlider.value <= 0) break;
            yield return null; // Wait for the next frame
        }

        Debug.Log("Finished slider lerp");


        // Ensure final value is set correctly
        enemyHPSlider.value = newHP;
        enemyHPText.text = $"{EnemyPokemon.currentHP}/{enemyHPSlider.maxValue}";
    }

    public void SetDebugText(string newDebugText)
    {
        debugText.text = newDebugText;
    }

    public void SetBottomPanelText(string newText)
    {
        StartCoroutine(TypewriterText(newText));
    }

    public void ClearBottomPanelText()
    {
        bottomPanelText.text = string.Empty;
    }

    public void ResetBottomPanelText(string pokemonName = "")
    {
        if (!string.IsNullOrEmpty(pokemonName))
            //StartCoroutine($"What will {char.ToUpper(pokemonName[0]) + pokemonName[1..]} do?");
            StartCoroutine($"What will {pokemonName} do?");
        else
            bottomPanelText.text = "What will your Pokemon do?";
    }

    public void HardResetBottomPanelText(string pokemonName = "")
    {
        if (!string.IsNullOrEmpty(pokemonName))
            bottomPanelText.text = $"What will {char.ToUpper(pokemonName[0]) + pokemonName[1..]} do?";
        else
            bottomPanelText.text = "What will your Pokemon do?";
    }


    public IEnumerator TypewriterText(string message)
    {
        Debug.Log($"Message to typewrite is: {message}");
        // Clear the text before starting
        bottomPanelText.text = "";

        foreach (var letter in message)
        {
            bottomPanelText.text += letter;
            yield return new WaitForSeconds(typeWriterSpeed); // Speed controls how fast the text appears
        }

        yield return new WaitForSeconds(textPauseTime); // Pause before clearing or moving on
    }

    public void CloseMovesPanel()
    {
        movesPanel.SetActive(false);
        bottomPanel.SetActive(true);
        Debug.Log("Top panel opened.");
    }
}