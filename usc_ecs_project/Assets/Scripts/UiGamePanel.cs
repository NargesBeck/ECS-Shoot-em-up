using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiGamePanel : MonoBehaviour
{
    private static UiGamePanel instance;
    [SerializeField] private Text fpsCounterText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text entityCount;
    [SerializeField] private Button resetButton;
    [SerializeField] private Image levelSlider;
    [SerializeField] private Text levelText;

    private string fpsCounterFormat;
    private string timeFormat;
    private string entityCountFormat;
    
    private float frameCount = 0;
    private float dt = 0.0f;
    private float fps = 0.0f;

    private float targetSliderAmount = 0;

    private void Start()
    {
        instance = this;
        fpsCounterFormat = fpsCounterText.text;
        timeFormat = timeText.text;
        entityCountFormat = entityCount.text;
        
        resetButton.onClick.AddListener(() => 
        { 
            SceneManager.LoadScene(sceneBuildIndex: 0); 
        });
    }

    private void Update()
    {
        CalculateFPS();
        fpsCounterText.text = string.Format(fpsCounterFormat, fps.ToString("f1"));
        timeText.text = string.Format(timeFormat, Time.deltaTime);
        entityCount.text = string.Format(entityCountFormat, Game.Instance.EnemyCount);
        UpdateLevelBar();
    }

    private void CalculateFPS()
    {
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 0.5f)
        {
            fps = frameCount / dt;
            fpsCounterText.text = string.Format(fpsCounterFormat, fps.ToString("f1"));
            fpsCounterText.color = (fps >= 30) ? Color.green : ((fps < 10) ? Color.red : Color.yellow);
            dt = frameCount = 0;
        }
    }

    public static void UpdateLevelBar(int playerLevel, float levelProgress)
    {
        if (instance != null)
        {
            instance.targetSliderAmount = levelProgress;
            if (levelProgress > 0)
                instance.levelSlider.fillAmount = Mathf.Lerp(instance.levelSlider.fillAmount , levelProgress, 0.1f);
            else
                instance.levelSlider.fillAmount =  levelProgress;
            instance.levelText.text = $"Level {playerLevel}";
        }
    }

    private static void UpdateLevelBar()
    {
        if (instance != null)
        {
            instance.levelSlider.fillAmount = Mathf.Lerp(instance.levelSlider.fillAmount , instance.targetSliderAmount, 0.1f);
        }
    }
}