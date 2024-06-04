using UnityEngine;

public class MazeColliderGenerator : MonoBehaviour
{
    public Texture2D mazeTexture;  // Assign the black and white maze image here
    public GameObject wallPrefab;  // Assign a wall prefab with a BoxCollider2D

    public float wallSize = 1.0f;  // Size of each wall piece

    void Start()
    {
        GenerateCollidersFromImage();
    }

    void GenerateCollidersFromImage()
    {
        if (mazeTexture == null)
        {
            Debug.LogError("Maze texture is not assigned!");
            return;
        }

        for (int y = 1; y < mazeTexture.height - 1; y++)
        {
            for (int x = 1; x < mazeTexture.width - 1; x++)
            {
                Color pixelColor = mazeTexture.GetPixel(x, y);
                Color lColor = mazeTexture.GetPixel(x-1, y);
                Color rColor = mazeTexture.GetPixel(x+1, y);
                Color uColor = mazeTexture.GetPixel(x, y-1);
                Color dColor = mazeTexture.GetPixel(x, y+1);

                if (pixelColor == Color.black && (lColor == Color.white || rColor == Color.white || uColor == Color.white || dColor == Color.white))
                {
                    Vector2 position = new Vector2(x * wallSize, y * wallSize);
                    Instantiate(wallPrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
