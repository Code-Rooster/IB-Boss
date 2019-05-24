using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextZoomIn : MonoBehaviour
{
    TMPro.TMP_Text dialogueText;

    private void Start()
    {
        dialogueText = this.GetComponent<TMP_Text>();
    }
}
