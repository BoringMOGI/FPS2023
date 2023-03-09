using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IHpBar
{
    public float Hp { get; }
    public float MaxHp { get; }
}
public class HpBarUI : MonoBehaviour
{
    [SerializeField] CanvasGroup group;
    [SerializeField] Image fillImage;

    IHpBar target;

    const float MIN_DISTANCE = 20f;     // 보여지는 최소 거리.
    const float MAX_DISTANCE = 30f;     // 보여지는 최대 거리.

    public void Setup(IHpBar target)
    {
        this.target = target;
    }
    private void Update()
    {
        fillImage.fillAmount = target.Hp / target.MaxHp;

        // 체력바의 최전
        //  - 내 위치에서 카메라를 바라보는 방향을 대입하면 체력바가 거꾸로 돌아간다.
        //  - 따라서 카메라에서 내 위치를 보는 방향으로 계산해야한다.
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);


        // 카메라와의 거리에 따라서 활성화/비활성화

        // D(최대거리 - 현재거리) / (최대 - 최소) => Alpha의 비율 값.
        // 나와 카메라의 거리가 가까워질수록 D의 값이 커진다.
        // 나와 카메라의 거리가 멀어질수록 D의 값이 작아진다.
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float offset = (MAX_DISTANCE - distance) / (MAX_DISTANCE - MIN_DISTANCE);

        group.alpha = Mathf.Clamp(offset, 0f, 1f);
    }
}
