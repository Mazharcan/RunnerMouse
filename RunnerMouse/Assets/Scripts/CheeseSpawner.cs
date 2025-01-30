using System.Collections;
using UnityEngine;

public class CheeseSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;    // Peynirlerin spawn edilece�i farkl� konumlar

    [SerializeField] GameObject[] cheesePrefabs; // Peynirlerin prefableri (�nceden olu�turulmu� peynir nesneleri)

    [SerializeField] float minSpawnTime = 1f; // Peynirin en erken ne kadar s�rede spawn olaca�� (saniye cinsinden)
    [SerializeField] float maxSpawnTime = 3f; // Peynirin en ge� ne kadar s�rede spawn olaca�� (saniye cinsinden)

    [SerializeField] CharacterController _characterController; // Karakterin hayatta olup olmad���n� kontrol etmek i�in

    // Coroutine: Belirli bir i�lemi zaman i�inde tekrar etmek i�in kullan�l�r.
    private void Start()
    {
        StartCoroutine(SpawnCheeses()); // Ba�lang��ta peynir �retme i�lemini ba�lat�yoruz
    }

    IEnumerator SpawnCheeses()
    {
        // Karakter hayatta oldu�u s�rece peynir spawn etmeye devam et
        while (_characterController.isAlive)
        {
            // Rastgele bir s�re bekle
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime); // Min ve max s�re aras�nda rastgele bir de�er se�
            yield return new WaitForSeconds(randomTime); // Rastgele se�ilen s�re kadar bekle

            // Rastgele bir spawn noktas� se�
            int randomIndex = Random.Range(0, spawnPoints.Length); // Spawn noktalar�ndan birini rastgele se�
            Transform spawnPoint = this.spawnPoints[randomIndex];  // Se�ilen spawn noktas�n� al

            // Peynir �ret (Prefab'�n ilk eleman�n� se�ip, se�ilen noktada olu�tur)
            Instantiate(cheesePrefabs[0], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
