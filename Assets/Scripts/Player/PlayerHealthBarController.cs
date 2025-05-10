public class PlayerHealthBarController : HealthBarComponent
{
    protected override HealthController HealthController => (PlayerController.GetInstance() as ICharacter).Health;
}