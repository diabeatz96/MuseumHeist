using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TitleScreenTVSequence : MonoBehaviour
{
    [SerializeField]
    GameObject[] screens;
    void Start()
    {
        StartCoroutine(DoSomethingEveryTenSeconds());
        screens[0].SetActive(true);

        IEnumerator DoSomethingEveryTenSeconds()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                // Your action here
                Debug.Log("2 seconds have passed!");
                screens[0].SetActive(false);
                screens[1].SetActive(true);
            }
        }
    }
}
