using UnityEngine;

public class Player_BasicAttackState : EntityState
{

    private float attackVelocityTimer;
    private float lastTimeAttacked;

    private bool comboAttackQueued;
    private int attackDir;
    private int comboIndex = 1;
    private int comboLimit = 3;

    private const int FirstComboIndex = 1;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length; // Ensure combo limit matches the length of attack velocities
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeeded();

        attackDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;

        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();

    }


    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();


        if (triggerCalled)
            HandleStateExit();
    }


    public override void Exit()
    {
        base.Exit();
        comboIndex++;

        lastTimeAttacked = Time.time;
    }
    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
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
        player.SetVelocity(player.attackVelocity[comboIndex - 1].x * attackDir, player.attackVelocity[comboIndex - 1].y);
    }

    private void ResetComboIndexIfNeeeded()
    {
        if (Time.time > lastTimeAttacked + player.comboResetTime)
        {
            comboIndex = FirstComboIndex; // Reset combo index if enough time has passed since last attack
        }

        if (comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex; // Reset combo index if it exceeds the limit
        }
    }
}
