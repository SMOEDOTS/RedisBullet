using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public int moveSpeed;
    int speedx, speedy;
    public GameObject bulletPrefab;
    bool allowfire = true;
    public Transform bulletSpawn;
    public int bulletSpeed;
    public Sprite tsprite;

	// Use this for initialization
	void Start () {
		
	}

    public override void OnStartLocalPlayer()
    {
        GetComponent<SpriteRenderer>().sprite = tsprite;
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetKeyUp(KeyCode.W))
        {
            speedy = 0;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            speedy = 0;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            ;
            speedx = 0;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            speedx = 0;
        }


        GetComponent<Rigidbody2D>().velocity = new Vector2(speedx, speedy);

        if (Input.GetKeyDown(KeyCode.W))
        {
            speedy = moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            speedy = -moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            speedx = -moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            speedx = moveSpeed;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(speedx, speedy);

        if (Input.GetKey(KeyCode.W))
        {
            speedy = moveSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            speedy = -moveSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            speedx = -moveSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            speedx = moveSpeed;
        }

        if ((Input.GetMouseButton(0)))
        {
            CmdFire1();
        }
    }

    [Command]
    void CmdFire1()
    {
        if (!allowfire)
        {
            return;
        }
        allowfire = false;
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

        Destroy(bullet, 2.0f);

        NetworkServer.Spawn(bullet);

        StartCoroutine(AllowToFire());
    }

    IEnumerator AllowToFire()
    {
        yield return new WaitForSeconds(0.15f);
        allowfire = true;
    }
}
