using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
public class GameBootstrap : MonoBehaviour {
    [SerializeField] UIScaleAnimator loading;
    [SerializeField] List<SystemInfo> coreSystemsGroup;
    readonly List<IInitializable> initializableSystems = new();

    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
    public static void StartGame() {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Awake() => StartCoroutine( LoadCoreSystems() );

    IEnumerator LoadCoreSystems() {
        loading.gameObject.SetActive( true );
        loading.PlayAnimation();

        coreSystemsGroup.Sort( ( a, b ) => a.LoadOrder.CompareTo( b.LoadOrder ) );
        foreach ( var systemInfo in coreSystemsGroup ) {
            var system = Instantiate( systemInfo.Prefab );
            if ( system.TryGetComponent( out IInitializable initComponent ) )
                initializableSystems.Add( initComponent );
            yield return null;
        }

        foreach ( var system in initializableSystems )
            yield return system.Initialize();

        loading.StopAnimation();
        YandexGame.GameReadyAPI();
        loading.gameObject.SetActive( false );

        GameEvents.GameInitComplete();
        initializableSystems.Clear();
    }
}

[Serializable]
public class SystemInfo {
    public GameObject Prefab;
    public int LoadOrder;
}

public interface IInitializable {
    IEnumerator Initialize();
}