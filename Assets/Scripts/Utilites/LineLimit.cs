using TMPro;
using UnityEngine;

public class LineLimit : MonoBehaviour
{
    private TMP_InputField _mainInputField;
   
       void Start()
       {
           _mainInputField = GetComponent<TMP_InputField>();
           //Changes the character limit in the main input field.
           _mainInputField.characterLimit = 15;
       }
}
