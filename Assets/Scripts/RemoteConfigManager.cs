using UnityEngine;
using UnityEngine.UI;
using Unity.RemoteConfig;
using Unity.Services.RemoteConfig;
using TMPro;

public class RemoteConfigsManger : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    public TextMeshProUGUI menuText;
    public SpriteRenderer spriteRenderer; 
    public Image imageComponent;

    private string textColor;
    private string backgroundColor;

    void Start()
    {
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void ApplyRemoteConfig(ConfigResponse response)
    {
        if (response.status == ConfigRequestStatus.Success)
        {
            textColor = RemoteConfigService.Instance.appConfig.GetString("textColor", "red");
            backgroundColor = RemoteConfigService.Instance.appConfig.GetString("backgroundColor", "green");

            if (string.IsNullOrEmpty(textColor) || string.IsNullOrEmpty(backgroundColor))
            {
                Debug.LogWarning("Одно из значений конфигурации пустое.");
            }

            Debug.Log($"Text Color: {textColor}");
            Debug.Log($"Background Color: {backgroundColor}");

            ChangeTextColor();
            ChangeBackgroundColor();
        }
        else
        {
            Debug.LogError("Не удалось загрузить конфигурацию.");
        }
    }

    private void ChangeTextColor()
    {
        if (ColorUtility.TryParseHtmlString(textColor, out Color color))
        {
            menuText.color = color;
        }
        else
        {
            Debug.LogError("Неверный цвет текста.");
        }
    }

    private void ChangeBackgroundColor()
    {
        if (ColorUtility.TryParseHtmlString(backgroundColor, out Color color))
        {
            spriteRenderer.color = color;  

          
            if (imageComponent != null)
            {
                imageComponent.sprite = null; 
                imageComponent.color = color;
            }
        }
        else
        {
            Debug.LogError("Неверный цвет фона.");
        }
    }
}