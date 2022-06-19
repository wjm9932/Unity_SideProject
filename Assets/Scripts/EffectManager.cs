using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
            return m_Instance;
        }
    }

    public enum EffectType
    {
        Commom,
        Flesh
    }
    public EffectType type { get; private set; }

    public ParticleSystem metalHit;
    //public ParticleSystem fleshHit;


    private void Start()
    {
        type = EffectType.Commom;
    }

    public void PlayEffect(Vector3 pos, Vector3 dir, EffectType type, Transform parent = null)
    {
        ParticleSystem target = metalHit;

        if(type == EffectType.Flesh)
        {
            
        }

        ParticleSystem effect = Instantiate(target, pos, Quaternion.LookRotation(dir));

        if (parent != null)
        {
            effect.transform.SetParent(parent);
        }

        effect.Play();
    }
}
