using System.Collections;
using UnityEngine;

public class CheeseSpawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;    // Peynirlerin spawn edileceði farklý konumlar

    [SerializeField] GameObject[] cheesePrefabs; // Peynirlerin prefableri (önceden oluþturulmuþ peynir nesneleri)

    [SerializeField] float minSpawnTime = 1f; // Peynirin en erken ne kadar sürede spawn olacaðý (saniye cinsinden)
    [SerializeField] float maxSpawnTime = 3f; // Peynirin en geç ne kadar sürede spawn olacaðý (saniye cinsinden)

    [SerializeField] CharacterController _characterController; // Karakterin hayatta olup olmadýðýný kontrol etmek için

    // Coroutine: Belirli bir iþlemi zaman içinde tekrar etmek için kullanýlýr.
    private void Start()
    {
        StartCoroutine(SpawnCheeses()); // Baþlangýçta peynir üretme iþlemini baþlatýyoruz
    }

    IEnumerator SpawnCheeses()
    {
        // Karakter hayatta olduðu sürece peynir spawn etmeye devam et
        while (_characterController.isAlive)
        {
            // Rastgele bir süre bekle
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime); // Min ve max süre arasýnda rastgele bir deðer seç
            yield return new WaitForSeconds(randomTime); // Rastgele seçilen süre kadar bekle

            // Rastgele bir spawn noktasý seç
            int randomIndex = Random.Range(0, spawnPoints.Length); // Spawn noktalarýndan birini rastgele seç
            Transform spawnPoint = this.spawnPoints[randomIndex];  // Seçilen spawn noktasýný al

            // Peynir üret (Prefab'ýn ilk elemanýný seçip, seçilen noktada oluþtur)
            Instantiate(cheesePrefabs[0], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
