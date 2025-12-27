using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool HasStartedBefore;
    public static bool CanStartViaKeyboard = true;

    [HideInInspector] public bool IsGameGoing;
    [HideInInspector] public bool IsGameOver;

    private int _score;
    private bool isStartButtonPressed;

    [SerializeField] private AudioSource _playerAudioSouce;
    [SerializeField] private AudioSource _interfaceAudioSouce;
    [SerializeField] private AudioClip[] _playerAudioClips;
    [SerializeField] private AudioClip[] _interfaceAudioClips;

    [SerializeField] private GameObject _startPanel;
    [SerializeField] private Animator _fadeAnimator;

    [SerializeField] private RectTransform _gameOverText;
    [SerializeField] private RectTransform _scorePanel;
    [SerializeField] private RectTransform _restartButton;

    [SerializeField] private TMP_Text _scoreCounterText;
    [SerializeField] private TMP_Text _finalScoreText;

    [SerializeField] private Image _ponyIcon;
    [SerializeField] private Sprite _ponySmileExpression;

    private void Awake()
    {
        Instance = this;

        if (!HasStartedBefore)
            Application.targetFrameRate = 60;
    }

    private void Start()
    {
        if (!HasStartedBefore) return;

        IsGameGoing = true;
        _scoreCounterText.enabled = true;
        Destroy(_startPanel);

        PlayerController.Instance.MakePlayable();
        PipeSpawner.Instance.StartSpawning();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanStartViaKeyboard)
            StartNewGameButton();
    }

    public void GameOver()
    {
        IsGameGoing = false;
        IsGameOver = true;
        _scoreCounterText.enabled = false;
        _fadeAnimator.SetTrigger("Death");

        if (PlayerPrefs.HasKey("best_score"))
        {
            int bestScore = PlayerPrefs.GetInt("best_score");

            if (_score > bestScore)
            {
                PlayerPrefs.SetInt("best_score", _score);
                _finalScoreText.text = $"Score: {_score}\n\nBest Score: {_score}";
                _ponyIcon.sprite = _ponySmileExpression;
            }
            else
                _finalScoreText.text = $"Score: {_score}\n\nBest Score: {bestScore}";
        }
        else
        {
            PlayerPrefs.SetInt("best_score", _score);
            _finalScoreText.text = $"Score: {_score}\n\nBest Score: {_score}";
            _ponyIcon.sprite = _ponySmileExpression;
        }

        Invoke("ShowGameOver", 0.7f);
        Invoke("ShowScorePanel", 1f);
        Invoke("ShowRestartButton", 1.2f);
    }

    public void PlayerSounds(int soundIndex)
    {
        _playerAudioSouce.PlayOneShot(_playerAudioClips[soundIndex]);
    }

    public void InterfaceSounds(int soundIndex)
    {
        _interfaceAudioSouce.PlayOneShot(_interfaceAudioClips[soundIndex]);
    }

    public void StartNewGameButton()
    {
        if (!HasStartedBefore)
            HasStartedBefore = true;

        if (isStartButtonPressed) return;

        isStartButtonPressed = true;
        CanStartViaKeyboard = false;

        _fadeAnimator.SetTrigger("Start");
        Invoke("RefreshScene", 0.4f);
    }

    public void UpdateScore()
    {
        _score++;
        _scoreCounterText.text = _score.ToString();

        InterfaceSounds(0);
    }

    private void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowGameOver()
    {
        _gameOverText.GetComponent<CanvasGroup>().alpha = 1f;
        _gameOverText.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_gameOverText.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack));
    }

    private void ShowScorePanel()
    {
        _scorePanel.GetComponent<CanvasGroup>().alpha = 1f;
        _scorePanel.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_scorePanel.transform.DOScale(2f, 0.5f).SetEase(Ease.OutBack));
    }

    private void ShowRestartButton()
    {
        _restartButton.GetComponent<CanvasGroup>().alpha = 1f;
        _restartButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        _restartButton.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_restartButton.transform.DOScale(2f, 0.5f).SetEase(Ease.OutBack));

        CanStartViaKeyboard = true;
    }
}