using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Vfx : MonoBehaviour
{
    ParticleSystem particle;

    // ���� �״�� ���.
    public void Play()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();
    }

    // �������� ������ Ű���� ���.
    public void Play(float scale)   // ����.
    {
        for(int i = 0; i< transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.localScale *= scale;
        }

        transform.localScale *= scale;
        Play();
    }
}
