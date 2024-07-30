using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : MonoBehaviour
{
    [Header("Transform Properties")]
    public Vector2 sizeAdjustmentMultiplierRange;
    public Vector2 sizeByDistanceRange;
    public Vector2 distanceRange;
    public AnimationCurve sizeByDistanceCurve;

    [Header("Masks")] 
    public Transform[] masks;

    
    [ContextMenu("Face Towards World Origin")]
    void FaceTowardsCentre()
    {
        transform.forward = -transform.position;
    }
    
    [ContextMenu("Randomize Size - Slightly")]
    void RandomizeSizeSlightly()
    {
        transform.localScale *= Random.Range(sizeAdjustmentMultiplierRange.x, sizeAdjustmentMultiplierRange.y);
    }
    
    [ContextMenu("Set Size Based On Distance")]
    void SetSizeBasedOnDistance()
    {
        float distance = Vector3.Distance(transform.position, Vector3.zero);
        distance = Mathf.Clamp(distance, distanceRange.x, distanceRange.y);
        distance -= distanceRange.x;
        float size = Mathf.Lerp(sizeByDistanceRange.x, sizeByDistanceRange.y,
            sizeByDistanceCurve.Evaluate(distance / (distanceRange.y - distanceRange.x)));
        transform.localScale = Vector3.one * size;
    }

    [ContextMenu("Randomize Mask")]
    void ChooseRandomMask()
    {
        if (masks.Length > 0)
        {
            int mask = Random.Range(0, masks.Length);

            for (int i = 0; i < masks.Length; i++)
            {
                masks[i].gameObject.SetActive(i == mask ? true : false);
            }
        }
    }

    void Awake()
    {
        GetComponent<Animator>().SetFloat("CycleOffset", Random.Range(0f, 1f));
    }
    
    

}
