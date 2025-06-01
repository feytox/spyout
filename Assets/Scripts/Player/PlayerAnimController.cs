public class PlayerAnimController : CharacterAnimController
{
    public void OnRevive()
    {
        CurrentFlashTime = -1;
        CurrentDissolveTime = -1;
        Material.SetFloat(Fade, 1.0f);
    }
}