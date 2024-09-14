using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRotationOffset : MonoBehaviour
{
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        StartCoroutine(Delay());
    }


    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1f));
        _animator.enabled = true;
    }
}
