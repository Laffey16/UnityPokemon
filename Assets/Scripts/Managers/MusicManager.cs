using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip twinLeafTown;
    [SerializeField] private AudioClip battleTheme;

    [SerializeField] BattleManager battleManager;

    private void Awake()
    {
        battleManager.OnBattleStart += PlayBattleTheme;
        battleManager.OnBattleEnd += StopPlayingBattleTheme;
    }

    private void OnDisable()
    {
        battleManager.OnBattleStart -= PlayBattleTheme;
        battleManager.OnBattleEnd -= StopPlayingBattleTheme;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(twinLeafTown);
    }


    private void PlayBattleTheme()
    {
        AudioManager.Instance.PlayMusicWithFade(battleTheme);
    }

    private void StopPlayingBattleTheme()
    {
        AudioManager.Instance.PlayMusicWithFade(twinLeafTown);
    }
}
