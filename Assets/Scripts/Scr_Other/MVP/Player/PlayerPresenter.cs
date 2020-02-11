public class PlayerPresenter : IPresenter
{
    private PlayerView playerView;

    public PlayerPresenter(PlayerView view, PlayerModel model)
    {
        playerView = view;
        var playerModel = model;
        playerModel.Death += Death;
        playerModel.OnHealthChanged += RedirectValues;
        playerView.OnTakeDamge += playerModel.ResiveDemage;
        playerView.OnUpdateValues += playerModel.UpdateValues;
    }

    public void Death()
    {
        playerView.ObjectShutdown();
    }

    public void RedirectValues(int health, int Maxhealth)
    {
        playerView.HealthAnimation(health, Maxhealth);
    }
}