using UnityEngine;

public class PlayerPokemon : MonoBehaviour
{
    // Class for all player Pokemon
    public PokemonData pokemonData;
    public PokemonInstance firstPokemon;
    public PokemonInstance secondPokemon;
    public PokemonInstance thirdPokemon;


    public void Awake()
    {
        firstPokemon = new PokemonInstance(pokemonData);
        firstPokemon.InitialisePokemon();
        for (var i = 0; i < 10; i++) firstPokemon.LevelUp();
        Debug.Log("Set up first pokemon");
    }
}