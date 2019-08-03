using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHeightController : MonoBehaviour
{
    const float yIntersect = -50f;

    void LateUpdate()
    {
        Vector3 position = transform.position;
        position.y = yIntersect - transform.position.z;
        transform.position = position;
    }
    
}
