using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    private const int HALF_SIZE_SCREEN = 5;

    private const float COLUMN_WIDTH = 0.83f;
    private const float COLUMN_SPEED = 5f;
    private const int COLUMN_COUNT = 6;
    private const int COLUMN_SPAWN_POS_X = 10;
    private const int NEXT_COLUMN_SPAWN_POS_X = 5;
    private const int COLUMN_REST_POS_X = -10;

    [SerializeField] private Transform columnPref;
    private List<Transform> obstacle = new();
    private Queue<Transform> columnPool = new();
    private Vector3 startPoint = new(10, 0, 0);
    private Vector3 resetPoint = new(-10, 0, 0);



    private void Awake()
    {
        for (int i = 0; i < COLUMN_COUNT; i++)
        {
            Transform column = Instantiate(columnPref);
            column.transform.position = new(COLUMN_SPAWN_POS_X, 0, 0);
            column.gameObject.SetActive(false);
            columnPool.Enqueue(column);

        }
    }
    private void Start()
    {
        CreatingObstacle(0, 4f);

    }
    private void Update()
    {
        if (obstacle.Count > 0)
        {
            List<Transform> objToMove = obstacle;


            float speed = Time.deltaTime * COLUMN_SPEED;

            foreach (Transform obj in objToMove)
            {
                startPoint = obj.position;
                resetPoint = new(COLUMN_REST_POS_X, obj.position.y, obj.position.z);

                obj.position = Vector3.MoveTowards(startPoint, resetPoint, speed);

                if (obj.position == resetPoint)
                {
                    obj.gameObject.SetActive(false);
                }

            }

        }

        if (obstacle[1].position.x == NEXT_COLUMN_SPAWN_POS_X)
        {
            CreatingObstacle(0, 4f);
        }
        else if (obstacle[1].position.x == COLUMN_REST_POS_X)
        {
            obstacle.Clear();
            CreatingObstacle(0, 4f);
        }

    }

    private void CreatingObstacle(float gapY, float gapSize)
    {
        float halfGap = gapSize * 0.5f;
        Transform upColumn = SetColumnParameters(true, new(COLUMN_SPAWN_POS_X, gapY + halfGap, 0));
        Transform dawnColumn = SetColumnParameters(false, new(COLUMN_SPAWN_POS_X, gapY - halfGap, 0));

        obstacle.Add(upColumn);
        obstacle.Add(dawnColumn);

    }

    private Transform SetColumnParameters(bool inAir, Vector3 position)
    {
        Transform column = columnPool.Dequeue();
        column.gameObject.SetActive(true);
        column.transform.position = position;

        //Get all Sprite Renderer components with children of the column and then we set the first component in the list as the column head and the second as its body
        SpriteRenderer[] columnSpriteRenderer = column.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer headColumn = columnSpriteRenderer[0];
        SpriteRenderer bodyColumn = columnSpriteRenderer[1];

        //Setting length and orientation column
        float height = inAir ? HALF_SIZE_SCREEN - position.y : position.y + HALF_SIZE_SCREEN;
        bodyColumn.size = new(COLUMN_WIDTH, height);
        bodyColumn.flipY = inAir;
        headColumn.flipY = inAir;

        columnPool.Enqueue(column);
        return column;
    }

}
