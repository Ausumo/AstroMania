using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _playerObj;
    [SerializeField]
    private GameObject _playerCamera;
    [SerializeField]
    private GameObject _rocket;
    [SerializeField]
    private GameObject[] _stoneCollection = new GameObject[10]; //All Stones

    private void Awake()
    {
        if(GameManager.Instance != null)
        {
            if (GameManager.Instance.isGameLoaded)
                Invoke("LoadFromJson", 0.01f);
        }
    }

    public void SaveToJson()
    {
        SaveGame saveGame = new SaveGame();
        saveGame.playerPosition = _player.transform.position;
        saveGame.playerRotation= _playerObj.transform.rotation;

        saveGame.cameraPosition = _playerCamera.transform.position;
        saveGame.CameraAxisX = _playerCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value;
        saveGame.CameraAxisY = _playerCamera.GetComponent<CinemachineFreeLook>().m_YAxis.Value;

        saveGame.playerFuel = _player.GetComponent<FuelSystem>().playerFuel;
        saveGame.rocketFuel = _rocket.GetComponentInChildren<LagerSystem>().rocketFuel;
        saveGame.playerAir = _player.GetComponent<RespiratorySystem>().lungVolume;

        saveGame.isDead = _player.GetComponent<RespiratorySystem>().isDead;
        saveGame.isWin = _rocket.GetComponent<LagerSystem>().isPlayerWin;

        for(int i = 0; i < _stoneCollection.Length; i++)
        {
            _stoneCollection[i].SetActive(saveGame.stoneCollection[i]);
        }

        string json = JsonUtility.ToJson(saveGame, true);
        File.WriteAllText(Application.dataPath + "/playerData.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/playerData.json");
        SaveGame saveGame = JsonUtility.FromJson<SaveGame>(json);

        #region Load Position and Rotation
        _player.transform.position = saveGame.playerPosition;
        _playerObj.transform.rotation = saveGame.playerRotation;

        _playerCamera.transform.position = saveGame.cameraPosition;
        _playerCamera.GetComponent<CinemachineFreeLook>().m_XAxis.Value = saveGame.CameraAxisX;
        _playerCamera.GetComponent<CinemachineFreeLook>().m_YAxis.Value = saveGame.CameraAxisY;
        #endregion

        #region Load Player and Rocket Variables
        _player.GetComponent<FuelSystem>().playerFuel = saveGame.playerFuel;
        _player.GetComponent<FuelSystem>().UpdatePlayerFuelSlider();

        _rocket.GetComponentInChildren<LagerSystem>().rocketFuel = saveGame.rocketFuel;
        _rocket.GetComponentInChildren<LagerSystem>().UpdateRocketFuelSlider();

        _player.GetComponent<RespiratorySystem>().lungVolume = saveGame.playerAir;
        _player.GetComponent<RespiratorySystem>().UpdateLungSlider();

        _player.GetComponent<RespiratorySystem>().isDead = saveGame.isDead;
        _rocket.GetComponent<LagerSystem>().isPlayerWin = saveGame.isWin;
        #endregion

        #region Load Stones
        for (int i = 0; i < _stoneCollection.Length; i++)
        {
            _stoneCollection[i].SetActive(saveGame.stoneCollection[i]);
        }
        #endregion
    }
}