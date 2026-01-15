using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    private const float COLUMN_SPEED = 5f;

    private const int COLUMN_COUNT = 10;
    private const int COLUMN_SPAWN_POS_X = 10;
    private const int COLUMN_REST_POS_X = -10;
    private const int COLUMN_NEXT_SPAWN_POS_X = 5;

    private readonly Queue<Transform> columnToMovePool = new();
    private readonly Queue<Column> columnPool = new();



    [SerializeField] private Transform columnPref;


    private void Awake()
    {
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            Transform obstacl = Instantiate(columnPref);

            obstacl.position = new(COLUMN_SPAWN_POS_X, 0, 0);
            obstacl.gameObject.SetActive(false);

            Column column = obstacl.GetComponent<Column>();
            columnPool.Enqueue(column);
        }
    }

    private void Start()
    {
        CreatingObstacle(0, 4f);
        Debug.Log("columnPool " + columnPool.Count);

    }
    private void Update()
    {

        for (int i = 0; i < columnToMovePool.Count; i++)
        {
            float speed = Time.deltaTime * COLUMN_SPEED;

            Transform obj = columnToMovePool.Dequeue();
            Column column = obj.GetComponent<Column>();

            Vector3 startPosition = new(COLUMN_SPAWN_POS_X, 0, 0);
            Vector3 resetPoint = new(COLUMN_REST_POS_X, obj.position.y, obj.position.z);

            obj.position = Vector3.MoveTowards(obj.position, resetPoint, speed);


            if ((obj.position.x <= COLUMN_NEXT_SPAWN_POS_X) && column.spawnNextObstacle)
            {
                CreatingObstacle(0, 4f);
                column.spawnNextObstacle = false;
            }

            if (obj.position.x == resetPoint.x)
            {
                obj.gameObject.SetActive(false);
                obj.position = startPosition;
                column.spawnNextObstacle = true;
                columnPool.Enqueue(column);
                CreatingObstacle(0, 4f);
            }
            else
            {
                columnToMovePool.Enqueue(obj.transform);

            }

            Debug.Log("columnToMovePool " + columnToMovePool.Count);
            Debug.Log("columnPool " + columnPool.Count);
        }

    }

    private void CreatingObstacle(float gapY, float gapSize)
    {
        if (columnPool.Count >= 2)
        {
            float halfGap = gapSize * 0.5f;
            Column upColumn = columnPool.Dequeue();
            upColumn.SetColumnParameters(true, new(COLUMN_SPAWN_POS_X, gapY + halfGap, 0));

            Column dawnColumn = columnPool.Dequeue();
            dawnColumn.SetColumnParameters(false, new(COLUMN_SPAWN_POS_X, gapY - halfGap, 0));

            columnToMovePool.Enqueue(upColumn.transform);
            columnToMovePool.Enqueue(dawnColumn.transform);
        }

    }

    //private Transform SetColumnParameters(bool inAir, Vector3 position)
    //{
    //    Transform column = columnPool.Dequeue();

    //    column.gameObject.SetActive(true);
    //    column.transform.position = position;

    //    //Getting all Sprite Renderer components with children of the column and then we set the first component in the list as the column head and the second as its body
    //    SpriteRenderer[] columnSpriteRenderer = column.GetComponentsInChildren<SpriteRenderer>();
    //    SpriteRenderer headColumn = columnSpriteRenderer[0];
    //    SpriteRenderer bodyColumn = columnSpriteRenderer[1];

    //    //Setting length and orientation column
    //    float height = inAir ? HALF_SIZE_SCREEN - position.y : position.y + HALF_SIZE_SCREEN;
    //    bodyColumn.size = new(COLUMN_WIDTH, height);
    //    bodyColumn.flipY = inAir;
    //    headColumn.flipY = inAir;

    //    //Getting collider column and set his length and offset
    //    BoxCollider2D columnCollider2D = column.GetComponent<BoxCollider2D>();

    //    float colliderOffset = height * 0.5f;
    //    colliderOffset = inAir ? colliderOffset : -colliderOffset;
    //    columnCollider2D.offset = new(0, colliderOffset);

    //    columnCollider2D.size = new(COLUMN_WIDTH, height);

    //    //Return redy Column
    //    return column;

    //}

}



