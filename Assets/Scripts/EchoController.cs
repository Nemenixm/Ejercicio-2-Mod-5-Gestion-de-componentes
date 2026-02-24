using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections.Generic;

public class EchoManager : MonoBehaviour
{
    #region Properties
    [Header("Prefab")]
    public GameObject ecoPrefab;
    [System.Serializable]
    public class EchoSaveData
    {
        public int[] ids;
        public float[] values;
    }

    
    #endregion

    #region  Fields
    private List<EchoData> ecosActivos = new List<EchoData>();
    private int currentID = 0;
     private string rutaGuardado;
    #endregion

    #region Unity Callbacks
    void Awake()
    {
        rutaGuardado = Application.persistentDataPath + "/echoSave.json";
        LoadEcos();
    }


    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CreateEcho();
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            SaveEcos();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetEcos();
        }
    }
    #endregion
    #region Public Methods
    void CreateEcho()
    {
        GameObject newEco = Instantiate(ecoPrefab, Random.insideUnitSphere * 3f, Quaternion.identity);

        EchoData data = newEco.GetComponent<EchoData>();

        float randomValue = Random.Range(0f, 1f);

        data.Initialize(currentID, randomValue);

        ecosActivos.Add(data);

        currentID++;
    }

    void SaveEcos()
    {
        EchoSaveData saveData = new EchoSaveData();

        EchoData[] ecoArray = ecosActivos.ToArray();

        saveData.ids = new int[ecoArray.Length];
        saveData.values = new float[ecoArray.Length];

        for (int i = 0; i < ecoArray.Length; i++)
        {
            saveData.ids[i] = ecoArray[i].id;
            saveData.values[i] = ecoArray[i].value;
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(rutaGuardado, json);

        Debug.Log("Se ha gurdado en: " + rutaGuardado);
    }

    void LoadEcos()
    {
        if (!File.Exists(rutaGuardado))
        {
            Debug.Log("Sin datos guardados.");
            return;
        }

        string json = File.ReadAllText(rutaGuardado);
        EchoSaveData saveData = JsonUtility.FromJson<EchoSaveData>(json);

        for (int i = 0; i < saveData.ids.Length; i++)
        {
            GameObject newEco = Instantiate(ecoPrefab, Random.insideUnitSphere * 3f, Quaternion.identity);

            EchoData data = newEco.GetComponent<EchoData>();
            data.Initialize(saveData.ids[i], saveData.values[i]);

            ecosActivos.Add(data);
        }

        currentID = saveData.ids.Length;

        Debug.Log("Datos cargados.");
    }

    void ResetEcos()
    {
        foreach (EchoData eco in ecosActivos)
        {
            Destroy(eco.gameObject);
        }

        ecosActivos.Clear();
        currentID = 0;

        if (File.Exists(rutaGuardado))
        {
            File.Delete(rutaGuardado);
        }

        Debug.Log("Se ha reiniciado las esferas.");
    }
}

    #endregion
    
   

