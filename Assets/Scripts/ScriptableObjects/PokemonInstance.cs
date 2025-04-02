using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PokemonInstance
{
    public enum StatusCondition
    {
        None,
        Paralyzed,
        Poisoned,
        BadlyPoisoned,
        Burned,
        Frozen,
        Asleep
    }

    public int attack;
    private int attackEV;
    private int attackIV;

    [Header("Base Data")] public PokemonData baseData;

    public int currentExp;
    public int currentHP;

    [Header("Current Stats")] public int currentLevel;

    public List<MoveInstance> currentMoves = new();
    public int defense;
    private int defenseEV;
    private int defenseIV;
    public int expToNextLevel;

    [Header("Effort Values (EVs)")] private int hpEV;

    [Header("Individual Values (IVs)")] private int hpIV;

    public int maxHP;
    public int spAtk;
    private int spAtkEV;
    private int spAtkIV;
    public int spDef;
    private int spDefEV;
    private int spDefIV;
    public int speed;
    private int speedEV;
    private int speedIV;

    [Header("Battle Status")] public StatusCondition status;

    public PokemonInstance(PokemonData basePokemondata)
    {
        baseData = basePokemondata;
    }

    public PokemonInstance()
    {
        // Generate random settings for all stats

        baseData = ScriptableObject.CreateInstance<PokemonData>();
        baseData.pokemonName = "RANDOM";
        baseData.baseHP = Random.Range(50, 250);
        baseData.baseAttack = Random.Range(10, 100);
        baseData.baseDefense = Random.Range(10, 100);
        baseData.baseSpAtk = Random.Range(10, 100);
        baseData.baseSpDef = Random.Range(10, 100);
        baseData.baseSpeed = Random.Range(10, 100);

        baseData.primaryType = PokemonData.PokemonType.Water;
        baseData.secondaryType = PokemonData.PokemonType.Flying;
        baseData.learnableMoves = new List<MoveData>
        {
            new("Tackle", 40, 35, PokemonData.PokemonType.Normal, MoveData.MoveCategory.Physical, 100, 1),
            new("Growl", 0, 40, PokemonData.PokemonType.Normal, MoveData.MoveCategory.Status, 100, 2),
            new("Quick Attack", 40, 30, PokemonData.PokemonType.Normal, MoveData.MoveCategory.Physical, 100, 3),
            new("Thunder Shock", 40, 30, PokemonData.PokemonType.Electric, MoveData.MoveCategory.Special, 100, 4)
        };
        InitialisePokemon();
        for (var i = 0; i < 30; i++) LevelUp();
    }

    public bool isFainted => currentHP <= 0;


    public void InitialisePokemon()
    {
        GenerateIVs();
        if (baseData.pokemonName != "Random") currentLevel = 1;

        GenerateStartingMoves();
        CalculateStats();
        currentHP = maxHP;
        status = StatusCondition.None;
    }

    private void GenerateIVs()
    {
        // Generate random IVs (0-31) for each stat
        hpIV = Random.Range(0, 32);
        attackIV = Random.Range(0, 32);
        defenseIV = Random.Range(0, 32);
        spAtkIV = Random.Range(0, 32);
        spDefIV = Random.Range(0, 32);
        speedIV = Random.Range(0, 32);
    }

    public void CalculateStats()
    {
        // HP calculation is different from other stats
        maxHP = CalculateHP();
        attack = CalculateStat(baseData.baseAttack, attackIV, attackEV);
        defense = CalculateStat(baseData.baseDefense, defenseIV, defenseEV);
        spAtk = CalculateStat(baseData.baseSpAtk, spAtkIV, spAtkEV);
        spDef = CalculateStat(baseData.baseSpDef, spDefIV, spDefEV);
        speed = CalculateStat(baseData.baseSpeed, speedIV, speedEV);

        CalculateExpToNextLevel();
    }

    private int CalculateHP()
    {
        return Mathf.FloorToInt((2 * baseData.baseHP + hpIV + hpEV / 4) * currentLevel / 100f) + currentLevel + 10;
    }

    private int CalculateStat(int baseStat, int iv, int ev)
    {
        return Mathf.FloorToInt((2 * baseStat + iv + ev / 4) * currentLevel / 100f) + 5;
    }

    private void CalculateExpToNextLevel()
    {
        // This is a simplified version - you might want to implement different exp curves
        expToNextLevel = currentLevel * currentLevel * currentLevel;
    }

    private void GenerateStartingMoves()
    {
        currentMoves.Clear();
        foreach (var move in baseData.learnableMoves)
            if (move.levelLearned <= currentLevel && currentMoves.Count < 4)
                currentMoves.Add(new MoveInstance(move));
    }

    public void TakeDamage(int damage)
    {
        currentHP = Mathf.Max(0, currentHP - damage);
        //currentHP = currentHP - damage;
        if (isFainted) HandleFaint();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    private void HandleFaint()
    {
        // Handle what happens when Pokemon faints
        Debug.Log($"{baseData.pokemonName} has fainted!");
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        while (currentExp >= expToNextLevel && currentLevel < 100) LevelUp();
    }

    public void LevelUp()
    {
        currentLevel++;
        var oldMaxHP = maxHP;
        CalculateStats();

        // Heal the difference in HP
        currentHP += maxHP - oldMaxHP;

        // Check for new moves
        CheckForNewMoves();
    }

    private void CheckForNewMoves()
    {
        foreach (var move in baseData.learnableMoves)
            if (move.levelLearned == currentLevel)
                TryLearnMove(new MoveInstance(move));
    }

    public void TryLearnMove(MoveInstance newMove)
    {
        if (currentMoves.Count < 4)
        {
            Debug.Log($"{baseData.pokemonName} has learned {newMove.moveData.moveName}!");
            currentMoves.Add(newMove);
        }
        else
        {
            //TODO Replace this with asking if they want to replace a move or just not learn one.
            Debug.Log($"Cannot learn {newMove.moveData.moveName} - already knows 4 moves!");
        }
    }
}

// Class to represent an instance of a move with PP tracking
[Serializable]
public class MoveInstance
{
    public MoveData moveData;
    public int currentPP;
    public int maxPP;

    public MoveInstance(MoveData data)
    {
        moveData = data;
        maxPP = data.powerPoints;
        currentPP = maxPP;
    }
}