using System.Collections.Generic;
using UnityEngine;

public abstract class ConfigBase<T> : ScriptableObject where T : class {
    [SerializeField] protected List<T> items;
    protected Dictionary<byte, T> itemDictionary;

    private void OnEnable() {
        InitializeDictionary();
    }

    void InitializeDictionary() {
        itemDictionary = new Dictionary<byte, T>();
        foreach ( var item in items ) {
            byte id = GetID( item );
            itemDictionary[ id ] = item;
        }
    }

    protected abstract byte GetID( T item );

    public T GetItemByID( byte id ) {
        if ( itemDictionary.TryGetValue( id, out T item ) ) {
            return item;
        } else {
            Debug.LogError( $"Item with ID {id} not found!" );
            return null;
        }
    }

    public void UpdateItems( List<T> updatedItems ) {
        items = updatedItems;
        InitializeDictionary();
    }
}

