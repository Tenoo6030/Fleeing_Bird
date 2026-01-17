using UnityEngine;

public class Column : MonoBehaviour
{
    private const float COLUMN_WIDTH = 0.83f;
    private const int HALF_SIZE_SCREEN = 5;

    public bool spawnNextObstacle = true;


    public Transform SetColumnParameters(bool inAir, Vector3 position)
    {
        spawnNextObstacle = inAir;
        gameObject.SetActive(true);
        transform.position = position;

        //Getting all Sprite Renderer components with children of the column and then we set the first component in the list as the column head and the second as its body
        SpriteRenderer[] columnSpriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer headColumn = columnSpriteRenderer[0];
        SpriteRenderer bodyColumn = columnSpriteRenderer[1];

        //Setting length and orientation column
        float height = inAir ? HALF_SIZE_SCREEN - position.y : position.y + HALF_SIZE_SCREEN;
        bodyColumn.size = new(COLUMN_WIDTH, height);
        bodyColumn.flipY = inAir;
        headColumn.flipY = inAir;

        //Getting collider column and set his length and offset
        BoxCollider2D columnCollider2D = GetComponent<BoxCollider2D>();

        float colliderOffset = height * 0.5f;
        colliderOffset = inAir ? colliderOffset : -colliderOffset;
        columnCollider2D.offset = new(0, colliderOffset);
        columnCollider2D.size = new(COLUMN_WIDTH, height);

        //Return redy Column
        return transform;

    }


}
