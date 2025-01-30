using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Road = new List<GameObject>(); // Yollar� tutaca��m�z liste
    [SerializeField] private Transform playerPrefab;                 // Oyuncu prefab'� (karakter)

    // Spawn noktalar� (arabalar�n ve peynirlerin spawn edilece�i noktalar)
    [SerializeField] Transform carSPawn;
    [SerializeField] Transform cheeseSPawn;

    float previousPlayerZ; // Oyuncunun �nceki z ekseni pozisyonu

    private float roadLenght = 3.2f; // Yol uzunlu�u (yeni yol prefab'�n�n olu�turulma mesafesi)

    // �lk ba�ta olu�turulacak yol say�s�
    int count = 8;

    public CharacterController characterController; // Karakterin kontrol�n� sa�layan script

    private void Start()
    {
        Time.timeScale = 1; // Oyunu ba�lat
        // �lk yolu olu�tur
        Instantiate(Road[0], transform.position, transform.rotation);

        // �lk ba�ta rastgele 8 yol prefab'� �retilecek
        for (int i = 0; i < count; i++)
        {
            CreateRoad();
        }
    }

    private void Update()
    {
        // E�er oyuncunun z pozisyonu, yollar�n uzunlu�undan fazla ise yeni bir yol olu�tur
        if (playerPrefab.position.z > roadLenght - 3.2f * count)
        {
            CreateRoad();
        }
    }

    private void FixedUpdate()
    {
        // Oyuncunun z pozisyonundaki de�i�im
        float deltaZ = playerPrefab.position.z - previousPlayerZ;

        // CarSpawner ve CheeseSpawner'�n pozisyonlar�n� z ekseninde oyuncunun hareketine g�re g�ncelle
        carSPawn.position += new Vector3(0, 0, deltaZ); // Arabalar�n spawn noktas�
        cheeseSPawn.position += new Vector3(0, 0, deltaZ); // Peynirlerin spawn noktas�

        // Bir sonraki kare i�in �nceki z pozisyonunu g�ncelle
        previousPlayerZ = playerPrefab.position.z;
    }

    void CreateRoad()
    {
        // Rastgele bir yol prefab'� olu�tur ve onu yol uzunlu�una g�re yerle�tir
        Instantiate(Road[Random.Range(0, Road.Count)], transform.forward * roadLenght, transform.rotation);
        roadLenght += 3.2f; // Yeni yolun uzunlu�unu artt�r
    }

    // Oyun yeniden ba�lat�ld���nda sahneyi tekrar y�kle
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Ana men�ye d�nmek i�in sahneyi de�i�tir
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Oyuna devam etmek i�in zaman� tekrar ba�lat
    public void ContinueGame()
    {
        Time.timeScale = 1; // Zaman� ba�lat
        characterController.musicSource.Play(); // M�zi�i tekrar �al
        characterController.stopScreen.SetActive(false); // Durma ekran�n� gizle
    }
}
