using _Scripts.Player.Layers;
using _Scripts.Scriptables.Input;
using _Scripts.Scriptables.Player;
using UnityEngine;
using System;

namespace _Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor : MonoBehaviour
    {
        #region References
        
        [SerializeField] private InputReader inputReader;
        [SerializeField] private MovementConfig configSO;
        private PlayerContext _context = new();
        private Rigidbody2D _rb;

        #endregion
        [Space(10)]

        #region Checks

        [Header("Checks")]
        [SerializeField] private GroundCheck groundCheck;
        [SerializeField] private WallCheck wallCheck;

        #endregion

        private OverrideFSM _overrideFSM;
        private LocomotionFSM _locomotionFSM;
        private VerticalFSM _verticalFSM;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            
            #region Context

            _context.PlayerMotor = this;
            _context.Config = configSO;

            #endregion

            #region Movement Layers
            
            _locomotionFSM = new LocomotionFSM(_context);
            _verticalFSM = new VerticalFSM(_context);
            _overrideFSM = new OverrideFSM(_context);

            #endregion
        }

        private void Update()
        {
            #region Input
            
            _context.MoveInput = inputReader.Get<Vector2>(InputActions.Move);
            _context.DashInput = inputReader.IsHeld(InputActions.Dash);
            _context.WallGrabInput = inputReader.IsHeld(InputActions.WallGrab);

            #endregion

            #region Physics

            _rb.linearVelocityY = Mathf.Clamp(_rb.linearVelocityY, -configSO.MaxFallSpeed, float.MaxValue);
            _context.Velocity = _rb.linearVelocity;
            _context.GravityScale = _rb.gravityScale;

            #endregion

            #region Scenarios

            _context.OnGround = groundCheck.OnGround(transform);
            _context.OnWall = wallCheck.OnWall(transform, out var leftWall, out var rightWall);
            _context.OnLeftWall = leftWall;
            _context.OnRightWall = rightWall;

            #endregion
            
            _overrideFSM.FrameTick();
            if (_overrideFSM.IsActive) return;
            
            _locomotionFSM.FrameTick();
            _verticalFSM.FrameTick();
        }

        private void FixedUpdate()
        {
            _overrideFSM.PhysicsTick();
            if (_overrideFSM.IsActive) return;
            
            _locomotionFSM.PhysicsTick();
            _verticalFSM.PhysicsTick();
        }
        
        #region Physics Functions
        
        public void AddForce(Vector2 force, bool useImpulse) => _rb.AddForce(force, useImpulse ? ForceMode2D.Impulse : ForceMode2D.Force);
        public void OverrideVelocity(Vector2 newVelocity) => _rb.linearVelocity = newVelocity;
        public void SetGravityScale(float newScale) => _rb.gravityScale = newScale;

        #endregion
    }

    #region Check Classes
    
    [Serializable]
    public class GroundCheck
    {
        [SerializeField] private Vector2 boxSize;
        [SerializeField] private float boxDistance;
        [SerializeField] private LayerMask layersToIgnore;

        public Vector2 BoxSize => boxSize;
        public float BoxDistance => boxDistance;
        
        public bool OnGround(Transform transform) => 
            Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, boxDistance,  ~layersToIgnore);
    }

    [Serializable]
    public class WallCheck
    {
        [SerializeField] private Vector2 boxSize;
        [SerializeField] private float boxDistance;
        [SerializeField] private LayerMask layersToIgnore;

        public Vector2 BoxSize => boxSize;
        public float BoxDistance => boxDistance;

        public bool OnWall(Transform transform, out bool leftWall, out bool rightWall)
        {
            leftWall = Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.right, boxDistance,  (LayerMask) ~layersToIgnore);
            rightWall = Physics2D.BoxCast(transform.position, boxSize, 0f, transform.right, boxDistance,  (LayerMask) ~layersToIgnore);
            return leftWall || rightWall;
        }
    }
    
    #endregion
}