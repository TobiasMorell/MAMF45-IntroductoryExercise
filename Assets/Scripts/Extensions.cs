using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public static class Extensions
{
    private class CoroutineHelper : MonoBehaviour
    {
        public static CoroutineHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject();
                    _instance = go.AddComponent<CoroutineHelper>();
                }
                return _instance;
            }
        }

        private static CoroutineHelper _instance;
    }

    public static void Open(this CanvasGroup cg)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().DisableMovement();
        
        cg.interactable = true;
        cg.blocksRaycasts = true;
        CoroutineHelper.Instance.StartCoroutine(FadeUi(cg));
    }

    public static void Close(this CanvasGroup cg, bool hideCursor = false)
    {
        if (hideCursor)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().EnableMovemnet();
        }
        cg.interactable = false;
        cg.blocksRaycasts = false;
        CoroutineHelper.Instance.StartCoroutine(FadeUi(cg));
    }

    private static IEnumerator FadeUi(CanvasGroup cg, float speed = 0.2f)
    {
        var start = cg.alpha;
        var increasing = start == 0;

        while ((increasing && start < 1) || (!increasing && start > 0))
        {
            if (increasing)
                start += speed;
            else
                start -= speed;
            cg.alpha = start;
            yield return null;
        }
    }

    public static Color GetColor(this SelectionMode sm)
    {
        switch (sm)
        {
            case SelectionMode.Move:
                return Color.blue;
            case SelectionMode.Delete:
                return Color.red;
            case SelectionMode.Inspect:
                return Color.yellow;
            case SelectionMode.None:
                return Color.white;
            default:
                throw new ArgumentOutOfRangeException("sm", sm, null);
        }
    }
}
