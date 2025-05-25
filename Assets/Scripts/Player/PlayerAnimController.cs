public class PlayerAnimController : CharacterAnimController
{
    public void OnRevive()
    {
        Material.SetFloat(Fade, 1.0f);
    }
}