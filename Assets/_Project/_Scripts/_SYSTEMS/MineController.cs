using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
public class MineController : MonoBehaviour, IInitializable {
    [Header( "Settings" )]
    [SerializeField] MineConfig mineConfig;
    [SerializeField] List<Mine> mines = new List<Mine>( 2 );

    public IEnumerator Initialize() {
        yield return null;
    }

    private void OnEnable() {
        StageEvents.OnLevelUpData += UpdateMines;
        GameEvents.OnGameMenuEnter += DeactivateMines;
    }

    void UpdateMines( int stageLevel ) {
        var mineSettings = mineConfig.GetItemByID( (byte)stageLevel );
        foreach ( var mine in mines )
            mine.Setup( mineSettings );

        int mineIndex = stageLevel % mines.Count == 0 ? 1 : 0;
        mines[ mineIndex ].Activate();
    }

    void DeactivateMines() {
        foreach ( var mine in mines )
            mine.Deactivate();
    }

    void OnDisable() {
        StageEvents.OnLevelUpData -= UpdateMines;
        GameEvents.OnGameMenuEnter -= DeactivateMines;
    }
}