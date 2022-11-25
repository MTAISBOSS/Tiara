using UnityEngine;

namespace AV{
    public class UI : MonoBehaviour
    {
        [SerializeField] private Nokhod_Spawner nokhodSpawner;
        public void PickedDestination( Transform id )
        {
            if ( nokhodSpawner.mainNokhod.GetComponent<Nokhod_Logic>().pickedDestination == id )
            {
                Debug.Log("True");
            }
        }
    }
}
