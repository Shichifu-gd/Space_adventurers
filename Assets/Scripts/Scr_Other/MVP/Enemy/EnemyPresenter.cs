public class EnemyPresenter : IPresenter
{
    private EnemyView enemyView;

    public EnemyPresenter(EnemyView view, EnemyModel model)
    {
        enemyView = view;
        var enemyModel = model;
        enemyModel.Death += Death;
        enemyModel.OnHealthChanged += RedirectValues;
        enemyView.OnTakeDamge += enemyModel.ResiveDemage;
        enemyView.OnUpdateValues += enemyModel.UpdateValues;
    }

    public void Death()
    {
        enemyView.ObjectShutdown();
    }

    public void RedirectValues(int health, int maxHealth)
    {
        enemyView.HealthAnimation(health, maxHealth);
    }
}