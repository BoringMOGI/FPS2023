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

    private bool isTargetOn;        // Ÿ���� �븮�� �ִ���.
    private Vector3 scaleOn;        // Ÿ���� �븮�� �������� ������(ũ��)
    private Vector3 scaleOff;       // ���� ������(ũ��)

    private Color diffColor;        // ���� ���� ��.
    private float colorTime;        // ���� Ÿ��.

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
        // ���� ���¿� ���� ������ n�ʿ� ���� �����ؾ��Ѵ�.
        float deltaTime = Time.deltaTime * (isTargetOn ? -1f : 1f);             // On�� ���(-) Off�� ���(+)
        colorTime = Mathf.Clamp(colorTime + deltaTime, 0f, changeColorTime);    // colorTime�� ���.

        float ratio = colorTime / changeColorTime;                              // ���� ����.
        crosshairImage.color = colorOn + (diffColor * ratio);                   // ���� ������ ���� �ּ� ���� + (���� ����)
    }

    public void Switch(bool isOn)
    {
        // ���� ��ȭ.
        isTargetOn = isOn;
        rectTransform.localScale = isTargetOn ? scaleOn : scaleOff;
    }
}
