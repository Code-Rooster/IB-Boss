using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator ShakeCam(float magnitude, float iterations, float roughness)
    {
        List<Vector3> states = new List<Vector3>();

        for (int i = 0; i < iterations; i++)
        {
            states.Add(new Vector3(magnitude * Random.Range(-1, 1), magnitude * Random.Range(-1, 1), magnitude * Random.Range(-500, 500)));
        }

        for (int i = 0; i < iterations; i++)
        {
            float elapsedTime = 0;

            while (elapsedTime < roughness)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(states[i].x, states[i].y, 0), Time.deltaTime * elapsedTime / roughness);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, 0, states[i].z), Time.deltaTime * elapsedTime / roughness);

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
