using UnityEngine;
using UnityEngine.InputSystem;

public class EchoData : MonoBehaviour
{
    #region Properties
     public int id;
    public float value;
    #endregion

    #region Public Methods
        public void Initialize(int newId, float newValue)
    {
        id = newId;
        value = newValue;

        Debug.Log(" ID: " + id + " // Valor: " + value);
    }
    #endregion
   
   
}
