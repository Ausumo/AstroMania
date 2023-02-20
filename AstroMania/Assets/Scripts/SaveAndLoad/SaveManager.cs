using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    private void Start()
    {
        bool _isGameLoaded = GameManager.Instance.isGameLoaded;

        if (_isGameLoaded)
            LoadFromJson();

    }

    public void SaveToJson()
    {
        SaveGame saveGame = new SaveGame();
        saveGame.position[0] = _player.transform.position.x;
        saveGame.position[1] = _player.transform.position.y;
        saveGame.position[2] = _player.transform.position.z;

        saveGame.rotation[0] = _player.transform.rotation.eulerAngles.x;
        saveGame.rotation[1] = _player.transform.rotation.eulerAngles.y;
        saveGame.rotation[2] = _player.transform.rotation.eulerAngles.z;
        
        saveGame.playerFuel = _player.GetComponent<FuelSystem>().playerFuel;
        saveGame.playerAir = _player.GetComponent<RespiratorySystem>().lungVolume;

        saveGame.isDead = _player.GetComponent<RespiratorySystem>().isDead;

        string json = JsonUtility.ToJson(saveGame, true);
        File.WriteAllText(Application.dataPath + "/playerData.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/playerData.json");
        SaveGame playerData = JsonUtility.FromJson<SaveGame>(json);

        #region Load Position and Rotation
        Vector3 position = Vector3.zero;
        position.x = playerData.position[0];
        position.y = playerData.position[1];
        position.z = playerData.position[2];

        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        rotation.x = playerData.rotation[0];
        rotation.y = playerData.rotation[1];
        rotation.z = playerData.rotation[2];

        _player.transform.position = position;
        _player.transform.rotation = Quaternion.Euler(playerData.rotation[0], playerData.rotation[1], playerData.rotation[2]);
        #endregion

        #region Load Player Variables
        _player.GetComponent<FuelSystem>().playerFuel = playerData.playerFuel;
        _player.GetComponent<RespiratorySystem>().lungVolume = playerData.playerAir;
        _player.GetComponent<RespiratorySystem>().isDead = playerData.isDead;
        #endregion
    }
}