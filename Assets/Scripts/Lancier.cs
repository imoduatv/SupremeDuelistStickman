using UnityEngine;

public class Lancier : MonoBehaviour
{
	private GameManager SkinChoose;

	public GameObject Montagne;

	public LeftJoystick leftJoystick;

	public RightJoystick rightJoystick;

	public Vector2 direction;

	public Vector2 Power;

	public int timeFirsAtt;

	public int Cooldown;

	public Rigidbody2D rb;

	public bool PlayerOneOrTwo;

	public bool PowerhitReady;

	public bool directionChosen;

	public float maniment;

	public Rigidbody2D Bras;

	public Rigidbody2D AutreBras;

	private PlayerDirection DirPlayer;

	public bool JoystickOnZero;

	public SpriteRenderer symboleUlt;

	public bool isBlue;

	public AudioSource source;

	public AudioClip PowerAbility;

	private GameManager gManag;

	public GameObject Manager;

	public GameObject Camera;

	public float ReculLance;

	public bool StopContinue;

	public Rigidbody2D Corps;

	private void Start()
	{
		if (source == null)
		{
			source = GameObject.Find("SoundEffect").GetComponent<AudioSource>();
		}
		Manager = GameObject.Find("GameManager");
		SkinChoose = GameObject.Find("GameManager").GetComponent<GameManager>();
		rb = GetComponent<Rigidbody2D>();
		if (isBlue)
		{
			symboleUlt.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f);
		}
		else
		{
			symboleUlt.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
		}
		if (PlayerOneOrTwo)
		{
			DirPlayer = GameObject.Find("bout2").GetComponent<PlayerDirection>();
		}
		AutreBras.gameObject.GetComponent<HingeJoint2D>().useLimits = false;
	}

	private void FixedUpdate()
	{
		timeFirsAtt++;
		Cooldown--;
		Bras.AddForce(direction * maniment * Time.fixedDeltaTime);
		AutreBras.AddForce(-direction * ReculLance * Time.fixedDeltaTime);
		if (!PlayerOneOrTwo)
		{
			if (!SkinChoose.OnePlayer)
			{
				direction = leftJoystick.GetInputDirection();
				JoystickOnZero = leftJoystick.IsTouching;
			}
			else if (!SkinChoose.LeftUser)
			{
				direction = rightJoystick.GetInputDirection();
				JoystickOnZero = rightJoystick.IsTouching;
			}
			else
			{
				direction = leftJoystick.GetInputDirection();
				JoystickOnZero = leftJoystick.IsTouching;
			}
		}
		else if (!DirPlayer.AI)
		{
			direction = rightJoystick.GetInputDirection();
			JoystickOnZero = rightJoystick.IsTouching;
		}
		else
		{
			direction = DirPlayer.direction / 4f;
		}
		direction = direction.normalized;
		if (direction.magnitude != 0f)
		{
			Power = direction;
		}
		if (Cooldown <= 0)
		{
			if (direction.magnitude > 0.2f && timeFirsAtt > 100)
			{
				PowerhitReady = true;
			}
			if (direction.magnitude == 0f && PowerhitReady && !JoystickOnZero)
			{
				directionChosen = true;
				PowerhitReady = false;
			}
			if (isBlue)
			{
				symboleUlt.GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 1f);
			}
			else
			{
				symboleUlt.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
			}
		}
		if (directionChosen)
		{
			source.PlayOneShot(PowerAbility);
			Cooldown = 200;
			directionChosen = false;
			symboleUlt.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
			Montagne.transform.position = base.transform.position;
			Montagne.transform.rotation = base.transform.rotation;
			Montagne.gameObject.SetActive(value: true);
			Montagne.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
			Montagne.GetComponent<Rigidbody2D>().AddForce(Power * 25f, ForceMode2D.Impulse);
		}
	}
}
