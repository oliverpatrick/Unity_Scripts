using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public ProjectileWeapon projectileWeapon;
    public RigidBody rb;
    public BoxCollider collider;
    public Transform player, gunContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    // RigidBody assigned to weapons - Extrapolate & Continuous Speculative
    private void Start()
    {
      //Setup
      if (!equipped)
      {
        projectileWeapon.enabled = false;
        rb.isKinematic = false;
        collider.isTrigger = false;
      }

      if (equipped) {
        projectileWeapon.enabled = true;
        rb.isKinematic = true;
        coll.isTrigger = true;
        slotFull = true;
      }
    }


    private void Update()
    {
      //Check if player is in range and "E" is pressed
      Vector3 distanceToPlayer = player.position - transform.position;
      if(!equipped && distanceToPlayer.magniture <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

      //Drop if equipped and "Q" is pressed
      if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
    }
    public void PickUp()
    {
        equipped = true;
        slotFull = true;

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localPosition = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make RigidBody kinematic and boxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable Script
        projectileWeapon.enabled = true;
    }

    public void Drop()
    {
        equipped = false;
        slotFull = false;

        //Set parent to null
        transform.SetParent(null);

        //Make RigidBody kinematic and boxCollider a trigger
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<RigidBody>().velocity;

        //AddForce
        rb.AddForce(fpsCam.foward * dropForwardForce, ForceMode.Impluse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impluse);

        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Enable Script
        projectileWeapon.enabled = false;
    }
}
