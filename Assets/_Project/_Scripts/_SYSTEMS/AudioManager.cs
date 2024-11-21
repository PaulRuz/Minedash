using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour, IInitializable {
    [Header( "Settings" )]
    [SerializeField] AudioSource sourcePrefab;
    [SerializeField] AudioDataList baseAudioDataList;
    [SerializeField] AudioDataList mainAudioDataList;
    [SerializeField] Switcher audioSwitcher;

    ObjectPool<AudioSource> _audioSourcePool;
    Dictionary<SoundEffect, AudioClip> _audioClips = new();

    public IEnumerator Initialize() {
        _audioSourcePool = new ObjectPool<AudioSource>( sourcePrefab, transform );
        yield return LoadSounds( baseAudioDataList );
        StartCoroutine( LoadSounds( mainAudioDataList ) );
    }

    public void ToggleSound( bool isMuted ) => AudioListener.pause = isMuted;

    void OnEnable() {
        PlayerLevelEvents.OnSendStarToLevel += SendStarToLevel;
        PlayerLevelEvents.OnPlayerLevelUp += PlayerLevelUp;

        StageEvents.OnLevelUpData += StageLevelUp;

        EntityEvents.OnBulletTakeDamage += BulletTakeDamage;
        EntityEvents.OnBulletLaunch += BulletLaunch;

        EntityEvents.OnMineDestroy += MineDestroy;
        EntityEvents.OnMineGetOre += MineGetOre;
        EntityEvents.OnMineTakeDamage += MineTakeDamage;
        EntityEvents.OnEnemyTakeDamage += EnemyTakeDamage;
    }

    #region PlayerEvents
    public void SendStarToLevel() => PlaySound( SoundEffect.StarToLevelUp, 1f, 1.1f, false );
    public void PlayerLevelUp() {
        PlaySound( SoundEffect.PlayerLevelUp, 1f );
        PlaySound( SoundEffect.PlayerLevelUpVoice, 1f );
    }
    #endregion


    #region BulletEvents
    void BulletTakeDamage() => PlaySound( SoundEffect.EnemyShot, 1f );
    void BulletLaunch() => PlaySound( SoundEffect.LaunchPlayer, 0.4f );
    #endregion

    #region EnemyEvents
    void EnemyTakeDamage() {
        PlaySound( SoundEffect.PlayerDmgToEnemy, 0.8f );
        // рандомно выбираем из EnemyDmg01-03:
        PlaySound( SoundEffect.EnemyDmg01, 1f );
    }
    #endregion

    #region MineEvents
    void MineGetOre() {
        PlaySound( SoundEffect.MineBreak, 1f );
        PlaySound( SoundEffect.MineDmgOre, 1f );
    }
    void MineTakeDamage() => PlaySound( SoundEffect.MineDmg, 1f );
    void MineDestroy() {
        PlaySound( SoundEffect.MineDmgOre, 1f );
        PlaySound( SoundEffect.MineBreak, 1f );
        PlaySound( SoundEffect.PlayerVoice, 0.6f );
    }
    #endregion


    #region LevelEvents
    void StageLevelUp( int stageLevel ) {
        if ( stageLevel > 1 ) {
            var pitch = 1f + stageLevel / 10f;
            PlaySound( SoundEffect.GetStar, 1.1f, pitch, false );
        }
    }
    #endregion

    #region UIEvents
    void PlayClock01() => PlaySound( SoundEffect.Clock01, 0.8f, 1f, false );
    void PlayClock02() => PlaySound( SoundEffect.Clock02, 0.8f, 1f, false );
    #endregion

    #region private Methods
    IEnumerator LoadSounds( AudioDataList audioDataList ) {
        foreach ( var audioData in audioDataList.audio ) {
            var handle = Addressables.LoadAssetAsync<AudioClip>( audioData.audioClipRef );
            yield return handle;
            if ( handle.Status == AsyncOperationStatus.Succeeded )
                _audioClips[ audioData.soundEffect ] = handle.Result;
            else
                Debug.LogWarning( $"Failed to load sound: {audioData.soundEffect}" );
        }
    }

    void PlaySound( SoundEffect soundEffect, float volume = 1f, float pitch = 1f, bool randomPitch = true ) {
        if ( AudioListener.pause )
            return;

        if ( _audioClips.TryGetValue( soundEffect, out AudioClip clip ) ) {
            AudioSource source = _audioSourcePool.GetNextObject();

            SetAudioSourceSettings( source, clip, volume, pitch, randomPitch );
            StartCoroutine( ReturnAudioSourceAfterPlaying( source, clip.length ) );
        } else {
            Debug.LogWarning( "Sound not found." );
        }
    }

    // настройка звука для воспроизведения
    void SetAudioSourceSettings( AudioSource source, AudioClip clip, float volume, float pitch, bool randomPitch ) {
        source.clip = clip;
        source.pitch = randomPitch ? Random.Range( 0.8f, 1.2f ) : pitch;
        source.volume = volume;
        source.Play();
    }
    IEnumerator ReturnAudioSourceAfterPlaying( AudioSource source, float duration ) {
        yield return new WaitForSeconds( duration );
        source.Stop();
        _audioSourcePool.ReturnObjectToPool( source );
    }
    #endregion

    private void OnDisable() {
        _audioSourcePool?.DeactivateAllObjects();
        PlayerLevelEvents.OnSendStarToLevel -= SendStarToLevel;
        PlayerLevelEvents.OnPlayerLevelUp -= PlayerLevelUp;

        StageEvents.OnLevelUpData -= StageLevelUp;

        EntityEvents.OnBulletTakeDamage -= BulletTakeDamage;
        EntityEvents.OnBulletLaunch -= BulletLaunch;

        EntityEvents.OnMineDestroy -= MineDestroy;
        EntityEvents.OnMineGetOre -= MineGetOre;
        EntityEvents.OnMineTakeDamage -= MineTakeDamage;
        EntityEvents.OnEnemyTakeDamage -= EnemyTakeDamage;
    }
}

public enum SoundEffect {
    EnemyDmg01, EnemyDmg02, EnemyDmg03, EnemyShot,
    GetStar,
    LaunchPlayer,
    MineBreak,
    MineDmg,
    MineDmgOre,
    PlayerVoice,
    PlayerDmgToEnemy,
    PlayerEndGame,
    PlayerLevelUp, PlayerLevelUpVoice,
    PlayerShowMaxResult,
    StarToLevelUp,
    Switch,
    Clock01, Clock02,
}