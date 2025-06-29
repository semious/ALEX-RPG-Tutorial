using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>(); 
    }

    private void DisableMovementAndJump()
    {
        player.EnableJumpAndMovement(false);
    }

    private void EnableMovementAndJump()
    {
        player.EnableJumpAndMovement(true);
    }

}
