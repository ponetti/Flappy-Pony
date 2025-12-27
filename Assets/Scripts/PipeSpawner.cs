using UnityEngine;
using System.Collections;

public class PipeSpawner : MonoBehaviour
{
    public static PipeSpawner Instance;

    [SerializeField] private float _minHeightRange;
    [SerializeField] private float _maxHeightRange;
    [SerializeField] private float _spawnDelay;

    [SerializeField] private GameObject _pipePrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnPipes());
    }

    private IEnumerator SpawnPipes()
    {
        while (true)
        {
            if (!GameManager.Instance.IsGameGoing) yield break;

            yield return new WaitForSeconds(_spawnDelay);

            Vector3 spawnPosition = new Vector3(6f, Random.Range(_minHeightRange, _maxHeightRange), 0f);
            GameObject pipe = Instantiate(_pipePrefab, spawnPosition, Quaternion.identity);
            Destroy(pipe, 10f);
        }
    }
}