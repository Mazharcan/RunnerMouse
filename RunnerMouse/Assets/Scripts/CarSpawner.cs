using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Arabalar�n spawn edilece�i farkl� referans noktalar�
    [SerializeField] Transform[] spawnPoints;

    // Arabalar�n prefablar� (�nceden olu�turulmu� araba nesneleri)
    [SerializeField] GameObject[] carPrefabs;

    // Arabalar�n spawn edilme aral��� i�in minimum ve maksimum s�re
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 3f;

    [SerializeField] CharacterController _characterController; // Karakterin hayatta olup olmad���n� kontrol etmek i�in

    private void Start()
    {
        StartCoroutine(SpawnCars()); // Ba�lang��ta arabalar�n spawn edilmesini ba�lat�r
    }

    IEnumerator SpawnCars()
    {
        // Karakter hayatta oldu�u s�rece arabalar spawn edilmeye devam eder
        while (_characterController.isAlive)
        {
            // Rastgele bir s�re beklet
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime); // Min ve max s�re aras�nda rastgele bir de�er se�
            yield return new WaitForSeconds(randomTime); // Rastgele s�re kadar bekle

            // Rastgele bir spawn noktas� se�
            int randomIndex = Random.Range(0, spawnPoints.Length); // Spawn noktalar�ndan birini rastgele se�
            Transform spawnPoint = this.spawnPoints[randomIndex];  // Se�ilen spawn noktas�n� al

            // Arabay� �ret (Prefab'lar� aras�nda rastgele birini se�ip, se�ilen noktada olu�tur)
            Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
