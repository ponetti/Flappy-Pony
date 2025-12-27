using UnityEngine;

public class PipeMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        if (!GameManager.Instance.IsGameGoing) return;

        transform.position += Vector3.left * _speed * Time.deltaTime;
    }
}