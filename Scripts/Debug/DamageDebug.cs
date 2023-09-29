using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Battle
{

    public class DamageDebug : MonoBehaviour
    {
        public GameObject enemy1Prefab;
        public GameObject enemy2Prefab;
        public Transform _spawn1Pos;
        public Transform _spawn2Pos;

        private Enemy enemy1;
        private Enemy enemy2;

        private void Start()
        {
            enemy1 = Instantiate(enemy1Prefab, _spawn1Pos.position, _spawn1Pos.rotation).GetComponent<Enemy>();
            enemy2 = Instantiate(enemy2Prefab, _spawn2Pos.position, _spawn2Pos.rotation).GetComponent<Enemy>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z) && enemy1.IsAlive)
            {
                var damageValue = Random.Range(5, 20);
                var damage = new Game.Damage(damageValue, this.gameObject, false);
                enemy1.TakeDamage(damage);
                enemy2.TakeDamage(damage);

                if (!enemy1.IsAlive)
                {
                    Invoke("Respown", 3f);
                }

            }
        }

        private void Respown()
        {
            Destroy(enemy1.gameObject);
            enemy1 = Instantiate(enemy1Prefab, _spawn1Pos.position, _spawn1Pos.rotation).GetComponent<Enemy>();

            Destroy(enemy2.gameObject);
            enemy2 = Instantiate(enemy2Prefab, _spawn2Pos.position, _spawn2Pos.rotation).GetComponent<Enemy>();
        }
    }

}