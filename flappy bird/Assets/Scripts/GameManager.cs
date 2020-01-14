using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    static GameManager _instance;
    public bool gameStarted;
    public bool playerDead;

    private float _timer = 3f;
    public GameObject pipeline;

    private int _currentScore, _bestScore;

    private Text _cScore;
    private Text _bScore;
    private Text _sScore;
    private GameObject _blink;
    private GameObject _scoreBoard;
    private GameObject _floor;
    private Button _restartButton;

    public static GameManager Instance => _instance;

    void Awake()
    {
        _instance = this;
    }

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            _cScore.text = _currentScore.ToString();
        }
    }

    void InitGame()
    {
        gameStarted = true;
        _cScore.enabled = true;
        _blink.SetActive(false);
        _scoreBoard.SetActive(false);
    }

    public void EndGame()
    {
        playerDead = true;
        gameStarted = false;
        _floor.GetComponent<Animator>().speed = 0f;
        StartCoroutine(PlayBlinkAnimation());
        SetScores();
    }

    private void SetScores()
    {
        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
        }

        _sScore.text = _currentScore.ToString();
        _bScore.text = _bestScore.ToString();
    }
    
    IEnumerator PlayBlinkAnimation()
    {
        _blink.SetActive(true);
        while (_blink.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        _scoreBoard.SetActive(true);
        _blink.SetActive(false);
    }
    
    void Start()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        Transform scoreBoard = canvas.transform.Find("ScoreBoard").transform;
        _cScore = canvas.Find("CurrentScore").gameObject.GetComponent<Text>();
        _bScore = scoreBoard.Find("Best").GetComponent<Text>();
        _sScore = scoreBoard.Find("Score").GetComponent<Text>();
        _blink = canvas.Find("Blink").gameObject;
        _scoreBoard = canvas.Find("ScoreBoard").gameObject;
        _floor = GameObject.Find("Floor");
        _restartButton = scoreBoard.Find("RestartButton").GetComponent<Button>();
        _restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (!gameStarted && !playerDead && Input.GetMouseButtonDown(0))
        {
            InitGame();
        }

        if (gameStarted)
        {
            CreatePipeline();
        }
    }

    void CreatePipeline()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Instantiate(pipeline, new Vector3(5, Random.Range(-2f, 3f)), Quaternion.identity);
            _timer = 1.8f;
        }
    }

    void RestartGame()
    {
        playerDead = false;
        gameStarted = false;
        SceneManager.LoadScene("Scenes/Main");
    }
}
