using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour, IInitializable {
    [Header( "Settings" )]
    [SerializeField] Bullet bulletPrefab;

    ObjectPool<Bullet> _bulletPool;
    Bullet _bullet;
    bool _canShoot;

    public IEnumerator Initialize() {
        _canShoot = false;
        _bulletPool = new ObjectPool<Bullet>( bulletPrefab, transform );
        yield return null;
    }

    void OnEnable() {
        GameEvents.OnGameStart += GetBullet;
        GameEvents.OnGameComplete += Deactivate;
        UIEvents.OnButtonShotPressed += Shoot;
        EntityEvents.OnBulletShow += IsShootReady;
    }

    void Shoot() {
        if ( !_canShoot )
            return;
        _canShoot = false;
        _bullet.SetMove();
        EntityEvents.BulletLaunch();

        _bullet = null;
        GetBullet();
    }
    void GetBullet() {
        if ( _bullet != null )
            return;
        _bullet = _bulletPool.GetNextObject();
        _bullet.Activate();
    }
    void Deactivate() {
        _bullet = null;
        _canShoot = false;
        StopAllCoroutines();
        _bulletPool.DeactivateAllObjects();
    }
    void IsShootReady() => _canShoot = true;

    void OnDisable() {
        GameEvents.OnGameStart -= GetBullet;
        GameEvents.OnGameComplete -= Deactivate;
        UIEvents.OnButtonShotPressed -= Shoot;
        EntityEvents.OnBulletShow -= IsShootReady;
    }
}