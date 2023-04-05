using System;
using IdleRPG.Scripts.Runtime.Models;
using IdleRPG.Scripts.Runtime.Views;

namespace IdleRPG.Scripts.Runtime.Controllers
{
    public class ProjectileController : IDisposable
    {
        private readonly MovingModel _movingModel;
        private readonly ProjectileView _projectileView;

        public ProjectileController(MovingModel movingModel, ProjectileView projectileView)
        {
            _movingModel = movingModel;
            _projectileView = projectileView;
            SubscribeEvents();
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _movingModel.OnTargetReached += _projectileView.ResolveHit;
        }

        private void UnsubscribeEvents()
        {
            _movingModel.OnTargetReached -= _projectileView.ResolveHit;
        }
    }
}
