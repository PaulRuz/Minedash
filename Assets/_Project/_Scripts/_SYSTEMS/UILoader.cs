using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UILoader : MonoBehaviour, IInitializable {
    [Header( "Settings" )]
    [SerializeField] GameObject envPrefab;
    [SerializeField] List<SystemInfo> baseUIGroup;
    [SerializeField] List<SystemInfo> gameUIGroup;

    Transform _env;

    public IEnumerator Initialize() {
        _env = Instantiate( envPrefab ).transform;
        yield return LoadGroups( baseUIGroup );
        yield return LoadGroups( gameUIGroup );
    }

    public void ShowResult() {
        _env.position = new Vector3(
            Random.Range( -0.64f, 0.64f ),
            _env.position.y, _env.position.z );
    }

    IEnumerator LoadGroups( List<SystemInfo> group) {
        group.Sort( ( a, b ) => a.LoadOrder.CompareTo( b.LoadOrder ) );
        foreach ( var systemInfo in group ) {
            var instance = Instantiate( systemInfo.Prefab );           
            yield return null;
        }
    }
}