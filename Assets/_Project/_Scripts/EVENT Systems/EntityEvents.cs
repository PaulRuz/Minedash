using System;
public static class EntityEvents {

    #region Bullet Events
    public static event Action OnBulletShow;
    public static void BulletShow() {
        OnBulletShow?.Invoke();
    }
    public static event Action OnBulletLaunch;
    public static void BulletLaunch() {
        OnBulletLaunch?.Invoke();
    }
    public static event Action OnBulletTakeDamage;
    public static event Action<HitPointData> OnBulletTakeDamageData;
    public static void BulletTakeDamage( HitPointData hitPointData ) {
        OnBulletTakeDamageData?.Invoke( hitPointData );
        OnBulletTakeDamage?.Invoke();
    }
    #endregion

    #region Mine Events
    public static event Action OnMineTakeDamage;
    public static event Action<HitPointData> OnMineTakeDamageData;
    public static void MineTakeDamage( HitPointData hitPointData ) {
        OnMineTakeDamageData?.Invoke( hitPointData );
        OnMineTakeDamage?.Invoke();
    }
    public static event Action OnMineGetOre;
    public static event Action<HitPointData> OnMineGetOreData;
    public static void MineGetOre( HitPointData hitPointData ) {
        OnMineGetOreData?.Invoke( hitPointData );
        OnMineGetOre?.Invoke();
    }
    public static event Action OnMineDestroy;
    public static event Action<HitPointData> OnMineDestroyData;
    public static void MineDestroy( HitPointData hitPointData ) {
        OnMineDestroyData?.Invoke( hitPointData );
        OnMineDestroy?.Invoke();
    }
    #endregion

    #region Enemy Events
    public static event Action OnEnemyTakeDamage;
    public static event Action<HitPointData> OnEnemyTakeDamageData;
    public static void EnemyTakeDamage( HitPointData hitPointData ) {
        OnEnemyTakeDamageData?.Invoke( hitPointData );
        OnEnemyTakeDamage?.Invoke();
    }
    #endregion
}