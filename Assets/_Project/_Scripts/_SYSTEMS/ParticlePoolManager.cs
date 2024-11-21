using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParticlePoolManager : MonoBehaviour, IInitializable {
    [Serializable]
    public struct ParticleSettings {
        public ParticleType Type;
        public BaseParticle Prefab;
        public float SectorAngle;
    }
    [Header( "Settings" )]
    [SerializeField] List<ParticleSettings> particleSettingsList;
    public bool IsSpawned { get; set; }

    Dictionary<ParticleType, ObjectPool<BaseParticle>> _particlePools = new();
    Dictionary<ParticleType, SectorDirectionGenerator> _angleGenerators = new();

    public IEnumerator Initialize() {
        yield return null;
    }

    void OnEnable() {
        EntityEvents.OnBulletTakeDamageData += BulletTakeShot;

        EntityEvents.OnMineDestroyData += MineDestroy;
        EntityEvents.OnMineGetOreData += MineGetOre;
        EntityEvents.OnMineTakeDamageData += MineTakeDamage;
        EntityEvents.OnEnemyTakeDamageData += EnemyTakeDamage;
    }

    readonly TextParticle _bulletTakeDamageParticle = new( ParticleType.TimePenality, Color.red, 2f, "-" );
    readonly TextParticle _mineGetOreParticle = new( ParticleType.MinePoints, Color.yellow, 2f );
    readonly TextParticle _mineTakeDamageParticle = new( ParticleType.MinePoints, Color.white, 1.6f );
    readonly TextParticle _mineDestroyParticle = new( ParticleType.MinePoints, Color.cyan, 2.4f );
    readonly TextParticle _enemyTakeDamageParticle = new( ParticleType.EnemyPoints, Color.magenta, 2f );

    #region Bullet Particles
    void BulletTakeShot( HitPointData data ) {
        SpawnParticle( ParticleType.Bullet, data );
        SpawnTextParticle( _bulletTakeDamageParticle, data );

    }
    #endregion

    #region Mine Particles
    void MineGetOre( HitPointData mineData ) {
        SpawnTextParticle( _mineGetOreParticle, mineData );
        SpawnParticle( ParticleType.Ore, mineData );
        SpawnParticle( ParticleType.Stone, mineData );
    }
    void MineTakeDamage( HitPointData mineData ) {
        SpawnTextParticle( _mineTakeDamageParticle, mineData );
        SpawnParticle( ParticleType.Stone, mineData );
    }

    void MineDestroy( HitPointData mineData ) {
        SpawnTextParticle( _mineDestroyParticle, mineData );
        SpawnParticle( ParticleType.MainOre, mineData );
        SpawnParticle( ParticleType.Stone, mineData, 4 );
    }
    #endregion

    #region Enemy Particles
    void EnemyTakeDamage( HitPointData enemyData ) {
        SpawnParticle( ParticleType.Enemy, enemyData );
        SpawnTextParticle( _enemyTakeDamageParticle, enemyData );
    }
    #endregion


    #region Private methods
    void SpawnParticle( ParticleType type, HitPointData data, int count = 1 ) {
        if ( _particlePools.ContainsKey( type ) == false )
            InitializePool( type );

        var particlePool = _particlePools[ type ];
        var directionGenerator = _angleGenerators[ type ];

        for ( int i = 0 ; i < count ; i++ ) {
            var particle = particlePool.GetNextObject();
            if ( particle == null )
                continue;

            particle.Transform.position = data.Position;
            var forceVector = directionGenerator.AngleToVectorInSector();
            particle.Activate( forceVector );
        }
    }

    void SpawnTextParticle( TextParticle particleData, HitPointData data ) {
        if ( _particlePools.ContainsKey( particleData.Type ) == false )
            InitializePool( particleData.Type );

        var particlePool = _particlePools[ particleData.Type ];
        var directionGenerator = _angleGenerators[ particleData.Type ];

        PointParticle particle = (PointParticle)particlePool.GetNextObject();
        if ( particle == null )
            return;

        particle.Transform.position = data.Position;
        var forceVector = directionGenerator.AngleToVectorInSector();

        particle.SetText( data.Points, particleData.Color, particleData.FontSize, particleData.Prefix );
        particle.Activate( forceVector );
    }


    struct TextParticle {
        public ParticleType Type;
        public Color Color;
        public float FontSize;
        public string Prefix;
        public TextParticle( ParticleType type, Color color = default, float fontSize = 1f, string prefix = "+" ) {
            Type = type;
            Color = color;
            FontSize = fontSize;
            Prefix = prefix;
        }
    }

    public void DeactivateAllParticles( ParticleType type ) {
        if ( _particlePools.TryGetValue( type, out var pool ) )
            pool.DeactivateAllObjects();
    }

    public void DeactivateAll() {
        foreach ( var pool in _particlePools.Values )
            pool.DeactivateAllObjects();
    }

    void InitializePool( ParticleType particleType ) {
        var settings = particleSettingsList.Find( ps => ps.Type == particleType );
        _particlePools[ particleType ] = new ObjectPool<BaseParticle>( settings.Prefab, transform );
        _angleGenerators[ particleType ] = new SectorDirectionGenerator( settings.SectorAngle );
    }

    void OnDisable() {
        EntityEvents.OnBulletTakeDamageData -= BulletTakeShot;

        EntityEvents.OnMineDestroyData -= MineDestroy;
        EntityEvents.OnMineGetOreData -= MineGetOre;
        EntityEvents.OnMineTakeDamageData -= MineTakeDamage;
        EntityEvents.OnEnemyTakeDamageData -= EnemyTakeDamage;
    }
    #endregion
}

public enum ParticleType {
    MinePoints, EnemyPoints,
    Bullet, Enemy,
    Stone, Ore, MainOre,
    TimePenality
}
