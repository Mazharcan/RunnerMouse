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
        // E�er oyun duraklat�lm��sa, arabalar hareket etmesin
        if (Time.timeScale == 0) return;

        // E�er karakter hayatta ise arabay� hareket ettir
        if (characterController.isAlive)
        {
            transform.Translate(0, 0, carSpeed);
        }
    }
}
