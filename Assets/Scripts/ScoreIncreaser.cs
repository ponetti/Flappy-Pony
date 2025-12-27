using UnityEngine;

public class ScoreIncreaser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.UpdateScore();
    }
}