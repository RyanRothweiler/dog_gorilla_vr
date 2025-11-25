using UnityEngine;

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
