using System.Collections;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    private Transform ShopLocation; // The target location for the NPC (e.g., a shop)
    public float WalkSpeed = 2f;
    private Animator _animator;
    private Rigidbody2D r2d;
    private bool isMoving = false;

    public int MoneySpawn = 10;

    public GameObject floatingTextPrefab; // Reference to the floating text prefab
    public Transform NPccanvas;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        r2d = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform shopLocation, float walkSpeed)
    {
        ShopLocation = shopLocation;
        WalkSpeed = walkSpeed;
    }

    private void Start()
    {
        if (ShopLocation == null)
        {
            Debug.LogError("Shop location not assigned!");
        }
    }

    private void ShowFloatingText(int damage)
    {
        if (floatingTextPrefab != null && NPccanvas != null)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, NPccanvas);
            NotifText floatingTextComponent = floatingText.GetComponent<NotifText>();
            floatingTextComponent.SetText("+" + damage.ToString(), Color.yellow);
        }
    }

    private void Update()
    {
        if (ShopLocation != null)
        {
            MoveTowardsShop();
        }
    }

    private void MoveTowardsShop()
    {
        float distanceToShop = Vector2.Distance(transform.position, ShopLocation.position);

        if (distanceToShop > 0.1f) // Check if NPC is not at the shop yet
        {
            Vector2 direction = (ShopLocation.position - transform.position).normalized;
            r2d.linearVelocity = direction * WalkSpeed;
            SetMovementAnimation(true);

            if (direction.x < 0)
            {
                Turn(-1); // Face left
            }
            else if (direction.x > 0)
            {
                Turn(1); // Face right
            }
        }
        else
        {
            r2d.linearVelocity = Vector2.zero;
            SetMovementAnimation(false); // Stop moving animation when at the shop
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shop"))
        {
            ShowFloatingText(MoneySpawn);
            GameManager.instance.PlayerMoney += MoneySpawn;


            StartCoroutine(DestroyAfterDelay(1f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Destroy(gameObject); // Destroy the GameObject after the delay
    }

    private void SetMovementAnimation(bool isMoving)
    {
        this.isMoving = isMoving;
        _animator.SetBool("Idle", !isMoving);
        _animator.SetBool("Walking", isMoving);
    }

    private void Turn(int direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = direction * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

}
