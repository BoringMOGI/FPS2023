using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    public void OnHit(float power);
}

public class Enemy : MonoBehaviour, IHit, IHpBar
{
    [SerializeField] HpBarUI hpUI;
    [SerializeField] float maxHp;

    private float hp;

    public float MaxHp => maxHp;
    public float Hp => hp;

    void Start()
    {
        hp = maxHp;
        hpUI.Setup(this);
    }

    public void OnHit(float power)
    {
        hp = Mathf.Clamp(hp - power, 0f, maxHp);
        if(hp <= 0)
        {
            Destroy(gameObject);
        }    
    }
}
