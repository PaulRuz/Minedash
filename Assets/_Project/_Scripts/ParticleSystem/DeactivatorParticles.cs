using UnityEngine;

public class DeactivatorParticles : MonoBehaviour {
    private void OnTriggerExit2D( Collider2D collider ) {
        if ( collider.TryGetComponent<IDeactivatable>( out var obj ) )
            obj.Deactivate();
    }
}