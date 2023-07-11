using UnityEngine;
using UnityEngine.UI;

public class UiConfig : MonoBehaviour
{
    [SerializeField] private Text enemyCountText;
    [SerializeField] private Text enemySpeedText;
    [SerializeField] private Text waveIntervalText;
    [SerializeField] private Text playerSpeedText;
    [SerializeField] private Slider enemySpeedSlider;
    [SerializeField] private Slider enemyCountSlider;
    [SerializeField] private Slider waveIntervalSlider;
    [SerializeField] private Slider playerSpeedSlider;
    [SerializeField] private Toggle ecsToggle;
    [SerializeField] private Button playButton;

    private string enemySpeedFormat;
    private string enemyCountFormat;
    private string waveIntervalFormat;
    private string playerSpeedFormat;

    private int enemyCount;
    private float waveInterval;
    private float enemySpeed;
    private float playerSpeed;

    private void Start()
    {
        enemyCountFormat = enemyCountText.text;
        enemySpeedFormat = enemySpeedText.text;
        waveIntervalFormat = waveIntervalText.text;
        playerSpeedFormat = playerSpeedText.text;

        enemySpeedSlider.onValueChanged.AddListener(OnChangeEnemySpeed);
        enemyCountSlider.onValueChanged.AddListener(OnChangeEnemyCount);
        waveIntervalSlider.onValueChanged.AddListener(OnChangeWaveInterval);
        playerSpeedSlider.onValueChanged.AddListener(OnChangePlayerSpeed);

        ecsToggle.isOn = false;
        ecsToggle.onValueChanged.AddListener(active => { Game.ECSActive = active; });

        enemyCount = Game.Instance.enemySpawner.SpawnCount;
        enemyCountText.text = string.Format(enemyCountFormat, enemyCount);
        enemySpeed = Game.Instance.enemySpawner.Speed;
        enemySpeedText.text = string.Format(enemySpeedFormat, enemySpeed);
        waveInterval = Game.Instance.enemySpawner.SpawnInterval;
        waveIntervalText.text = string.Format(waveIntervalFormat, waveInterval);
        playerSpeed = Game.Instance.PlayerSpeed;
        playerSpeedText.text = string.Format(playerSpeedFormat, playerSpeed);

        playButton.onClick.AddListener(StartGame);
    }

    private void OnChangePlayerSpeed(float value)
    {
        playerSpeed = (int) (value * 20) + 1;
        playerSpeedText.text = string.Format(playerSpeedFormat, playerSpeed);
    }

    private void OnChangeEnemyCount(float value)
    {
        enemyCount = (int) (value * 1000);
        enemyCountText.text = string.Format(enemyCountFormat, enemyCount);
    }

    private void OnChangeEnemySpeed(float value)
    {
        enemySpeed = (int) (value * 30);
        enemySpeedText.text = string.Format(enemySpeedFormat, enemySpeed);
    }

    private void OnChangeWaveInterval(float value)
    {
        waveInterval = (int) (value * 100) + .5f;
        waveIntervalText.text = string.Format(waveIntervalFormat, waveInterval);
    }

    private void StartGame()
    {
        Game.Instance.enemySpawner.SpawnCount = enemyCount;
        Game.Instance.enemySpawner.SpawnInterval = waveInterval;
        Game.Instance.enemySpawner.Speed = enemySpeed;
        Game.Instance.PlayerSpeed = playerSpeed;
        Game.Instance.StartGame();
        gameObject.SetActive(false);
    }
}