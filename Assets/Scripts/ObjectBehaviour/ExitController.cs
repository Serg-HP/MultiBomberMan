using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.ObjectBehaviour
{
    class ExitController: MonoBehaviour
    {
        private Collider collider;
        private GameObject[] playerCount;
        private Text resultText;
        private void Start()
        {
            resultText = GameObject.Find("ResultText").GetComponent<Text>();
            collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;
        }
        private void Update()
        {
            playerCount = GameObject.FindGameObjectsWithTag("Player");
            if (playerCount.Length == 1)
            {
                collider.enabled = true;
            }
            else
                collider.enabled = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player")
            {
                resultText.text = "Victory!!!";
                Destroy(gameObject);
                Application.Quit();
            }
        }
    }
}
