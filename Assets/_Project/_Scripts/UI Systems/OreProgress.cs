using System.Collections.Generic;
using UnityEngine;

public class OreProgress : MonoBehaviour {
    [SerializeField] PlayerStatsSO playerStats;
    [SerializeField] List<GameObject> currentOres;
    [SerializeField] List<GameObject> completedLevels;

    void Awake() {
        PlayerLevelEvents.OnPlayerLevelUpData += SetLevel;
        GameEvents.OnGameInitComplete += UpdateLevelView;
    }

    void UpdateLevelView() {
        SetLevel( playerStats.Level );
    }

    void SetLevel( int level ) {
        if ( level >= 8 )
            level = 8;

        var value = level - 1;
        currentOres[ value ].SetActive( true );

        if ( value > 0 ) {
            for ( int i = 0 ; i < value ; i++ ) {
                completedLevels[ i ].SetActive( true );
            }
        }
    }

    void OnDisable() {
        PlayerLevelEvents.OnPlayerLevelUpData -= SetLevel;
        GameEvents.OnGameInitComplete -= UpdateLevelView;
    }
}