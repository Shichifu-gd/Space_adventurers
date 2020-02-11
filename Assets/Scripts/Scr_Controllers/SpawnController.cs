using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public enum SpawnObject
{
    Enemy,
    Asteroid,
    Background,
}

public class SpawnController : MonoBehaviour
{
    private ObjectsForSpawn objectsForSpawn;
    [SerializeField] private PlayerController playerController;
    [SerializeField]
    private EnemyDistribution enemyDistribution = new EnemyDistribution();

    private int NumberObjects;

    private float BaseWaitingTime;
    private float CurrentWaitingTime;
    private float TimeBetweenSpawn;

    private bool OnActive;

    [SerializeField] private bool ActiveEnemy = true;
    [SerializeField] private bool ActiveAsteroid = true;
    [SerializeField] private bool ActiveBackground = true;

    private Vector3 SelectedPoint;

    [SerializeField] private List<Transform> SpawnPoint = new List<Transform>();

    private void Awake()
    {
        PoolManager.Instance.AddPool(PoolType.Pool_Asteroid);
        PoolManager.Instance.AddPool(PoolType.Pool_ShipEnemy);
        PoolManager.Instance.AddPool(PoolType.Pool_RocketEnemy);
        PoolManager.Instance.AddPool(PoolType.Pool_RocketPlayer);
        PoolManager.Instance.AddPool(PoolType.Pool_Background);
        objectsForSpawn = GetComponent<ObjectsForSpawn>();
    }

    private void Start()
    {
        BaseWaitingTime = 3.5f;
        CurrentWaitingTime = 0f;
        OnActive = false;
    }

    private void FixedUpdate()
    {
        if (CanSpawn()) StartCoroutine(SpawnLocation());
        if (!OnActive) CurrentWaitingTime += Time.deltaTime;
    }

    private bool CanSpawn()
    {
        if (CurrentWaitingTime >= BaseWaitingTime) return true;
        return false;
    }

    private IEnumerator SpawnLocation()
    {
        CurrentWaitingTime = 0f;
        OnActive = true;
        NumberObjects = GetRandomInt(5, 15);
        int currentNumberObjects = 0;
        while (true)
        {
            if (GetRandomBool())
            {
                TimeBetweenSpawn = GetRandomFloat(0.4f, 1f);
                yield return new WaitForSeconds(TimeBetweenSpawn);
                SelectedPoint = SpawnPoint[GetRandomInt(0, SpawnPoint.Count)].position;
                DetermineTypeObject();
                currentNumberObjects++;
                if (currentNumberObjects >= NumberObjects) break;
            }
        }
        BaseWaitingTime = GetRandomFloat(2.5f, 4f);
        OnActive = false;
    }

    private void DetermineTypeObject()
    {
        var random = GetRandomInt(0, 10);
        if (random == 0 || random == 1 || random == 3 || random == 4 || random == 5) ChooseFromList(SpawnObject.Asteroid);
        else if (random == 6 || random == 7 || random == 8) ChooseFromList(SpawnObject.Enemy);
        else if (random == 9) ChooseFromList(SpawnObject.Background);
    }

    private void ChooseFromList(SpawnObject SpawnObject)
    {
        GameObject preObject;
        var random = 0;
        if (SpawnObject == SpawnObject.Asteroid)
        {
            if (ActiveAsteroid)
            {
                random = GetRandomInt(0, objectsForSpawn.PreAsteroid.Length);
                preObject = objectsForSpawn.PreAsteroid[random];
                PoolManager.Instance.Spawn(PoolType.Pool_Asteroid, preObject, SelectedPoint);
                return;
            }
            else SpawnObject = SpawnObject.Enemy;
        }
        if (SpawnObject == SpawnObject.Enemy)
        {
            if (ActiveEnemy)
            {
                var score = playerController.GetScore();
                if (score > 150) score = 100; // TODO: Повысить n "score > {n}", при увеличении количества уровней
                enemyDistribution.CoefficientDistribution(score);
                preObject = enemyDistribution.EnemySelection(objectsForSpawn);
                PoolManager.Instance.Spawn(PoolType.Pool_ShipEnemy, preObject, SelectedPoint);
                return;
            }
        }
        if (SpawnObject == SpawnObject.Background)
        {
            if (ActiveBackground)
            {
                if (GetRandomBool())
                {
                    random = GetRandomInt(0, objectsForSpawn.PreForBackground.Length);
                    preObject = objectsForSpawn.PreForBackground[random];
                    SelectedPoint.y = GetRandomFloat(-20, -10);
                    PoolManager.Instance.Spawn(PoolType.Pool_Background, preObject, SelectedPoint);
                    return;
                }
            }
        }
    }

    private float GetRandomFloat(float min, float max)
    {
        if (min >= max) return Random.Range(0f, 1f);
        return Random.Range(min, max);
    }

    private int GetRandomInt(int min, int max)
    {
        if (min >= max) return Random.Range(0, 2);
        return Random.Range(min, max);
    }

    private bool GetRandomBool()
    {
        var random = Random.Range(0, 3);
        if (random != 0) return true;
        return false;
    }
}

[System.Serializable]
public class EnemyDistribution
{
    [SerializeField] private AnimationCurve EnemyLevel_1 = new AnimationCurve();
    [SerializeField] private AnimationCurve EnemyLevel_2 = new AnimationCurve();
    [SerializeField] private AnimationCurve EnemyLevel_3 = new AnimationCurve();
    [SerializeField] private AnimationCurve EnemyLevel_Rare = new AnimationCurve();

    private float[] Odds;
    private float[] EnemyLevels;

    public void CoefficientDistribution(float score)
    {
        Odds = new float[4];
        Odds[0] = EnemyLevel_1.Evaluate(score);
        Odds[1] = EnemyLevel_2.Evaluate(score);
        Odds[2] = EnemyLevel_3.Evaluate(score);
        Odds[3] = EnemyLevel_Rare.Evaluate(score);
    }

    public GameObject EnemySelection(ObjectsForSpawn objectsForSpawn)
    {
        var level = LevelSelection();
        if (level == 0) return objectsForSpawn.PreEnemyLevel_1[Random.Range(0, objectsForSpawn.PreEnemyLevel_1.Length)];
        if (level == 1) return objectsForSpawn.PreEnemyLevel_2[Random.Range(0, objectsForSpawn.PreEnemyLevel_2.Length)];
        if (level == 2) return objectsForSpawn.PreEnemyLevel_3[Random.Range(0, objectsForSpawn.PreEnemyLevel_3.Length)];
        if (level == 3) return objectsForSpawn.PreEnemyLevel_Rare[Random.Range(0, objectsForSpawn.PreEnemyLevel_Rare.Length)];
        return null;
    }

    private int LevelSelection()
    {
        float value = Random.Range(0, Odds.Sum());
        float sum = 0;
        for (int index = 0; index < Odds.Length; index++)
        {
            sum += Odds[index];
            if (value < sum) return index;
        }
        return 0;
    }
}