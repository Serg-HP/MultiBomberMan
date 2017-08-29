using Assets.Scripts;
using Assets.Scripts.ObjectGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MapCreator : NetworkBehaviour {

    public enum GenerationMethod
    {
        Random,
        Default
    }
    public int width;
    public int length;
    public int breakWallNumber;
    public GenerationMethod method;
    public void Start ()
    {
        CmdCreateMap();
    }
    [Command]
    private void CmdCreateMap()
    {
        MapGenerator newMap = new MapGenerator(width, length, breakWallNumber);
        newMap.GenerateConcreteWalls();
        newMap.GenerateGround();
        if (method == GenerationMethod.Random)
            newMap.RandomGenerateBreakWalls();
        else newMap.GenerateBreakWalls();
        DynamicObjectGenerator dynamicOG = new DynamicObjectGenerator(newMap.GetMatrix(), width, length);
        for (int i = 0; i < 2; i++)
            dynamicOG.CreatePlayer();
        PowerUpGenerator powerUpGen = new PowerUpGenerator(newMap.GetMatrix(), width, length);
        powerUpGen.SetPowerUps();
        powerUpGen.SetExit();
    }

}
