using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIExtensions
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

    private static MonoBehaviour _coroutineHelper;

    public static void UiOpen { get; private set; }

    public static void Open(this CanvasGroup cg)
    {
        UiOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        CoroutineHelper.Instance.StartCoroutine(FadeUi(cg));
    }

    public static void Close(this CanvasGroup cg, bool hideCursor = false)
    {
        if (hideCursor)
        {
            UiOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
}
