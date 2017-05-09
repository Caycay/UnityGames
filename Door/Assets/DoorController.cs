using UnityEngine;

namespace Assets
{
    public class DoorController : MonoBehaviour
    {
        public GameObject Door;
        public bool doorIsOpening;
        // Update is called once per frame
        void Update () {
            if (doorIsOpening)
            {
                Door.transform.Translate(Vector3.back * Time.deltaTime * 5);
            }
            if (Door.transform.position.x < 3f)
            {
                doorIsOpening = false;

            }

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Collider>().CompareTag("Player"))

            {
                doorIsOpening = true;

            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Collider>().CompareTag("Player"))
            {
                Door.transform.Translate(Vector3.forward * Time.deltaTime * 5);
                if (Door.transform.position.x > 3f)
                {
                    doorIsOpening = false;

                }

            }
        }
    }
}
