using Assets.New_Folder.Basis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.ObjectGeneration
{
    class PowerUpGenerator:Generator
    {
        private int powerUpNumber = 4;
        public PowerUpGenerator(int[,] matrix, int rows, int columns)
        {
            Matrix = matrix;
            width = rows;
            length = columns;
        }

        private void GetRandomPowerUp(int number, int x, int z)
        {
            GameObject obj = new GameObject();
            switch (number)
            {
                case 0:
                    obj = Instantiate(EnvironmentTools.GetBombUp(), new Vector3(x, 0, z), Quaternion.identity);
                    break;
                case 1:
                    obj = Instantiate(EnvironmentTools.GetFlameUp(), new Vector3(x, 0, z), Quaternion.identity);
                    break;
                case 2:
                    obj = Instantiate(EnvironmentTools.GetSpeedUp(), new Vector3(x, 0, z), Quaternion.identity);
                    break;
                case 3:
                    obj = Instantiate(EnvironmentTools.GetWallPass(), new Vector3(x, 0, z), Quaternion.identity);
                    break;
            }
            NetworkServer.Spawn(obj);
        }
        protected override void GetFreeCoordinates(out int x, out int z)
        {
            do
            {
                x = Random.Range(1, width - 1);
                z = Random.Range(1, length - 1);
            } while (!IsBreakWall(x,z));
        }
        private bool IsBreakWall(int x, int z)
        {
            return Matrix[x, z] == 2;
        }
        public void SetPowerUps()
        {
            for(int i=0; i < powerUpNumber; i++)
            {
                int x = 0;
                int z = 0;
                GetFreeCoordinates(out x, out z);
                GetRandomPowerUp(Random.Range(0, 4), x, z);
                Matrix[x, z] = 3;
            }
        }
        public void SetExit()
        {
            int x = 0;
            int z = 0;
            GetFreeCoordinates(out x, out z);
            GameObject obj = Instantiate(EnvironmentTools.GetExit(), new Vector3(x, -0.5f, z), Quaternion.identity);
            Matrix[x, z] = 3;
            NetworkServer.Spawn(obj);
        }
    }
}
