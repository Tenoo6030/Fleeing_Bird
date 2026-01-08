using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Const")]
    private const float COLUMN_WIDTH = 8.6f;
    private const float SPEED_COLUMN = 5f;
    private const int HALF_SIZE_SCREEN = 50;
    private const int COLUMN_SPAWN_X_POS = 100;
    private const int COLUMN_DESPAWN_X_POS = -100;
    private const int COLUMN_COUNT = 10;

    Transform obstacle;
    private Vector3 columnSpawnPoint = new(COLUMN_SPAWN_X_POS, 0, 0);
    private Vector3 columnDespawnPoint = new(COLUMN_DESPAWN_X_POS, 0, 0);

    private Queue<GameObject> columnHeadPool = new();
    private Queue<GameObject> columnBodyPool = new();

    private void Awake()
    {
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            //Creating a column head pool
            GameObject head = Instantiate(GameAssets.GetInstance.prefColumnHead);
            head.transform.position = columnSpawnPoint;
            head.SetActive(false);
            columnHeadPool.Enqueue(head);

            //Creating a column body pool
            GameObject body = Instantiate(GameAssets.GetInstance.prefColumnBody);
            body.transform.position = columnSpawnPoint;
            body.SetActive(false);
            columnBodyPool.Enqueue(body);
        }
    }

    private void Start()
    {
        CreatingObstacle(0, 40f, COLUMN_SPAWN_X_POS);

    }


    private void CreatingObstacle(float gapY, float gapSize, float xPosition)
    {
        float halfGap = gapSize * 0.5f;
        Transform upColumn = SetColumnValue(xPosition, gapY + halfGap, false);
        Transform downColumn = SetColumnValue(xPosition, gapY - halfGap, true);
        StartCoroutine(ColumnMove(upColumn, downColumn));

    }

    private Transform SetColumnValue(float xPosition, float yPosition, bool onGround)
    {
        //Positioning column head
        GameObject columnHead = columnHeadPool.Dequeue();
        columnHead.transform.position = new(xPosition, yPosition, 0);
        columnHead.transform.localScale = onGround ? Vector3.one : new(1, -1, 1);
        columnHead.SetActive(true);
        columnHeadPool.Enqueue(columnHead);

        //Positioning column body and setting his height
        GameObject columnBody = columnBodyPool.Dequeue();
        columnBody.transform.position = new(xPosition, yPosition, 0);
        SpriteRenderer colmnBodySpriteRenderer = columnBody.GetComponent<SpriteRenderer>();
        columnBody.transform.localScale = onGround ? new(1, -1, 1) : Vector3.one;
        float height = onGround ? yPosition + HALF_SIZE_SCREEN : HALF_SIZE_SCREEN - yPosition;
        colmnBodySpriteRenderer.size = new(COLUMN_WIDTH, height);
        columnBody.SetActive(true);
        columnBodyPool.Enqueue(columnBody);
        columnBody.transform.parent = columnHead.transform;

        return columnHead.transform;
    }

    private IEnumerator ColumnMove(Transform uColumn, Transform dColumn)
    {
        while (uColumn.transform.position.x != COLUMN_DESPAWN_X_POS && dColumn.transform.position.x != COLUMN_DESPAWN_X_POS)
        {
            float columnSpeed = SPEED_COLUMN * Time.deltaTime;
          // Vector3 dirMove = Vector3.Lerp(columnSpawnPoint, columnDespawnPoint, columnSpeed);
            uColumn.transform.position = Vector3.Lerp(columnSpawnPoint, columnDespawnPoint, columnSpeed);
            dColumn.transform.position = Vector3.Lerp(columnSpawnPoint, columnDespawnPoint, columnSpeed);

        }

        uColumn.transform.GetChild(0).SetParent(null);
        dColumn.transform.GetChild(0).SetParent(null);

        yield return null;
    }
}