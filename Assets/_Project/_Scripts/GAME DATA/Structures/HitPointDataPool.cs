using System;
using System.Collections.Generic;
using UnityEngine;

public class HitPointDataPool {
    Queue<HitPointData> _hitPointDataPool = new Queue<HitPointData>();

    public void SendHitPointData( Vector3 position, int points, Action<HitPointData> dataEvent ) {
        HitPointData data;
        if ( _hitPointDataPool.Count > 0 )
            data = _hitPointDataPool.Dequeue();
        else
            data = new HitPointData();

        data.Position = position;
        data.Points = points;

        dataEvent?.Invoke( data );
        _hitPointDataPool.Enqueue( data );
    }
}

public struct HitPointData {
    public Vector3 Position { get; set; }
    public int Points { get; set; }
    public HitPointData( Vector3 position, int points ) {
        Position = position;
        Points = points;
    }
}