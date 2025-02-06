using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab;

    public float xMin = -10f, xMax = 10f;
    public float yMin = -10f, yMax = 10f;
    public float zMin = -10f, zMax = 10f;

    private GameObject cubeClone; // Đối tượng Cube được tạo ra

    void Start()
    {
        StartCoroutine(CreateCube());
    }

    IEnumerator CreateCube()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(3);
            // Tạo Cube tại vị trí ngẫu nhiên
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);
            float randomZ = Random.Range(zMin, zMax);

            cubeClone = Instantiate(cubePrefab, new Vector3(randomX, randomY, randomZ), Quaternion.identity);
            StartCoroutine(MoveCube(cubeClone));

            
        }
    }

    IEnumerator MoveCube(GameObject cube)
    {
        float elapsedTime = 0f;
        float moveDuration = 3f; // Thời gian di chuyển về gốc
        Vector3 startPosition = cube.transform.position;

        while (elapsedTime < moveDuration)
        {
            cube.transform.position = Vector3.Lerp(startPosition, Vector3.zero, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cube.transform.position = Vector3.zero; // Đảm bảo Cube dừng tại gốc
    }

    IEnumerator FadeCube(GameObject cube)
    {
        // Đảm bảo Cube có Renderer
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        if (cubeRenderer == null)
        {
            Debug.LogError("Cube không có Renderer!");
            yield break;
        }

        // Đảm bảo vật liệu Cube ở chế độ Transparent
        Material cubeMaterial = cubeRenderer.material;
        SetMaterialToTransparent(cubeMaterial);

        float duration = 5f; // Thời gian làm mờ
        float elapsedTime = 0f;
        Color initialColor = cubeMaterial.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Tính toán giá trị alpha mới
            float newAlpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / duration);

            // Cập nhật alpha của vật liệu
            cubeMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, newAlpha);

            yield return null; // Đợi frame tiếp theo
        }

        // Đảm bảo alpha bằng 0 khi kết thúc
        cubeMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Hủy Cube sau khi làm mờ
        Destroy(cube);
    }

    void SetMaterialToTransparent(Material material)
    {
        material.SetFloat("_Mode", 3); // Transparent
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000; // Transparent queue
    }

    void Update()
    {
        // Khi nhấn phím Space, làm mờ CubeClone nếu nó tồn tại
        if (Input.GetKeyDown(KeyCode.Space) && cubeClone != null)
        {
            StartCoroutine(FadeCube(cubeClone));
        }
    }
}
