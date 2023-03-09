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

    const float MIN_DISTANCE = 20f;     // �������� �ּ� �Ÿ�.
    const float MAX_DISTANCE = 30f;     // �������� �ִ� �Ÿ�.

    public void Setup(IHpBar target)
    {
        this.target = target;
    }
    private void Update()
    {
        fillImage.fillAmount = target.Hp / target.MaxHp;

        // ü�¹��� ����
        //  - �� ��ġ���� ī�޶� �ٶ󺸴� ������ �����ϸ� ü�¹ٰ� �Ųٷ� ���ư���.
        //  - ���� ī�޶󿡼� �� ��ġ�� ���� �������� ����ؾ��Ѵ�.
        Vector3 direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);


        // ī�޶���� �Ÿ��� ���� Ȱ��ȭ/��Ȱ��ȭ

        // D(�ִ�Ÿ� - ����Ÿ�) / (�ִ� - �ּ�) => Alpha�� ���� ��.
        // ���� ī�޶��� �Ÿ��� ����������� D�� ���� Ŀ����.
        // ���� ī�޶��� �Ÿ��� �־������� D�� ���� �۾�����.
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        float offset = (MAX_DISTANCE - distance) / (MAX_DISTANCE - MIN_DISTANCE);

        group.alpha = Mathf.Clamp(offset, 0f, 1f);
    }
}
