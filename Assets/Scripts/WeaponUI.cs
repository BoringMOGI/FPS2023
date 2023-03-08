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

    // const    : ���ȭ. ���ܳ��� ������ ������ �ܰ�.
    // readonly : ���ȭ. ���ܳ��� ������ ��Ÿ�� ����.
    readonly Vector3 SIZE_UP = new Vector3(0.3f, 0.3f, 0.3f);

    Coroutine update;
    float sizeTime;

    public void SwitchSelected(bool isSelected)
    {
        hotkeyObject.SetActive(!isSelected);    // ������ �Ǿ��ٸ� ����Ű�� ����.

        // ������ ������ �ڷ�ƾ�� �ִٸ� ����.
        if (update != null)
            StopCoroutine(update);

        update = StartCoroutine(IEUpdate(isSelected));
    }
    private IEnumerator IEUpdate(bool isSelected)
    {
        // ���� ���� ���õǾ��ٸ�? (ũ�Ⱑ ������ �Ѵٸ�...)
        if(isSelected)
        {
            // sizeTime�� �ִ� Ÿ�Ӻ��� ���� ���
            while(sizeTime < changeSizeTime)
            {
                // sizeTime�� �ð��� �帧���� ���Ѵ�.
                // ���� 1�� �� ������� SIZE_UP * ������ŭ�� Ű���.
                sizeTime = Mathf.Clamp(sizeTime + Time.deltaTime, 0f, changeSizeTime);
                rectTransform.localScale = new Vector3(1f, 1f, 1f) + (SIZE_UP * (sizeTime / changeSizeTime));

                yield return null;
            }
        }
        else
        {
            // sizeTime�� 0���� Ŭ ���
            while (sizeTime > 0f)
            {
                // sizeTime�� �ð��� �帧���� ���Ѵ�.
                // ���� 1�� �� ������� SIZE_UP * ������ŭ�� Ű���.
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

        // ����. �������� �� ���� �����̴�.
        Color color = (ratio <= 0.2f) ? new Color(.4f, .1f, .1f) : Color.black;
        color.a = backImage.color.a;
        backImage.color = color;
    }

}
