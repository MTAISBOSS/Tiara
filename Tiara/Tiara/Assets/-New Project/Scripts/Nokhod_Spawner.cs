using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AV{
    public class Nokhod_Spawner : MonoBehaviour
    {
        [SerializeField] private GameObject nokhodPrefab;
        [SerializeField] private int spawnAmount;
        [SerializeField] private float spawnTimer = 0.5f;
        public List<Nokhod_Logic> allNokhods = new List<Nokhod_Logic>();
        public GameObject mainNokhod;
        private void Start()
        {
            StartCoroutine( SpawnNokhod() );
        }
        private IEnumerator SpawnNokhod()
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                GameObject newNokhod = Instantiate(nokhodPrefab);
                newNokhod.transform.position = new Vector3(-8, UnityEngine.Random.Range(-4f , 4f), 0);
                allNokhods.Add( newNokhod.GetComponent<Nokhod_Logic>() );
                if (allNokhods.Count == 1)
                {
                    newNokhod.GetComponent<Nokhod_Logic>().mainNokhod = true;
                    mainNokhod = newNokhod;
                }
                yield return new WaitForSeconds(spawnTimer);
            }
        }
    }
}
