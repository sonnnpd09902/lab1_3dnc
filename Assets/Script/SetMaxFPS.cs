using UnityEngine;

public class SetMaxFPS : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 300;
    }
}
