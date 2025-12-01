using UnityEngine;

/// <summary>
/// Hold F to speed up time (Editor only).
/// </summary>
public class DebugTools : MonoBehaviour
{
    void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.F))
            {
                Time.timeScale = 20.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
}
