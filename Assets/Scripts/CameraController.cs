using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCam;

    public float zOffset = -75f;

    int panSpeedMulti = 4;
    public float panSpeed = 5f;
    public float scrollSpeed = 20f;

    public float minZ = -50f;
    public float maxZ = -100f;

    public Vector2 panLimitMax;
    public Vector2 panLimitMin;

    private void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            pos.y += (panSpeed * panSpeedMulti) * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            pos.y -= (panSpeed * panSpeedMulti) * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += (panSpeed * panSpeedMulti) * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= (panSpeed * panSpeedMulti) * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.z += scroll * scrollSpeed * Time.deltaTime;

        //pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.x = Mathf.Clamp(pos.x, panLimitMin.x, panLimitMax.x);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        pos.y = Mathf.Clamp(pos.y, panLimitMin.y, panLimitMax.y);

        if (pos != transform.position)
        {
            transform.position = pos;
        }
    }

    public void SnapToPosition(float x, float y)
    {
        mainCam.transform.position = new Vector3(x, y, zOffset);
    }

    public void SetupCameraBounds(float mapWidth, float mapHeight)
    {
        float boarderOffsetX = mapWidth / 5;
        float boarderOffsetY = mapHeight / 10;

        panLimitMax.x = mapWidth - boarderOffsetX;
        panLimitMin.x = boarderOffsetX;

        panLimitMax.y = mapHeight - boarderOffsetY;
        panLimitMin.y = 45f;//boarderOffsetY;

        SnapToPosition(mapWidth / 2, mapHeight / 2);
    }
}
