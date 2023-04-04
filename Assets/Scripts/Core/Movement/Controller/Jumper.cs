using Core.Movement.Data;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _maxVerticalSize;
        private readonly Transform _transform;
        private readonly Transform _shadowTransform;
        private readonly Vector2 _shadowLocalPosition;

        private float _startJumpVerticalPos;
        private float _shadowVerticalPos;
        
        public bool IsJumping { get; private set; }
        
        public Jumper(Rigidbody2D rigidbody2D, JumpData jumpData, float maxVerticalSize)
        {
            _rigidbody = rigidbody2D;
            _jumpData = jumpData;
            _maxVerticalSize = maxVerticalSize;
            _shadowTransform = _jumpData.Shadow.transform;
            _shadowLocalPosition = _shadowTransform.localPosition;
            _transform = _rigidbody.transform;
        }
        
        public void Jump()
        {
            if (IsJumping)
                return;
            
            IsJumping = true;
            _startJumpVerticalPos = _rigidbody.position.y;
            var jumpModificator = _transform.localScale.y / _maxVerticalSize;
            var currentJumpForce = _jumpData.JumpForce * jumpModificator;
            _rigidbody.gravityScale = _jumpData.GravityScale * jumpModificator;
            _rigidbody.AddForce(Vector2.up * currentJumpForce);
            _shadowVerticalPos = _shadowTransform.position.y;
        }
        
        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _transform.position.y < _startJumpVerticalPos)
            {
                ResetJump();
                return;
            }
            
            var distance = _rigidbody.transform.position.y - _startJumpVerticalPos;
            _shadowTransform.position = new Vector2(_shadowTransform.position.x, _shadowVerticalPos);
            _shadowTransform.localScale = Vector2.one * (1 +_jumpData.ShadowSizeModificator * distance);
            _jumpData.Shadow.color = new Color(0, 0, 0, 1 - distance * _jumpData.ShadowAlphaModificator);
        }

        private void ResetJump()
        {
            _rigidbody.gravityScale = 0;
            _transform.position = new Vector2(_transform.position.x, _startJumpVerticalPos);
            
            _shadowTransform.localScale = Vector2.one;
            _shadowTransform.localPosition = _shadowLocalPosition;
            _jumpData.Shadow.color = Color.black;
            
            IsJumping = false;
        }
    }
}