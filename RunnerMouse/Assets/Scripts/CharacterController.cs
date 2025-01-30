using TMPro;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Inspector'dan eriþebilmek için private deðiþkenlerin baþýna [SerializeField] ekledik
    [SerializeField] Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] float speed;                                   // Karakterin hýzý
    [SerializeField] private float lateralSmootSpeed = 10f;         // Yanal hareket geçiþ hýzý
    [SerializeField] TextMeshProUGUI scoreText;                     // Skoru gösterecek text
    [SerializeField] TextMeshProUGUI cheeseScoreText;               // Peynir skorunu gösterecek text
    [SerializeField] public AudioSource effectSource, musicSource;  // Ses efektleri ve müzik
    [SerializeField] AudioClip cheeseClip, deathClip;               // Peynir ve ölüm ses efektleri
    [SerializeField] TextMeshProUGUI BestScore;                     // En iyi skoru gösterecek text

    float[] xPosition = { 0f, 0.368f, 0.736f };   // Karakterin geçebileceði 3 þerit pozisyonu

    int currentXPositionIndex = 1;   // Baþlangýçta hangi þeritte olacaðýný belirleyen index

    Vector3 targetPosition;          // Karakterin hedef pozisyonu

    public bool isAlive = true;      // Karakter hayatta mý?

    [SerializeField] GameObject deathScreen;        // Death ekraný
    [SerializeField] public GameObject stopScreen;  // Pause ekraný

    private float score;        // Skor
    private float cheeseScore;  // Peynir skoru

    private void Awake()
    {
        Time.timeScale = 1; // Zamaný baþlat
        targetPosition = transform.position; // Baþlangýç hedefi
    }

    private void Update()
    {
        if (isAlive)
        {
            // Skor güncelleme
            score += Time.deltaTime * speed * 5;
            scoreText.text = "Score : " + score.ToString("f0");
            cheeseScoreText.text = "Cheese : " + cheeseScore.ToString();

            // Yanal hareketler
            if (Input.GetKeyDown(KeyCode.A) && currentXPositionIndex > 0)       // Sol þeride geçiþ
            {
                currentXPositionIndex--;
                UpdateLateralPosition(); // Yeni hedef pozisyonunu güncelle
            }
            else if (Input.GetKeyDown(KeyCode.D) && currentXPositionIndex < 2) // Sað þeride geçiþ
            {
                currentXPositionIndex++;
                UpdateLateralPosition(); // Yeni hedef pozisyonunu güncelle
            }
        }

        // Oyun durdurma iþlemi
        if (isAlive && Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;         // Zamaný durdur
            stopScreen.SetActive(true); // Durma ekranýný göster
            musicSource.Stop();         // Müzik durdur
        }
    }

    // Sabit FPS ile hareket etmek için FixedUpdate kullanýlýr
    private void FixedUpdate()
    {
        if (isAlive) // Eðer hayatta isek
        {
            // Karakteri ileriye doðru hareket ettir
            Vector3 forwardMove = Vector3.forward * speed * Time.fixedDeltaTime;

            // Yumuþak geçiþ yaparak hedef pozisyona yönel
            Vector3 currentPosition = rb.position;
            Vector3 lateralMove = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * lateralSmootSpeed);

            // Yanal ve ileri hareketi birleþtir
            Vector3 combineMove = new Vector3(lateralMove.x, transform.position.y, rb.position.z) + forwardMove;
            rb.MovePosition(combineMove);
        }
    }

    // Yanal pozisyonu güncelleme
    void UpdateLateralPosition()
    {
        targetPosition = new Vector3(xPosition[currentXPositionIndex], transform.position.y, transform.position.z); // Yeni hedef pozisyonu
    }

    // Arabalarla çarpýþma durumu
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cars"))
        {
            isAlive = false;        // Karakter öldü
            animator.avatar = null; // Animatoru devre dýþý býrak
            animator.SetBool("death", true); // Ölüm animasyonunu baþlat

            musicSource.Stop(); // Müzik durdur
            effectSource.clip = deathClip; // Ölüm sesi çal
            effectSource.Play();

            SaveBestScore(); // En iyi skoru kaydet
            
            // Ölüm ekranýnda en iyi skoru göster
            float bestScore = PlayerPrefs.GetFloat("BestScore", 0);
            BestScore.text = "Best Score : " + bestScore.ToString("f0");

            deathScreen.SetActive(true); // Ölüm ekranýný göster
        }
    }

    // Peynir toplama sistemi
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cheese"))
        {
            Debug.Log("Cheese Collected");
            score += 5;     // Skoru arttýr
            speed += 0.1f;  // Hýzý arttýr

            effectSource.clip = cheeseClip; // Peynir sesi çal
            effectSource.Play();

            Destroy(other.gameObject); // Peynir objesini yok et

            cheeseScore++; // Peynir sayýsýný arttýr
        }
    }

    // En iyi skoru kaydetme
    private void SaveBestScore()
    {
        float bestScore = PlayerPrefs.GetFloat("BestScore", 0); // Daha önce kaydedilen skoru al, yoksa 0
        if (score > bestScore) // Eðer yeni skor daha yüksekse
        {
            PlayerPrefs.SetFloat("BestScore", score); // Yeni en iyi skoru kaydet
            PlayerPrefs.Save(); // Deðiþiklikleri kaydet
        }
    }
}
