using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator ShakeCam(float magnitude, float iterations, float timePerCicle, float roughness)
    {
        List<Vector3> states = new List<Vector3>();

        for (int i = 0; i < iterations; i++)
        {
            states.Add(new Vector3(magnitude * (Random.Range(0, 2) * 2 - 1), magnitude * (Random.Range(0, 2) * 2 - 1), magnitude * (Random.Range(0, 2) * 2 - 1)));
        }

        for (int i = 0; i < iterations; i++)
        {
            float elapsedTime = 0;

            while (elapsedTime < timePerCicle)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(states[i].x, states[i].y, 0), Time.deltaTime * elapsedTime / timePerCicle);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, 0, states[i].z), Time.deltaTime * elapsedTime / timePerCicle);

                elapsedTime += Time.deltaTime;

                //yield return null;
                yield return new WaitForSeconds(roughness);
            }
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        while (transform.localPosition != Vector3.zero || transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            transform.localPosition = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime);

            yield return new WaitForSeconds(roughness);
        }
    }
}
