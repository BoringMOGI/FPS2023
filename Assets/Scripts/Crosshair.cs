using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private static Crosshair instance;
    public static Crosshair Instance => instance;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image crosshairImage;

    [Header("Color")]
    [SerializeField] Color colorOn;
    [SerializeField] Color colorOff;
    [SerializeField] float changeColorTime;

    private bool isTargetOn;        // 타겟을 노리고 있는지.
    private Vector3 scaleOn;        // 타겟을 노리고 있을때의 스케일(크기)
    private Vector3 scaleOff;       // 평상시 스케일(크기)

    private Color diffColor;        // 색상 차이 값.
    private float colorTime;        // 색상 타임.

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isTargetOn = false;
        scaleOn = new Vector3(0.7f, 0.7f, 0.7f);
        scaleOff = new Vector3(1f, 1f, 1f);

        diffColor = colorOff - colorOn;
        colorTime = changeColorTime;

        Switch(false);
    }

    private void Update()
    {
        // 현재 상태에 따라 색상을 n초에 걸쳐 변경해야한다.
        float deltaTime = Time.deltaTime * (isTargetOn ? -1f : 1f);             // On일 경우(-) Off일 경우(+)
        colorTime = Mathf.Clamp(colorTime + deltaTime, 0f, changeColorTime);    // colorTime을 계산.

        float ratio = colorTime / changeColorTime;                              // 색상 비율.
        crosshairImage.color = colorOn + (diffColor * ratio);                   // 색상 비율에 따라 최소 색상 + (차이 비율)
    }

    public void Switch(bool isOn)
    {
        // 상태 변화.
        isTargetOn = isOn;
        rectTransform.localScale = isTargetOn ? scaleOn : scaleOff;
    }
}
