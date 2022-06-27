using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSitMove : MonoBehaviour
{
    [SerializeField]
    private PlayerCtrl player;
    void Update()
    {
        SitMove();
        Debug.Log(player.isGun);
    }
    void SitMove()
    {
        float spd = player.movespd;
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (player.isGun == true)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    spd = player.movespd * 0.5f;
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                    player.animator.SetBool("isGunSitMove_F", player.isSit);
                }
                else if (player.isSit == true)
                {
                    spd = player.movespd * 0.5f;
                    player.animator.SetBool("isGunSitMove_F", true);
                }
            }
            else if(player.isGun == false)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    spd = player.movespd * 0.5f;
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                    player.animator.SetBool("isSitMove_F", player.isSit);
                }
                else if (player.isSit == true)
                {
                    spd = player.movespd * 0.5f;
                    player.animator.SetBool("isSitMove_F", true);
                }
            }
        }
        else if(Input.GetKey(KeyCode.A))
        {
            if(player.isGun == true)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    spd = player.movespd * 0.5f;
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                    player.animator.SetBool("isGunSitMove_L", player.isSit);
                }
                else if (player.isSit == true)
                {
                    spd = player.movespd * 0.5f;
                    player.animator.SetBool("isGunSitMove_L", true);
                }
            }
            else if(player.isGun == false)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    spd = player.movespd * 0.5f;
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                    player.animator.SetBool("isSitMove_L", player.isSit);
                }
                else if (player.isSit == true)
                {
                    spd = player.movespd * 0.5f;
                    player.animator.SetBool("isSitMove_L", true);
                }
            }
        }
        else if(Input.GetKey(KeyCode.D))
        {
            if (player.isGun == true)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    spd = player.movespd * 0.5f;
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                    player.animator.SetBool("isGunSitMove_R", player.isSit);
                }
                else if (player.isSit == true)
                {
                    spd = player.movespd * 0.5f;
                    player.animator.SetBool("isGunSitMove_R", true);
                }
            }
            else if (player.isGun == false)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    spd = player.movespd * 0.5f;
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                    player.animator.SetBool("isSitMove_R", player.isSit);
                }
                else if (player.isSit == true)
                {
                    spd = player.movespd * 0.5f;
                    player.animator.SetBool("isSitMove_R", true);
                }
            }
        }
        else
        {
            if (player.isGun == true)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                }
            }
            if (player.isGun == false)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    player.isSit = !player.isSit;
                    player.animator.SetBool("isSit", player.isSit);
                }
            }
            player.animator.SetBool("isSitMove_F", false);
            player.animator.SetBool("isSitMove_L", false);
            player.animator.SetBool("isSitMove_R", false);
            player.animator.SetBool("isGunSitMove_F", false);
            player.animator.SetBool("isGunSitMove_L", false);
            player.animator.SetBool("isGunSitMove_R", false);
        }
    }
}
