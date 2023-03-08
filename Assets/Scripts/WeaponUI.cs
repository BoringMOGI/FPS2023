using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] GameObject hotkeyObject;
    [SerializeField] Image fillImage;
    [SerializeField] Image backImage;
    [SerializeField] float changeSizeTime;

    // const    : 상수화. 생겨나는 시점이 컴파일 단계.
    // readonly : 상수화. 생겨나는 시점이 런타임 간계.
    readonly Vector3 SIZE_UP = new Vector3(0.3f, 0.3f, 0.3f);

    Coroutine update;
    float sizeTime;

    public void SwitchSelected(bool isSelected)
    {
        hotkeyObject.SetActive(!isSelected);    // 선택이 되었다면 단축키를 끈다.

        // 기존에 실행한 코루틴이 있다면 중지.
        if (update != null)
            StopCoroutine(update);

        update = StartCoroutine(IEUpdate(isSelected));
    }
    private IEnumerator IEUpdate(bool isSelected)
    {
        // 만약 내가 선택되었다면? (크기가 거져야 한다면...)
        if(isSelected)
        {
            // sizeTime이 최대 타임보다 적을 경우
            while(sizeTime < changeSizeTime)
            {
                // sizeTime을 시간의 흐름으로 더한다.
                // 이후 1의 원 사이즈에서 SIZE_UP * 비율만큼을 키운다.
                sizeTime = Mathf.Clamp(sizeTime + Time.deltaTime, 0f, changeSizeTime);
                rectTransform.localScale = new Vector3(1f, 1f, 1f) + (SIZE_UP * (sizeTime / changeSizeTime));

                yield return null;
            }
        }
        else
        {
            // sizeTime이 0보다 클 경우
            while (sizeTime > 0f)
            {
                // sizeTime을 시간의 흐름으로 더한다.
                // 이후 1의 원 사이즈에서 SIZE_UP * 비율만큼을 키운다.
                sizeTime = Mathf.Clamp(sizeTime - Time.deltaTime, 0f, changeSizeTime);
                rectTransform.localScale = new Vector3(1f, 1f, 1f) + (SIZE_UP * (sizeTime / changeSizeTime));

                yield return null;
            }
        }
    }

    public void UpdateFill(float amount, float max)
    {
        float ratio = amount / max;
        fillImage.fillAmount = ratio;

        // 색상. 에너지를 다 쓰면 빨강이다.
        Color color = (ratio <= 0.2f) ? new Color(.4f, .1f, .1f) : Color.black;
        color.a = backImage.color.a;
        backImage.color = color;
    }

}
