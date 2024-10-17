using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightController : MonoBehaviour
{
    public Color customAmbientColor = Color.black;
    public Cubemap customReflection;
    public Light directionalLight; // 기본 조명 
    public Light ramp; // 비상등 
    public Light spotLight;
    private bool isBlackout = false;

    void Start()
    {
        SetSkyboxLighting();
        StartCoroutine(TriggerBlackout());
    }

    IEnumerator TriggerBlackout()
    {
        while (true)
        {
            float randomTime = Random.Range(60f, 120f);

            // 설정된 시간만큼 대기
            yield return new WaitForSeconds(randomTime);

            // 대기 후 실행할 함수 호출
            SetCustomLighting();
        }
    }
    // 정전일 때, Environment Lighting Source를 Color로, Reflection Source를 Custom으로 설정
    void SetCustomLighting()
    {
        directionalLight.enabled = false;
        ramp.enabled = true;
        spotLight.enabled = true;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = customAmbientColor; // 커스텀 컬러 설정

        // Reflection Source를 Custom으로 변경
        RenderSettings.customReflection = customReflection; // 커스텀 반사 소스 설정
        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom; // Custom으로 설정

        isBlackout = true;
    }

    // 정전이 아닐 때, Environment Lighting Source와 Reflection Source를 다시 Skybox로 전환
    public void SetSkyboxLighting()
    {
        directionalLight.enabled = true;
        ramp.enabled = false;
        spotLight.enabled = false;

        RenderSettings.ambientMode = AmbientMode.Skybox;

        RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

        isBlackout = false;
    }
}
