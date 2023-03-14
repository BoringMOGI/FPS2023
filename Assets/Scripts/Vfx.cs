using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Vfx : MonoBehaviour
{
    ParticleSystem particle;

    // 원본 그대로 재생.
    public void Play()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();
    }

    // 스케일의 비율을 키워서 재생.
    public void Play(float scale)   // 배율.
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
