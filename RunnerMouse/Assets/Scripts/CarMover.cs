using UnityEngine;

public class CarMover : MonoBehaviour
{
    [SerializeField] private float carSpeed;

     CharacterController characterController;

    private void Start()
    {
        Time.timeScale = 1;
        characterController = FindAnyObjectByType<CharacterController>();
    }

    private void Update()
    {
        // Eðer oyun duraklatýlmýþsa, arabalar hareket etmesin
        if (Time.timeScale == 0) return;

        // Eðer karakter hayatta ise arabayý hareket ettir
        if (characterController.isAlive)
        {
            transform.Translate(0, 0, carSpeed);
        }
    }
}
