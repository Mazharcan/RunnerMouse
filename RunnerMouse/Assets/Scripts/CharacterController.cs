using TMPro;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Inspector'dan eri�ebilmek i�in private de�i�kenlerin ba��na [SerializeField] ekledik
    [SerializeField] Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] float speed;                                   // Karakterin h�z�
    [SerializeField] private float lateralSmootSpeed = 10f;         // Yanal hareket ge�i� h�z�
    [SerializeField] TextMeshProUGUI scoreText;                     // Skoru g�sterecek text
    [SerializeField] TextMeshProUGUI cheeseScoreText;               // Peynir skorunu g�sterecek text
    [SerializeField] public AudioSource effectSource, musicSource;  // Ses efektleri ve m�zik
    [SerializeField] AudioClip cheeseClip, deathClip;               // Peynir ve �l�m ses efektleri
    [SerializeField] TextMeshProUGUI BestScore;                     // En iyi skoru g�sterecek text

    float[] xPosition = { 0f, 0.368f, 0.736f };   // Karakterin ge�ebilece�i 3 �erit pozisyonu

    int currentXPositionIndex = 1;   // Ba�lang��ta hangi �eritte olaca��n� belirleyen index

    Vector3 targetPosition;          // Karakterin hedef pozisyonu

    public bool isAlive = true;      // Karakter hayatta m�?

    [SerializeField] GameObject deathScreen;        // Death ekran�
    [SerializeField] public GameObject stopScreen;  // Pause ekran�

    private float score;        // Skor
    private float cheeseScore;  // Peynir skoru

    private void Awake()
    {
        Time.timeScale = 1; // Zaman� ba�lat
        targetPosition = transform.position; // Ba�lang�� hedefi
    }

    private void Update()
    {
        if (isAlive)
        {
            // Skor g�ncelleme
            score += Time.deltaTime * speed * 5;
            scoreText.text = "Score : " + score.ToString("f0");
            cheeseScoreText.text = "Cheese : " + cheeseScore.ToString();

            // Yanal hareketler
            if (Input.GetKeyDown(KeyCode.A) && currentXPositionIndex > 0)       // Sol �eride ge�i�
            {
                currentXPositionIndex--;
                UpdateLateralPosition(); // Yeni hedef pozisyonunu g�ncelle
            }
            else if (Input.GetKeyDown(KeyCode.D) && currentXPositionIndex < 2) // Sa� �eride ge�i�
            {
                currentXPositionIndex++;
                UpdateLateralPosition(); // Yeni hedef pozisyonunu g�ncelle
            }
        }

        // Oyun durdurma i�lemi
        if (isAlive && Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;         // Zaman� durdur
            stopScreen.SetActive(true); // Durma ekran�n� g�ster
            musicSource.Stop();         // M�zik durdur
        }
    }

    // Sabit FPS ile hareket etmek i�in FixedUpdate kullan�l�r
    private void FixedUpdate()
    {
        if (isAlive) // E�er hayatta isek
        {
            // Karakteri ileriye do�ru hareket ettir
            Vector3 forwardMove = Vector3.forward * speed * Time.fixedDeltaTime;

            // Yumu�ak ge�i� yaparak hedef pozisyona y�nel
            Vector3 currentPosition = rb.position;
            Vector3 lateralMove = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * lateralSmootSpeed);

            // Yanal ve ileri hareketi birle�tir
            Vector3 combineMove = new Vector3(lateralMove.x, transform.position.y, rb.position.z) + forwardMove;
            rb.MovePosition(combineMove);
        }
    }

    // Yanal pozisyonu g�ncelleme
    void UpdateLateralPosition()
    {
        targetPosition = new Vector3(xPosition[currentXPositionIndex], transform.position.y, transform.position.z); // Yeni hedef pozisyonu
    }

    // Arabalarla �arp��ma durumu
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cars"))
        {
            isAlive = false;        // Karakter �ld�
            animator.avatar = null; // Animatoru devre d��� b�rak
            animator.SetBool("death", true); // �l�m animasyonunu ba�lat

            musicSource.Stop(); // M�zik durdur
            effectSource.clip = deathClip; // �l�m sesi �al
            effectSource.Play();

            SaveBestScore(); // En iyi skoru kaydet
            
            // �l�m ekran�nda en iyi skoru g�ster
            float bestScore = PlayerPrefs.GetFloat("BestScore", 0);
            BestScore.text = "Best Score : " + bestScore.ToString("f0");

            deathScreen.SetActive(true); // �l�m ekran�n� g�ster
        }
    }

    // Peynir toplama sistemi
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cheese"))
        {
            Debug.Log("Cheese Collected");
            score += 5;     // Skoru artt�r
            speed += 0.1f;  // H�z� artt�r

            effectSource.clip = cheeseClip; // Peynir sesi �al
            effectSource.Play();

            Destroy(other.gameObject); // Peynir objesini yok et

            cheeseScore++; // Peynir say�s�n� artt�r
        }
    }

    // En iyi skoru kaydetme
    private void SaveBestScore()
    {
        float bestScore = PlayerPrefs.GetFloat("BestScore", 0); // Daha �nce kaydedilen skoru al, yoksa 0
        if (score > bestScore) // E�er yeni skor daha y�ksekse
        {
            PlayerPrefs.SetFloat("BestScore", score); // Yeni en iyi skoru kaydet
            PlayerPrefs.Save(); // De�i�iklikleri kaydet
        }
    }
}
