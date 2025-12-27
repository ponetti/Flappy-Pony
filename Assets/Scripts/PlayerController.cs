using UnityEngine;

public class PlayerController: MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float _velocitySpeed;
    [SerializeField] private float _rotationSpeed;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private CapsuleCollider2D _capsuleCollider2D;

    private void Awake()
    {
        Instance = this;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        InvokeRepeating("AutoFlyAnimation", 0f, 0.35f);
    }

    public void MakePlayable()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        CancelInvoke("AutoFlyAnimation");
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameGoing) return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (transform.position.y > 5.5f) return;

            _rigidbody2D.linearVelocity = Vector2.up * _velocitySpeed;
            _animator.SetTrigger("Fly");
            GameManager.Instance.PlayerSounds(0);
        }
    }

    private void AutoFlyAnimation()
    {
        _animator.SetTrigger("Fly");
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, _rigidbody2D.linearVelocity.y * _rotationSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(_capsuleCollider2D);

        GameManager.Instance.PlayerSounds(1);
        GameManager.Instance.GameOver();
    }
}