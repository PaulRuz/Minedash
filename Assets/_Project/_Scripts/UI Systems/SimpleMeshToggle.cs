using UnityEngine;

public class SimpleMeshToggle : MonoBehaviour {
    [SerializeField] Mesh meshOn;
    [SerializeField] Mesh meshOff;
    [SerializeField] MeshFilter filter;
    bool isToggle;

    public void Toggle() {
        isToggle = !isToggle;
        if ( isToggle ) {
            filter.mesh = meshOn;
        } else
            filter.mesh = meshOff;
    }
}
