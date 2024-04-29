using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Transform _spriteMask;
    [SerializeField] private CanvasGroup _gameUI;
    [SerializeField] private CanvasGroup _gameOverUI;

    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private SpriteRenderer _backgroundColor;
    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private TMP_Text _bestScoreText;
    private string _bestScoreFormat;

    [SerializeField] private TMP_Text _finalScoreText;
    private string _finalScoreFormat;

    private const string NameSave = "BestScore";
    private int _bestScore = 0;
    private int _currentlevel = 0;

    public void StartGame() =>
        _spriteMask.DOScale(50, 1f).SetEase(Ease.Linear).OnComplete(() => { GameManager.Instance.SwitchStateTo(GameState.Game); });

    public void RestartGame() => GameManager.Instance.SwitchStateTo(GameState.MainMenu);

    private void Awake()
    {
        _finalScoreFormat = _finalScoreText.text;
        _bestScoreFormat = _bestScoreText.text;

        LoadPlayerScore();
        _bestScoreText.text = string.Format(_bestScoreFormat, _bestScore);
    }

    void Start()
    {
        GameManager.Instance.OnGameEvent += ShowGameUI;
        GameManager.Instance.OnGameOverEvent += ShowGameOverUI;
        _coinManager.ChangeScoreEvent += UpdateLvlAndScore;
        UpdateLvlAndScore(0);
    }

    private void UpdateLvlAndScore(int value)
    {
        _scoreText.text = value.ToString();
        _finalScoreText.text = string.Format(_finalScoreFormat, value.ToString());
        if (value > _bestScore)
        {
            _bestScore = value;
            _bestScoreText.text = string.Format(_bestScoreFormat, _bestScore);
            SavePlayerScore();
        }

        int res = value / 10;
        if (res > _currentlevel)
        {
            _currentlevel = res;
            _backgroundColor.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.02f);
        }
    }

    private void ShowGameUI() => _gameUI.DOFade(1f, 2f);
    private void ShowGameOverUI()
    {
        _gameOverUI.gameObject.SetActive(true);
        _gameOverUI.DOFade(1f, 2f);
        _finalScoreText.transform.DOScale(0.5f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void SavePlayerScore()
    {
        PlayerPrefs.SetInt(NameSave, _bestScore);
        PlayerPrefs.Save();
    }

    private void LoadPlayerScore()
    {
        if (PlayerPrefs.HasKey(NameSave))
            _bestScore = PlayerPrefs.GetInt(NameSave);
    }
}