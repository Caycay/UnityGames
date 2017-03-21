using UnityEngine;

namespace Assets.Prefabs.Scripts_1
{
    public class SpawnManager : MonoBehaviour
    {
        public int maxPlatforms = 20;
        public GameObject platform;
        public float horizontalMin = 7.5f;
        public float horizontalMax = 14f;
        public float verticalMin = -6f;
        public float vericalMax = 6f;

        private Vector2 originPosition;

        // Use this for initialization
        void Start ()
        {
            originPosition = transform.position;
            Spawn();
        }

        void Spawn()
        {
            for (int i = 0; i < maxPlatforms; i++)
            {
                Vector2 randomPosition = originPosition +
                                         (new Vector2(Random.Range(horizontalMin, horizontalMax),
                                             Random.Range(verticalMin, vericalMax)));
                Instantiate(platform, randomPosition, Quaternion.identity);
                originPosition = randomPosition;
            }
        }
	}
}
