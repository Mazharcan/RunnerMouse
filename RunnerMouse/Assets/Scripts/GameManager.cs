using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Road = new List<GameObject>(); // Yollarý tutacaðýmýz liste
    [SerializeField] private Transform playerPrefab;                 // Oyuncu prefab'ý (karakter)

    // Spawn noktalarý (arabalarýn ve peynirlerin spawn edileceði noktalar)
    [SerializeField] Transform carSPawn;
    [SerializeField] Transform cheeseSPawn;

    float previousPlayerZ; // Oyuncunun önceki z ekseni pozisyonu

    private float roadLenght = 3.2f; // Yol uzunluðu (yeni yol prefab'ýnýn oluþturulma mesafesi)

    // Ýlk baþta oluþturulacak yol sayýsý
    int count = 8;

    public CharacterController characterController; // Karakterin kontrolünü saðlayan script

    private void Start()
    {
        Time.timeScale = 1; // Oyunu baþlat
        // Ýlk yolu oluþtur
        Instantiate(Road[0], transform.position, transform.rotation);

        // Ýlk baþta rastgele 8 yol prefab'ý üretilecek
        for (int i = 0; i < count; i++)
        {
            CreateRoad();
        }
    }

    private void Update()
    {
        // Eðer oyuncunun z pozisyonu, yollarýn uzunluðundan fazla ise yeni bir yol oluþtur
        if (playerPrefab.position.z > roadLenght - 3.2f * count)
        {
            CreateRoad();
        }
    }

    private void FixedUpdate()
    {
        // Oyuncunun z pozisyonundaki deðiþim
        float deltaZ = playerPrefab.position.z - previousPlayerZ;

        // CarSpawner ve CheeseSpawner'ýn pozisyonlarýný z ekseninde oyuncunun hareketine göre güncelle
        carSPawn.position += new Vector3(0, 0, deltaZ); // Arabalarýn spawn noktasý
        cheeseSPawn.position += new Vector3(0, 0, deltaZ); // Peynirlerin spawn noktasý

        // Bir sonraki kare için önceki z pozisyonunu güncelle
        previousPlayerZ = playerPrefab.position.z;
    }

    void CreateRoad()
    {
        // Rastgele bir yol prefab'ý oluþtur ve onu yol uzunluðuna göre yerleþtir
        Instantiate(Road[Random.Range(0, Road.Count)], transform.forward * roadLenght, transform.rotation);
        roadLenght += 3.2f; // Yeni yolun uzunluðunu arttýr
    }

    // Oyun yeniden baþlatýldýðýnda sahneyi tekrar yükle
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Ana menüye dönmek için sahneyi deðiþtir
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Oyuna devam etmek için zamaný tekrar baþlat
    public void ContinueGame()
    {
        Time.timeScale = 1; // Zamaný baþlat
        characterController.musicSource.Play(); // Müziði tekrar çal
        characterController.stopScreen.SetActive(false); // Durma ekranýný gizle
    }
}
