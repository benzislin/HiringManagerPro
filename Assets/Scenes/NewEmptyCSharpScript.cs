using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    public float speed = 2f;
    public float height = 10f;
    
    Vector3 startPos;

    void Start() => startPos = transform.localPosition;

    void Update()
    {
        // Creates a smooth up-and-down motion using a Sine wave
        float newY = Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
