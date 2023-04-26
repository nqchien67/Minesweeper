using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class DragHandler : MonoBehaviour
    {
        public float maxDistance;

        private Vector3 offset;
        private Tilemap tilemap;
        private Vector3 positionToMiddle;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        void OnMouseDown()
        {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        void OnMouseDrag()
        {
            Vector3 screenMiddle = GetScreenMiddlePosition();
            Vector3 tilemapMiddle = transform.position + tilemap.localBounds.center;

            //positionToMiddle = tilemapMiddle - transform.position;
            //Vector3 direction = tilemapMiddle - screenMiddle;
            //if (direction.magnitude > maxDistance)
            //{
            //    tilemapMiddle = screenMiddle + direction.normalized * (maxDistance - 0.1f);
            //    transform.position = tilemapMiddle - positionToMiddle;
            //    return;
            //}

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            transform.position = curPosition;
        }

        private Vector3 GetScreenMiddlePosition()
        {
            float screenMiddleX = Screen.width / 2f;
            float screenMiddleY = Screen.height / 2f;
            Vector3 screenMiddle = Camera.main.ScreenToWorldPoint(new Vector3(screenMiddleX, screenMiddleY));
            screenMiddle.z = 0;

            return screenMiddle;
        }
    }
}