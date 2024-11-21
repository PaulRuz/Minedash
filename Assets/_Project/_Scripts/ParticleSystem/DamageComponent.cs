using UnityEngine;

public class DamageComponent : MonoBehaviour {

    void OnCollisionEnter2D( Collision2D collision ) {
        if ( collision.gameObject.TryGetComponent<Enemy>( out var enemy ) )
            enemy.GetTopDamage();
    }
}
