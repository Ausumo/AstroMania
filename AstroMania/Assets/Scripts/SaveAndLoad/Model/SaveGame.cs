using UnityEngine;

[System.Serializable]
public class SaveGame
{

    //Player
    public Vector3 playerPosition; 
    public Quaternion playerRotation;

    //Camera
    public Vector3 cameraPosition;
    public float CameraAxisX;
    public float CameraAxisY;

    public int playerFuel; //Fuel das der Spieler bei sich trägt
    public int rocketFuel; //Fuel das in der Rocket steckt
    public int playerAir; //Die luft die der Spieler hat

    public bool isDead; //Ob der Spieler Tot ist
    public bool isWin; //Ob der Spieler das Game fertig hat

    public bool[] stoneCollection = new bool[10]; //All Stones
}
