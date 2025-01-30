using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Arabalarýn spawn edileceði farklý referans noktalarý
    [SerializeField] Transform[] spawnPoints;

    // Arabalarýn prefablarý (önceden oluþturulmuþ araba nesneleri)
    [SerializeField] GameObject[] carPrefabs;

    // Arabalarýn spawn edilme aralýðý için minimum ve maksimum süre
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 3f;

    [SerializeField] CharacterController _characterController; // Karakterin hayatta olup olmadýðýný kontrol etmek için

    private void Start()
    {
        StartCoroutine(SpawnCars()); // Baþlangýçta arabalarýn spawn edilmesini baþlatýr
    }

    IEnumerator SpawnCars()
    {
        // Karakter hayatta olduðu sürece arabalar spawn edilmeye devam eder
        while (_characterController.isAlive)
        {
            // Rastgele bir süre beklet
            float randomTime = Random.Range(minSpawnTime, maxSpawnTime); // Min ve max süre arasýnda rastgele bir deðer seç
            yield return new WaitForSeconds(randomTime); // Rastgele süre kadar bekle

            // Rastgele bir spawn noktasý seç
            int randomIndex = Random.Range(0, spawnPoints.Length); // Spawn noktalarýndan birini rastgele seç
            Transform spawnPoint = this.spawnPoints[randomIndex];  // Seçilen spawn noktasýný al

            // Arabayý üret (Prefab'larý arasýnda rastgele birini seçip, seçilen noktada oluþtur)
            Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
