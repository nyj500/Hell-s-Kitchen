using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightController : MonoBehaviour
{
    public Color customAmbientColor = Color.black;
    public Cubemap customReflection;
    public Light directionalLight; // 기본 조명 
    public Light spotLight; // 비상등 

    void Update()
    {
        // 정전되는 조건
        if (/* 임의의 조건 */ Input.GetKeyDown(KeyCode.C))
        {
            SetCustomLighting();
        }
        // 레버를 당겼을 때
        if (/* 임의의 조건 */ Input.GetKeyDown(KeyCode.V))
        {
            SetSkyboxLighting();
        }
    }

    // 정전일 때, Environment Lighting Source를 Color로, Reflection Source를 Custom으로 설정
    void SetCustomLighting()
    {
        directionalLight.enabled = false;
        spotLight.enabled = true;

        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = customAmbientColor; // 커스텀 컬러 설정

        // Reflection Source를 Custom으로 변경
        RenderSettings.customReflection = customReflection; // 커스텀 반사 소스 설정
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom; // Custom으로 설정

        Debug.Log("Environment Lighting: Color, Reflection: Custom");
    }

    // 정전이 아닐 때, Environment Lighting Source와 Reflection Source를 다시 Skybox로 전환
    void SetSkyboxLighting()
    {
        directionalLight.enabled = true;
        spotLight.enabled = false;

        RenderSettings.ambientMode = AmbientMode.Skybox;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

        Debug.Log("Environment Lighting: Skybox, Reflection: Skybox");
    }
}
