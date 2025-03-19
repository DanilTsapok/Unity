using UnityEngine;
using UnityEngine.UI;
using Unity.RemoteConfig;
using Unity.Services.RemoteConfig;
using TMPro;
using Unity.Services.Core;
using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class RemoteConfigsManger : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    public TextMeshProUGUI menuText;
    public SpriteRenderer spriteRenderer; 
    public Image imageComponent;

    private string textColor;
    private string backgroundColor;

    async void Start()
    {
        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            await InitializeUnityServices();
        }


        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;

        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
        
    }
    private async Task InitializeUnityServices()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn) { 
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        }
    }

    private void ApplyRemoteConfig(ConfigResponse response)
    {
        Debug.Log($"Remote Config Response: {response.status}");

        if (response.status == ConfigRequestStatus.Success)
        {
            textColor = RemoteConfigService.Instance.appConfig.GetString("textColor");
            backgroundColor = RemoteConfigService.Instance.appConfig.GetString("backgroundColor");

            Debug.Log($"Отримання кольору: textColor = {textColor}, backgroundColor = {backgroundColor}");

            ChangeTextColor();
            ChangeBackgroundColor();
        }
        else
        {
            Debug.LogError("Не вдалось отримати колір");
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
            Debug.LogError("Не правильний колір тексту");
        }
    }

    private void ChangeBackgroundColor()
    {
        if (ColorUtility.TryParseHtmlString(backgroundColor, out Color color))
        {

            if (spriteRenderer != null)  
            {
                spriteRenderer.color = color;
            }


            if (imageComponent != null)
            {
                imageComponent.sprite = null; 
                imageComponent.color = color;
            }
        }
        else
        {
            Debug.LogError("Не правильний колір фону");
        }
    }
}