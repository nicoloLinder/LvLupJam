using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Dome : MonoBehaviour
{
    private static Dome _instance;

    [SerializeField] private Animator animator;

    public static Dome Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Dome>();
                if (_instance == null)
                {
                    throw new Exception();
                }
            }
            return _instance;
        }
    }

    public void DomeHit()
    {
        animator.SetTrigger("Hit");
    }

    public void DomeStart()
    {
        animator.SetTrigger("StartLoop");
    }

}