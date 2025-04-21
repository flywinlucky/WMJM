using Sirenix.OdinInspector;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using WatermelonGameClone;

public class MoveAxe : MonoBehaviour
{
    [Header("Debug")]
    [Space]
    public Sphere moveSpherePosition;

    [Header("Referinces")]
    [Space]
    public EventTrigger eventTrigger;
    [Space]
    public Transform spawnPoint;
    public Transform leftAnchor;
    public Transform rightAnchor;

    public float boundsSafeArea;
    public float outputBoundsSafeArea;
    [Space]
    public GameObject trajectoryLine;

    // Define variabilele pentru mișcare între ancore
    public float moveSpeed;
    private bool isMovingRight = true;

    private Subject<Unit> dropItemSubject = new Subject<Unit>();
    public IObservable<Unit> OnDropItem => dropItemSubject;

    private void Start()
    {
        // Adaugă un PointerDown EventTrigger la care să reacționeze funcția DropItem
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.Drag;
        pointerDownEntry.callback.AddListener((data) => { UpdatePosition(); });
        eventTrigger.triggers.Add(pointerDownEntry);

        // Adaugă un PointerUp EventTrigger la care să reacționeze funcția DragImage
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { DropItem(); });
        eventTrigger.triggers.Add(pointerUpEntry);

        // Adaugă un PointerClick EventTrigger la care să reacționeze funcția DragImage
        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerDown;
        pointerClickEntry.callback.AddListener((data) => { UpdatePosition(); });
        eventTrigger.triggers.Add(pointerClickEntry);
    }

    public void GetCurrentSphereScale(Sphere currentSphere)
    {
        moveSpherePosition = currentSphere;
        boundsSafeArea = currentSphere.transform.localScale.x;
        outputBoundsSafeArea = boundsSafeArea / 2 + 0.2f;
    }

    [Button]
    public void UpdatePosition()
    {
        //Debug.Log("Update Sphere Position");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float offset = 0.01f; // Poate fi ajustat la nevoie

        // Calculăm direcția și mișcarea
        Vector3 direction = isMovingRight ? rightAnchor.position - leftAnchor.position : leftAnchor.position - rightAnchor.position;
        Vector3 movement = direction.normalized * moveSpeed * Time.deltaTime;

        // Actualizăm poziția spawnPoint
        Vector3 newPosition = new Vector3(mousePos.x, spawnPoint.position.y, spawnPoint.position.z);
        newPosition += movement * offset;

        // Limităm poziția la limitele leftAnchor și rightAnchor
        float clampedX = Mathf.Clamp(newPosition.x, leftAnchor.position.x + outputBoundsSafeArea, rightAnchor.position.x - outputBoundsSafeArea); // Adăugați sau scădeți boundsSafeArea la limitele x
        spawnPoint.position = new Vector3(clampedX, newPosition.y, newPosition.z);

        // Verificăm dacă spawnPoint a atins una dintre ancore și inversăm direcția
        if (Mathf.Approximately(clampedX, leftAnchor.position.x + outputBoundsSafeArea) || Mathf.Approximately(clampedX, rightAnchor.position.x - outputBoundsSafeArea))
        {
            isMovingRight = !isMovingRight;
        }
    }

    private void DropItem()
    {
        //Debug.Log("Drop Item");
        // Adăugați aici orice alte acțiuni pe care doriți să le efectuați atunci când se eliberează butonul de mouse

        dropItemSubject.OnNext(Unit.Default); // Emit evenimentul
    }
}