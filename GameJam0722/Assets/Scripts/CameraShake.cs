using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation, rotationMultiplayer;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) StartShake(0.2f, 0.1f, 7f);
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplayer * Time.deltaTime);
        }
        
        transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f,1f));
    }

    public void StartShake(float length, float power, float rotation)
    {
        shakeTimeRemaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
        shakeRotation = power * rotation;

        if (rotationMultiplayer != rotation) rotationMultiplayer = rotation;
    }
}