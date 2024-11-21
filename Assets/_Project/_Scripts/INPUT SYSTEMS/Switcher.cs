using UnityEngine;
using UnityEngine.Events;

public class Switcher : MonoBehaviour {
    [SerializeField] UnityEvent OnON;
    [SerializeField] UnityEvent OnOFF;
    [SerializeField] Mesh meshON;
    [SerializeField] Mesh meshOFF;
    public bool isON = false;

    MeshFilter meshFilter;

    void Awake() {
        meshFilter = GetComponent<MeshFilter>();
    }

    public void Toggle() {
        isON = !isON;
        if ( isON ) {
            OnON?.Invoke();
            meshFilter.mesh = meshON;
        } else {
            OnOFF?.Invoke();
            meshFilter.mesh = meshOFF;
        }
    }
}