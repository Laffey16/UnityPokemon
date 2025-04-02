using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "Pokemon/Create New Pokemon")]
public class PokemonData : ScriptableObject
{
    public enum PokemonType
    {
        None,
        Normal,
        Fire,
        Water,
        Electric,
        Grass,
        Ice,
        Fighting,
        Poison,
        Ground,
        Flying,
        Psychic,
        Bug,
        Rock,
        Ghost,
        Dragon,
        Dark,
        Steel,
        Fairy
    }

    [Header("Basic Info")] public string pokemonName;

    public int pokedexNumber;
    public Sprite pokemonSprite;
    public Sprite pokemonBackSprite;
    public PokemonType primaryType;
    public PokemonType secondaryType;

    [Header("Base Stats")] public int baseHP;

    public int baseAttack;
    public int baseDefense;
    public int baseSpAtk;
    public int baseSpDef;
    public int baseSpeed;

    [Header("Battle")] public List<MoveData> learnableMoves;

    public float catchRate;
    public int baseExperienceYield;
    public float levelingRate;

    [Header("Physical Attributes")] public float height; // in meters

    public float weight; // in kilograms
    public string description;
}

[Serializable]
public class MoveData
{
    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

    public string moveName;
    public PokemonData.PokemonType moveType;
    public int power;
    public int accuracy;
    public int powerPoints;
    public string description;
    public bool isSpecial; // true for special moves, false for physical moves
    public int levelLearned;
    public MoveCategory moveCategory;


    public MoveData(string moveName, int power, int powerpoints, PokemonData.PokemonType pokemonMoveType,
        MoveCategory moveCategory, int accuracy, int levelLearned)
    {
        this.moveName = moveName;
        this.power = power;
        powerPoints = powerpoints;
        moveType = pokemonMoveType;
        this.moveCategory = moveCategory;
        this.accuracy = accuracy;
        this.levelLearned = levelLearned;
    }
}