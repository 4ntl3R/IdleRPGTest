using System;
using IdleRPG.Scripts.Runtime.Interfaces;
using IdleRPG.Scripts.Runtime.Models;
using UnityEngine;

namespace IdleRPG.Scripts.Runtime.Controllers
{
    public class MovingController: IDisposable
    {
        private readonly MovingModel _movingModel;
        private readonly IMovable _movable;
        private readonly IIterator _iterator;

        public MovingController(MovingModel movingModel, IMovable movable, IIterator iterator)
        {
            _movingModel = movingModel;
            _movable = movable;
            
            SubscribeEvents();
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _movingModel.OnPositionChanged += _movable.MoveToPosition;
            _movingModel.OnTargetReached += _iterator.StopIterating;
            _iterator.OnIterate += _movingModel.MovingIteration;
            _movable.OnResetPositions += ResolveReset;
        }

        private void UnsubscribeEvents()
        {
            _movingModel.OnPositionChanged -= _movable.MoveToPosition;
            _movingModel.OnTargetReached -= _iterator.StopIterating;
            _iterator.OnIterate -= _movingModel.MovingIteration;
            _movable.OnResetPositions -= ResolveReset;
        }
        
        private void ResolveReset(Vector3 start, Vector3 end)
        {
            _movingModel.SetPositions(start, end);
            _movable.LookAt(end);
            _iterator.StartIterating();
        }
    }
}
