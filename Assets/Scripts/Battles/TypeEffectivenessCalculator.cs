using System.Collections.Generic;

public static class TypeEffectivenessCalculator
{
    // The effectiveness chart. Default is 1 (neutral).
    private static readonly Dictionary<PokemonData.PokemonType, Dictionary<PokemonData.PokemonType, float>> chart =
        new()
        {
            {
                PokemonData.PokemonType.Normal, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Rock, 0.5f },
                    { PokemonData.PokemonType.Ghost, 0f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Fire, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Fire, 0.5f },
                    { PokemonData.PokemonType.Water, 0.5f },
                    { PokemonData.PokemonType.Grass, 2f },
                    { PokemonData.PokemonType.Ice, 2f },
                    { PokemonData.PokemonType.Bug, 2f },
                    { PokemonData.PokemonType.Rock, 0.5f },
                    { PokemonData.PokemonType.Dragon, 0.5f },
                    { PokemonData.PokemonType.Steel, 2f }
                }
            },
            {
                PokemonData.PokemonType.Water, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Fire, 2f },
                    { PokemonData.PokemonType.Water, 0.5f },
                    { PokemonData.PokemonType.Grass, 0.5f },
                    { PokemonData.PokemonType.Ground, 2f },
                    { PokemonData.PokemonType.Rock, 2f },
                    { PokemonData.PokemonType.Dragon, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Electric, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Water, 2f },
                    { PokemonData.PokemonType.Electric, 0.5f },
                    { PokemonData.PokemonType.Grass, 0.5f },
                    { PokemonData.PokemonType.Ground, 0f },
                    { PokemonData.PokemonType.Flying, 2f },
                    { PokemonData.PokemonType.Dragon, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Grass, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Water, 2f },
                    { PokemonData.PokemonType.Fire, 0.5f },
                    { PokemonData.PokemonType.Grass, 0.5f },
                    { PokemonData.PokemonType.Poison, 0.5f },
                    { PokemonData.PokemonType.Ground, 2f },
                    { PokemonData.PokemonType.Flying, 0.5f },
                    { PokemonData.PokemonType.Bug, 0.5f },
                    { PokemonData.PokemonType.Rock, 2f },
                    { PokemonData.PokemonType.Dragon, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Ice, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Grass, 2f },
                    { PokemonData.PokemonType.Ground, 2f },
                    { PokemonData.PokemonType.Flying, 2f },
                    { PokemonData.PokemonType.Dragon, 2f },
                    { PokemonData.PokemonType.Fire, 0.5f },
                    { PokemonData.PokemonType.Water, 0.5f },
                    { PokemonData.PokemonType.Ice, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Fighting, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Normal, 2f },
                    { PokemonData.PokemonType.Ice, 2f },
                    { PokemonData.PokemonType.Rock, 2f },
                    { PokemonData.PokemonType.Dark, 2f },
                    { PokemonData.PokemonType.Steel, 2f },
                    { PokemonData.PokemonType.Poison, 0.5f },
                    { PokemonData.PokemonType.Flying, 0.5f },
                    { PokemonData.PokemonType.Psychic, 0.5f },
                    { PokemonData.PokemonType.Bug, 0.5f },
                    { PokemonData.PokemonType.Fairy, 0.5f },
                    { PokemonData.PokemonType.Ghost, 0f }
                }
            },
            {
                PokemonData.PokemonType.Poison, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Grass, 2f },
                    { PokemonData.PokemonType.Fairy, 2f },
                    { PokemonData.PokemonType.Poison, 0.5f },
                    { PokemonData.PokemonType.Ground, 0.5f },
                    { PokemonData.PokemonType.Rock, 0.5f },
                    { PokemonData.PokemonType.Ghost, 0.5f },
                    { PokemonData.PokemonType.Steel, 0f }
                }
            },
            {
                PokemonData.PokemonType.Ground, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Fire, 2f },
                    { PokemonData.PokemonType.Electric, 2f },
                    { PokemonData.PokemonType.Poison, 2f },
                    { PokemonData.PokemonType.Rock, 2f },
                    { PokemonData.PokemonType.Steel, 2f },
                    { PokemonData.PokemonType.Grass, 0.5f },
                    { PokemonData.PokemonType.Bug, 0.5f },
                    { PokemonData.PokemonType.Flying, 0f }
                }
            },
            {
                PokemonData.PokemonType.Flying, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Grass, 2f },
                    { PokemonData.PokemonType.Fighting, 2f },
                    { PokemonData.PokemonType.Bug, 2f },
                    { PokemonData.PokemonType.Electric, 0.5f },
                    { PokemonData.PokemonType.Rock, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Psychic, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Fighting, 2f },
                    { PokemonData.PokemonType.Poison, 2f },
                    { PokemonData.PokemonType.Psychic, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f },
                    { PokemonData.PokemonType.Dark, 0f }
                }
            },
            {
                PokemonData.PokemonType.Bug, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Grass, 2f },
                    { PokemonData.PokemonType.Psychic, 2f },
                    { PokemonData.PokemonType.Dark, 2f },
                    { PokemonData.PokemonType.Fire, 0.5f },
                    { PokemonData.PokemonType.Fighting, 0.5f },
                    { PokemonData.PokemonType.Poison, 0.5f },
                    { PokemonData.PokemonType.Flying, 0.5f },
                    { PokemonData.PokemonType.Ghost, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f },
                    { PokemonData.PokemonType.Fairy, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Rock, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Fire, 2f },
                    { PokemonData.PokemonType.Ice, 2f },
                    { PokemonData.PokemonType.Flying, 2f },
                    { PokemonData.PokemonType.Bug, 2f },
                    { PokemonData.PokemonType.Fighting, 0.5f },
                    { PokemonData.PokemonType.Ground, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Ghost, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Psychic, 2f },
                    { PokemonData.PokemonType.Ghost, 2f },
                    { PokemonData.PokemonType.Dark, 0.5f },
                    { PokemonData.PokemonType.Normal, 0f } // Normal moves have no effect on Ghosts
                }
            },
            {
                PokemonData.PokemonType.Dragon, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Dragon, 2f },
                    { PokemonData.PokemonType.Steel, 0.5f },
                    { PokemonData.PokemonType.Fairy, 0f }
                }
            },
            {
                PokemonData.PokemonType.Dark, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Psychic, 2f },
                    { PokemonData.PokemonType.Ghost, 2f },
                    { PokemonData.PokemonType.Fighting, 0.5f },
                    { PokemonData.PokemonType.Dark, 0.5f },
                    { PokemonData.PokemonType.Fairy, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Steel, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Ice, 2f },
                    { PokemonData.PokemonType.Rock, 2f },
                    { PokemonData.PokemonType.Fairy, 2f },
                    { PokemonData.PokemonType.Fire, 0.5f },
                    { PokemonData.PokemonType.Water, 0.5f },
                    { PokemonData.PokemonType.Electric, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            },
            {
                PokemonData.PokemonType.Fairy, new Dictionary<PokemonData.PokemonType, float>
                {
                    { PokemonData.PokemonType.Fighting, 2f },
                    { PokemonData.PokemonType.Dragon, 2f },
                    { PokemonData.PokemonType.Dark, 2f },
                    { PokemonData.PokemonType.Fire, 0.5f },
                    { PokemonData.PokemonType.Poison, 0.5f },
                    { PokemonData.PokemonType.Steel, 0.5f }
                }
            }
        };

    /// <summary>
    ///     Returns the multiplier for a single type matchup.
    /// </summary>
    public static float GetEffectivenessMultiplier(PokemonData.PokemonType attackType,
        PokemonData.PokemonType defenseType)
    {
        // If the attacking type has a chart and there is an entry for the defending type, return it; otherwise return neutral (1).
        if (chart.ContainsKey(attackType) && chart[attackType].ContainsKey(defenseType))
            return chart[attackType][defenseType];
        return 1f;
    }

    /// <summary>
    ///     Calculates overall effectiveness when attacking a defender with one or two types.
    /// </summary>
    /// <param name="move">The move being used.</param>
    /// <param name="defender">The defending Pokemon instance.</param>
    /// <returns>A multiplier (0, 0.25, 0.5, 1, 2, 4 etc.) representing the effectiveness.</returns>
    public static float CalculateEffectiveness(MoveData move, PokemonInstance defender)
    {
        // If the move is a status move, you might choose to return 1 (no multiplier)
        if (move.moveCategory == MoveData.MoveCategory.Status) return 1f;

        var multiplier = GetEffectivenessMultiplier(move.moveType, defender.baseData.primaryType);

        // If the Pokemon has a secondary type (and it's not "None"), multiply by its effectiveness
        if (defender.baseData.secondaryType != PokemonData.PokemonType.None)
            multiplier *= GetEffectivenessMultiplier(move.moveType, defender.baseData.secondaryType);

        return multiplier;
    }
}