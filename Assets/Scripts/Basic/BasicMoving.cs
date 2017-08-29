using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Basic
{
    abstract public class BasicMoving: NetworkBehaviour
    {
        public abstract void Move(Vector3 direction, int rotation);
        public void MoveLeft()
        {
            Move(Vector3.left, 270);
        }
        public void MoveRight()
        {
            Move(Vector3.right, 90);
        }
        public void MoveUp()
        {
            Move(Vector3.forward, 0);
        }
        public void MoveDown()
        {
            Move(Vector3.back, 180);
        }
    }
}
