using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Rendering;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _playerObj;
    [SerializeField]
    private GameObject _rocket;

    private void Start()
    {
        bool _isGameLoaded = GameManager.Instance.isGameLoaded;

        if (_isGameLoaded)
            //LoadFromJson();
            Invoke("LoadFromJson", 2f);

    }

    public void SaveToJson()
    {
        SaveGame saveGame = new SaveGame();
        saveGame.position[0] = _player.transform.position.x;
        saveGame.position[1] = _player.transform.position.y;
        saveGame.position[2] = _player.transform.position.z;

        saveGame.rotation[0] = _playerObj.transform.rotation.eulerAngles.x;
        saveGame.rotation[1] = _playerObj.transform.rotation.eulerAngles.y;
        saveGame.rotation[2] = _playerObj.transform.rotation.eulerAngles.z;
        
        saveGame.playerFuel = _player.GetComponent<FuelSystem>().playerFuel;
        saveGame.rocketFuel = _rocket.GetComponentInChildren<LagerSystem>().rocketFuel;
        saveGame.playerAir = _player.GetComponent<RespiratorySystem>().lungVolume;

        saveGame.isDead = _player.GetComponent<RespiratorySystem>().isDead;
        saveGame.isWin = _rocket.GetComponent<LagerSystem>().isPlayerWin;

        string json = JsonUtility.ToJson(saveGame, true);
        File.WriteAllText(Application.dataPath + "/playerData.json", json);
    }

    public void LoadFromJson()
    {
        print("test");
        string json = File.ReadAllText(Application.dataPath + "/playerData.json");
        SaveGame saveGame = JsonUtility.FromJson<SaveGame>(json);

        #region Load Position and Rotation
        Vector3 position = Vector3.zero;
        position.x = saveGame.position[0];
        position.y = saveGame.position[1];
        position.z = saveGame.position[2];

        Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
        rotation.x = saveGame.rotation[0];
        rotation.y = saveGame.rotation[1];
        rotation.z = saveGame.rotation[2];

        _player.transform.position = position;
        _playerObj.transform.rotation = Quaternion.Euler(saveGame.rotation[0], saveGame.rotation[1], saveGame.rotation[2]);
        #endregion

        #region Load Player Variables
        _player.GetComponent<FuelSystem>().playerFuel = saveGame.playerFuel;
        _player.GetComponent<FuelSystem>().UpdatePlayerFuelSlider();
        _rocket.GetComponentInChildren<LagerSystem>().rocketFuel = saveGame.rocketFuel;
        _rocket.GetComponentInChildren<LagerSystem>().UpdateRocketFuelSlider();
        _player.GetComponent<RespiratorySystem>().lungVolume = saveGame.playerAir;
        _player.GetComponent<RespiratorySystem>().UpdateLungSlider();

        _player.GetComponent<RespiratorySystem>().isDead = saveGame.isDead;
        _rocket.GetComponent<LagerSystem>().isPlayerWin = saveGame.isWin;
        #endregion
    }
}