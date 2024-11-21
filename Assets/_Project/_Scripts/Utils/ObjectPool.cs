using System;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool<T> where T : Component {
    readonly T _prefab;
    readonly Transform _parentTransform;
    readonly Queue<T> _objects = new Queue<T>();
    readonly HashSet<T> _activeObjects = new HashSet<T>();

    public ObjectPool( T prefab, Transform parentTransform = null ) {
        _prefab = prefab;
        _parentTransform = parentTransform;
    }

    T CreateObject() {
        T obj = UnityEngine.Object.Instantiate( _prefab, _parentTransform );
        obj.gameObject.SetActive( false );
        if ( obj is IPoolable poolable ) {
            poolable.OnDeactivated += () => ReturnObjectToPool( obj );
        }
        return obj;
    }

    public T GetNextObject() {
        T obj;

        if ( _objects.Count > 0 ) {
            obj = _objects.Dequeue();
        } else {
            obj = CreateObject();
        }

        obj.gameObject.SetActive( true );
        _activeObjects.Add( obj );
        return obj;
    }

    public void DeactivateAllObjects() {
        // Деактивируем и возвращаем в очередь все активные объекты
        foreach ( var obj in _activeObjects ) {
            obj.gameObject.SetActive( false );
            _objects.Enqueue( obj );
        }
        _activeObjects.Clear(); // Очищаем список активных объектов
    }

    public void ReturnObjectToPool( T obj ) {
        if ( _activeObjects.Remove( obj ) ) {
            obj.gameObject.SetActive( false );
            _objects.Enqueue( obj );
        }
    }

    public Queue<T> GetQueue() {
        return _objects;
    }
    public HashSet<T> GetHash() {
        return _activeObjects;
    }
}

public interface IPoolable {
    event Action OnDeactivated;
}