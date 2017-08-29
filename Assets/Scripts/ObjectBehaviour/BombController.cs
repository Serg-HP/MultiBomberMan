using Assets.New_Folder.Basis;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    class BombController: NetworkBehaviour
    {
        public AudioClip explosionSound;
        private int explosionLength;
        private float lifespan = 2;
        private SphereCollider collider;
        private AudioSource source;
        void Start()
        {
            collider = gameObject.GetComponent<SphereCollider>();
            source = gameObject.GetComponent<AudioSource>();
            GetBombStrength();
            Invoke("PlayExplosionSound", lifespan-0.3f);
            Invoke("CmdMakeExplosion", lifespan);
            Destroy(collider.gameObject,lifespan);
        }

        [Command]
        private void CmdMakeExplosion()
        {
            float killDelay = 0;
            List<Ray> rayList = CreateExplosionArea();
            RaycastHit hit;
            ShowExplosion(Vector3.zero,1);
            foreach (Ray ray in rayList)
            {
                if (Physics.Raycast(ray, out hit, explosionLength))
                {
                    if (IsDestroyable(hit))
                    {
                        KillDynamicObject(hit,ref killDelay);
                        Destroy(hit.collider.gameObject, killDelay);
                        ShowExplosion(ray.direction, explosionLength);
                    }
                }
                else
                    ShowExplosion(ray.direction, explosionLength);

            }
        }

        private void ShowExplosion(Vector3 direction, float explosionLength)
        {
            for (int i = 1; i <= explosionLength; i++)
            {
                var position = new Vector3(transform.position.x + direction.x*i, transform.position.y, transform.position.z + direction.z*i);
                GameObject gameobj = Instantiate(EnvironmentTools.GetExplosion(), position, Quaternion.identity);
                Destroy(gameobj, lifespan);
            }
        }

        private List<Ray> CreateExplosionArea()
        {
            List<Ray> rayList = new List<Ray>();
            rayList.Add(new Ray(transform.position, Vector3.left));
            rayList.Add(new Ray(transform.position, Vector3.right));
            rayList.Add(new Ray(transform.position, Vector3.forward));
            rayList.Add(new Ray(transform.position, Vector3.back));
            return rayList;
        }

        private bool IsDestroyable(RaycastHit hit)
        {
            return hit.collider.tag == "BreakWall" || hit.collider.tag == "Enemy" || hit.collider.tag == "Player";
        }
        private void PlayExplosionSound()
        {
            source.PlayOneShot(explosionSound);
        }

        private void KillDynamicObject(RaycastHit hit, ref float killDelay)
        {
            if (hit.collider.tag == "Player")
            {
                GameObject go = hit.collider.gameObject;
                PlayerController other = (PlayerController)go.GetComponent(typeof(PlayerController));
                other.SetDead();
                Animator animatorPlayer = hit.collider.gameObject.GetComponent<Animator>();
                animatorPlayer.SetTrigger("IsDead");
                killDelay = 2;
            }
            hit.collider.enabled = false;
        }
        private void GetBombStrength()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.1f);
            foreach(Collider hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Player")
                {
                    GameObject go = hitCollider.gameObject;
                    PlayerController other = (PlayerController)go.GetComponent(typeof(PlayerController));
                    explosionLength = other.explosionArea;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            collider.isTrigger = false;
        }

    }
}
