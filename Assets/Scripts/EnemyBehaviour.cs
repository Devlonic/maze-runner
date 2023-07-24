using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyBehaviour : MonoBehaviour {
    private const float DEFAULT_SPEED = 4f;

    [SerializeField]
    private float _speed = DEFAULT_SPEED;

    [SerializeField]
    private Vector2 direction = Vector2.zero;

    [SerializeField]
    private GameObject followTarget;

    private Transform followTargetTransform;
    private Transform currentTransform;

    private Rigidbody2D _rb;
    private AudioSource _audioSource;
    private Animator _animator;
    private ParticleSystem _particles;



    private float GetAnimationSpeed() {
        return _speed / DEFAULT_SPEED;
    }
    public void IncreaseSpeed(float speed) {
        this._speed += speed;
        this._animator.speed = GetAnimationSpeed();
        this._audioSource.pitch = GetAnimationSpeed();
    }
    public void DecreaseSpeed(float speed) {
        this._speed -= speed;
        this._animator.speed = GetAnimationSpeed();
        this._audioSource.pitch = GetAnimationSpeed();
    }
    public void EnableParticles() {
        _particles.Play();
    }
    public void DisableParticles() {
        if ( _speed == DEFAULT_SPEED ) // todo remake this
            _particles.Stop();
    }



    // Start is called before the first frame update
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _particles = GetComponent<ParticleSystem>();

        if ( followTarget is not null ) {
            this.followTargetTransform = followTarget.GetComponent<Transform>();
        }
        currentTransform = GetComponent<Transform>();

        this.StartCoroutine(nameof(DirectionCoroutine));
    }


    private readonly System.Random random = new System.Random();
    // Update is called once per frame
    void Update() {

    }

    [SerializeField]
    private Vector2 currVect = Vector2.zero;

    private bool randomizerFlag = false;
    private IEnumerator DirectionCoroutine() {
        while ( true ) {
            //direction.x = random.Next(-1, 2);
            //direction.y = random.Next(-1, 2);
            if ( randomizerFlag )
                yield return new WaitForSeconds(2f);

            randomizerFlag = false;

            currVect = followTargetTransform.position - currentTransform.position;

            direction.x = currVect.x < 0 ? -1 : currVect.x > 0 ? 1 : 0;
            direction.y = currVect.y < 0 ? -1 : currVect.y > 0 ? 1 : 0;

            yield return new WaitForSeconds(0.3f);
        }
    }

    private Vector2 prevDirection = Vector2.zero;
    private void FixedUpdate() {
        _rb.MovePosition(_rb.position + direction * _speed * Time.fixedDeltaTime);

        //_animator.SetBool("IsWalking", direction != Vector2.zero);

        const string FRONT = "DirectionFront";
        const string BACK = "DirectionBack";
        const string LEFT = "DirectionLeft";
        const string RIGHT = "DirectionRight";
        const string UNKNOWN = "DirectionUnknown";

        if ( direction != Vector2.zero && prevDirection != direction ) {
            prevDirection = direction;
            string direct = "";

            if ( direction == Vector2.down )
                direct = FRONT;
            else if ( direction == Vector2.up )
                direct = BACK;
            else if ( direction == Vector2.left )
                direct = LEFT;
            else if ( direction == Vector2.right )
                direct = RIGHT;
            else
                direct = UNKNOWN;
        }

        //if ( direction != Vector2.zero && !_audioSource.isPlaying ) {
        //    _audioSource.Play();
        //}
        //else if ( direction == Vector2.zero ) {
        //    _audioSource.Pause();
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if ( collision != null ) {
            if ( collision.gameObject.tag == "Ground" ) {
                Debug.Log("Enemy touched ground");
                randomizerFlag = true;
                if ( direction == Vector2.left )
                    direction = Vector2.up;
                if ( direction == Vector2.right )
                    direction = Vector2.up;
                if ( direction == Vector2.up )
                    direction = Vector2.right;
                if ( direction == Vector2.down )
                    direction = Vector2.right;
            }
            else if ( collision.gameObject.tag == "Player" ) {
                Debug.Log("Enemy touched player");
            }
        }
    }
}


