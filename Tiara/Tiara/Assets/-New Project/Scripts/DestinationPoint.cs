using System.Linq;
using UnityEngine;

namespace AV{
    public class DestinationPoint : MonoBehaviour
    {
        private void OnEnable()
        {
            Destinations.destinations.Add(this.transform);
        }
        private void OnDisable()
        {
            Destinations.destinations.Remove(this.transform);
        }
        private void Update()
        {
            if (!Destinations.destinations.Contains(this.transform))
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
