using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float height = 10f;
    public float stiffness = 4f;

    private void Awake()
    {
        GameManager.OnLevelStart += OnLevelStart;
    }

    private void OnLevelStart()
    {
        Vector3 camPos = GameManager.Player.transform.position;
        camPos.y = height;
        transform.position = camPos;
    }

    private void LateUpdate()
    {
        if(!GameManager.HasStartedLevel) return;
        //transform.position = Vector3.MoveTowards(transform.position, GameManager.Player.transform.position + Vector3.up * height, 5f*Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, GameManager.Player.transform.position + Vector3.up * height, stiffness * TimeManager.GetTimeDelta(TimeChannel.Absolute));
    }
}
