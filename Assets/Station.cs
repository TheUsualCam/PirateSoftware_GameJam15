using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    [Tooltip("How base time (seconds) it takes for this station to complete.")]
    [SerializeField] private float baseDuration = 0.5f;
}
