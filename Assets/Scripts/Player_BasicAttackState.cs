using UnityEngine;

public class Player_BasicAttackState : EntityState
{

    private float attackVelocityTimer;

    private const int FirstComboIndex = 1; // Starting index for combo attacks, animator should handle this
    private int comboIndex = 1;
    private int comboLimit = 3;

    private float lastTimeAttacked;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if(comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length; // Ensure combo limit matches the length of attack velocities
        }
    }

    public override void Enter()
    {
        base.Enter();
        ResetComboIndexIfNeeeded();

        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }


    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;

        lastTimeAttacked = Time.time;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(player.attackVelocity[comboIndex-1].x * player.facingDir, player.attackVelocity[comboIndex-1].y);
    }

    private void ResetComboIndexIfNeeeded()
    {
        if(Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex = FirstComboIndex; // Reset combo index if enough time has passed since last attack
        }

        if (comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex; // Reset combo index if it exceeds the limit
        }
    }
}
