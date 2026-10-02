using UnityEngine;

// Input.GetTouch example.
//
// Attach to an origin based cube.
// A screen touch moves the cube on an iPhone or iPad.
// A second screen touch reduces the cube size.

public class Controls : MonoBehaviour
{
  private Vector3 position;
  public GridSlotPreviewManager gridSlotPreviewManager;
  public static bool pause;

  void Awake()
  {
    pause = false;
  }

  void Update()
  {
    if (!pause)
    {
      if (Input.touchCount > 0)
      {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
          Debug.Log("Doing something");
          Vector2 pos = touch.position;

          Ray ray = Camera.main.ScreenPointToRay(pos);
          Debug.Log("Doing something");
          var hit = Physics2D.Raycast(ray.origin, ray.direction);
          if (hit.collider != null && hit.collider.gameObject.GetComponent<GridSlot>() is var gridSlot && gridSlot != null)
          {
            if (gridSlot.IsEmpty)
            {
              gridSlot.Fill();
            }

          }

          Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);
        }

        if (Input.touchCount == 2)
        {
          touch = Input.GetTouch(1);

          if (touch.phase == TouchPhase.Began)
          {
          }

          if (touch.phase == TouchPhase.Ended)
          {
          }
        }
      }
    }
    // Handle screen touches.

  }
}
