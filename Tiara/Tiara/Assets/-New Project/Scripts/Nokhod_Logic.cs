using UnityEngine;
using System.Collections;
using System;

namespace AV{
    public class Nokhod_Logic : MonoBehaviour
    {
        [SerializeField] private float fadeOutTimer;
        [SerializeField] private Vector3 destinationPoint;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private float amplitude;
        [SerializeField] private float frequency = 3;
        private SpriteRenderer spriteRenderer;
        private Color color;
        private bool inDestination = false;
        private bool faded = false;
        private bool coroutinEnded = false;
        private int groupindex = 0;
        public bool lastRound = false;
        public bool mainNokhod;
        public Transform pickedDestination;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            GetRandomColor();
            StartCoroutine (Logic());
        }
        private void Update()
        {
            if (coroutinEnded)
            {
                if ( groupindex < Destinations.groups.Length)
                {
                    Destinations.groups[groupindex++].gameObject.SetActive(true);
                }
                if (groupindex + 1 >= Destinations.groups.Length)
                {
                    lastRound = true;
                } 
                if (Destinations.destinations.Count > 0)
                {
                    StopCoroutine(Logic());
                    StartCoroutine(Logic());
                    coroutinEnded = false;
                }
            }
        }
        private IEnumerator Logic()
        {
            GetDestination();

            while (!inDestination)
            {
                // Use one of them
                MoveLerp();
                //MoveSin();

                yield return null;
            }
            while (!faded)
            {
                FadeOut();
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            while (faded && !lastRound)
            {
                FadeIn();
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            coroutinEnded = true;
            
        }

        private void FadeIn()
        {
            faded = true;
            color.a += 0.01f;
            spriteRenderer.color = color;
            if (color.a == 1)
            {
                faded = false;
            }
        }

        private void FadeOut()
        {
            faded = false;
            color.a -= 0.01f;
            spriteRenderer.color = color;
            if (color.a <0.01) 
            {
                faded = true;
            }
        }
        private void MoveSin()
        {
            inDestination = false;
            amplitude = destinationPoint.y;
            float x = transform.position.x;
            float y = Mathf.Sin(Time.time * frequency) * amplitude;
            float z = 0;
            Vector3 pos = new Vector3(x, y, z);
            transform.position = Vector2.MoveTowards(pos, destinationPoint, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, destinationPoint) < 0.1)
            {
                transform.position = destinationPoint;
                inDestination = true;
            }
        }

        private void MoveLerp()
        {
            inDestination = false;
            Vector2 pos = transform.position;
            float offset_x = destinationPoint.x - pos.x;
            transform.position = Vector3.Slerp(pos, destinationPoint , Time.deltaTime * moveSpeed);

            if (Vector2.Distance(transform.position, destinationPoint) < 0.1)
            {
                transform.position = destinationPoint;
                inDestination = true;
            }
        }
        private Vector3 GetDestination()
        {
            inDestination = false;
            int pickedDestinationIndex = UnityEngine.Random.Range(0, Destinations.destinations.Count);
            Transform pickedDestinationTransform = Destinations.destinations[pickedDestinationIndex];
            if (lastRound && mainNokhod)
                pickedDestination = pickedDestinationTransform;
            Vector3 pickedDestinationPoint = pickedDestinationTransform.position;
            destinationPoint = pickedDestinationPoint;
            Destinations.destinations.Remove(pickedDestinationTransform);
            return pickedDestinationPoint;
        }

        private void GetRandomColor()
        {
            Color newColor = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f) , 1f);
            color = newColor;
            spriteRenderer.color = newColor;
        }
    }
}
