using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event UnityAction PlayerDetected;

    public static void OnPlayerDetected() => PlayerDetected?.Invoke();
    
}
