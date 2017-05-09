using UnityEngine;

namespace Assets
{
    public class threshold : MonoBehaviour {

        public GameObject Door;
        public bool doorIsOpening;
        // Update is called once per frame
        void Update()
        {
            if (!doorIsOpening)
            {
                Door.transform.Translate(Vector3.forward * Time.deltaTime * 5);
            }
            if (Door.transform.position.y > 3f)
            {
                doorIsOpening = true;

            }

        }

        void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Collider>().CompareTag("Player"))

            {
                doorIsOpening = false;

            }


        }
    }
}

