using System.Collections;
using UnityEditor;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    [Header("Health System")]
    [SerializeField] private float maxHealth = 100f; 
    [SerializeField]private float currentHealth; 
    public float CurrentHealth => currentHealth;//usamos getter y setter para el gameManager - solo lectura

    private MeshRenderer mesh;

    [SerializeField] private GameObject vfx;
    [SerializeField] private bool isDead;
    private Rigidbody rb;
    private Collider col;
    private Attack attack;
    public bool IsDead
    {
        get => isDead;
          
    }

    [SerializeField] private GameObject vfx;

    private Rigidbody rb;
    private Collider col;
    private Attack attack;
    private MeshRenderer mesh;
    private void Awake() 
    { 
        currentHealth = maxHealth; 
        isDead = false; 
        mesh = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        attack = GetComponent<Attack>();
    }
    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        //clamp permite normalizar un valor de acuerdo a los limites
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        /*
         * math.clamp(5, 1, 10) => 5 (esta dentro del rango)
         * math.clamp(-4, 0, 10) => 0 (es menor que el minimo, devuelve 0)
         * math.clamp(15, 0, 10) => 10 (es mayor que el maximo, devuelve 10)
         */

        if (currentHealth <= 0f)
        {
            Dead();
        }
    }
    private void Dead() 
    { 
        isDead = true;
        
        // Notificacion al GameManager si existiera
        if (gameObject.CompareTag("Player"))
        {
            GameManager.Instance.ChangeState(GameState.GameOver);


        }
        else if(gameObject.CompareTag("Character"))
        {
            GameManager.Instance.EnemyDefeat();
        }

        vfx.SetActive(true);
    
        StartCoroutine(DestroyTransition());
        
        //Destroy object
        //efectos
        //animaciones

    }

    IEnumerator DestroyTransition()
    {

        mesh.enabled = false;
        col.enabled = false;
        rb.isKinematic = true;
        attack.enabled = false;
        //gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }



}
