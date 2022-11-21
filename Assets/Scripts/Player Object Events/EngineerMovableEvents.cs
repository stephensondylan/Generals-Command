using UnityEngine;
using Mirror;

public class EngineerMovableEvents : MovableEvents
{
    AudioSource _audioSource;

    public override void Init()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        base.Init();
    }


    [Server]
    public override void OnMovementBegin()
    {
        RpcOnMovementBegin();
    }


    [ClientRpc]
    public void RpcOnMovementBegin()
    {
        PlayOneShotAudio(_audioSource, movementBeginSounds);
        StartParticleEffect(movementBeginParticles);
        StartParticleEffect(movementOngoingParticles);

        SetAnimationBool(animators, "IsMoving", true);
    }


    [Server]
    public override void OnMovementEnd()
    {
        if (GameData.instance.isHost)
            RpcOnMovementEnd();
    }


    [ClientRpc]
    public void RpcOnMovementEnd()
    {
        SetAnimationBool(animators, "IsMoving", false);
        StopParticleEffect(movementOngoingParticles);

        SetAnimationBool(animators, "IsMoving", false);
    }


    [Server]
    public override void OnUpdate(float forward, float right)
    {
        RpcOnUpdate(forward);
    }


    [ClientRpc]
    public void RpcOnUpdate(float forward)
    {
        foreach (Animator animator in animators)
        {
            animator.SetFloat("ForwardFacing", forward);
        }
    }
}