using UnityEngine;
using System.Collections;

public class NGUIHelper : MonoBehaviour
{
    public static void RemoveClickEventListener(GameObject gameObject, UIEventListener.VoidDelegate listener)
    {
        if (gameObject == null)
        {
            return;
        }

        UIEventListener.Get(gameObject).onClick -= listener;
    }
    public static void RemovePressEventListener(GameObject gameObject, UIEventListener.BoolDelegate listener)
    {
        if (gameObject == null)
        {
            return;
        }
    
        UIEventListener.Get(gameObject).onPress -= listener;
    }
}
