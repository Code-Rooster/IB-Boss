using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    public float elapsedTime = 0;

    public IEnumerator MoveToPos(GameObject objToMove, Vector3 moveTo, float speed)
    {
        Vector3 startPos = objToMove.transform.position;

        while (elapsedTime < speed && Vector3.Distance(objToMove.transform.position, moveTo) > 0.1)
        {
            elapsedTime += Time.deltaTime;

            objToMove.transform.position = Vector3.Lerp(startPos, moveTo, (elapsedTime / speed));

            yield return null;
        }

        objToMove.transform.position = moveTo;
    }
}
