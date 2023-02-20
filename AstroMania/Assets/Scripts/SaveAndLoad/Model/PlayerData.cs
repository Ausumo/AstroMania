using System.Numerics;

[System.Serializable]
public class SaveGame
{
    public float[] position = new float[3]; //Position des Spielers
    public float[] rotation = new float[4]; //Rotation des Spielers

    public int playerFuel; //Fuel das der Spieler bei sich trägt
    public int rocketFuel; //Fuel das in der Rocket steckt
    public int playerAir; //Die luft die der Spieler hat

    public bool isDead; //Ob der Spieler Tot ist
    public bool isWin; //Ob der Spieler das Game fertig hat
}
