using Enumerations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float speed = 5.0f;

    void FixedUpdate()
    {
        if (Game.State == GAME_STATE.IN_GAME)
        {
            var target = Game.Scene.player.transform.position + offset;
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, target, Time.deltaTime * speed);
        }
    }
}
