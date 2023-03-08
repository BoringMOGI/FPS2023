using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHit
{
    public void OnHit(int power);
}

public class Enemy : MonoBehaviour, IHit
{
    [SerializeField] float maxHp;

    float hp;

    void Start()
    {
        hp = maxHp;
    }

    public void OnHit(int power)
    {

    }
}
