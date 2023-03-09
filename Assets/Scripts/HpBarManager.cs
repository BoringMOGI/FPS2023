using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour
{
    private static HpBarManager instance;
    public static HpBarManager Instance => instance;

    [SerializeField] HpBarUI prefab;

    private void Awake()
    {
        instance = this; 
    }
    public void CreateHpUI(IHpBar target)
    {
        HpBarUI ui = Instantiate(prefab, transform);
        ui.Setup(target);
    }
}
