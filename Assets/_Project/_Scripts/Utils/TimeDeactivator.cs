using UnityEngine;

public class TimeDeactivator : MonoBehaviour {
    [SerializeField] float timeDeactivate;
    public void Deactivate( ) {
        Invoke( nameof( Deactivate ), timeDeactivate );
    }

    void Hide() {
        gameObject.SetActive( false );
    }
}