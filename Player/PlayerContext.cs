using _Scripts.Scriptables.Input;
using _Scripts.Scriptables.Player;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerContext
    {
        #region References

        public PlayerMotor PlayerMotor;
        public MovementConfig Config;

        #endregion
        
        #region Input

        public Vector2 MoveInput;
        public bool DashInput;
        public bool WallGrabInput;

        #endregion

        #region Physics

        public Vector2 Velocity;
        public float GravityScale;

        #endregion

        #region Scenario

        public bool OnGround;
        public bool OnWall;
        public bool OnLeftWall;
        public bool OnRightWall;

        #endregion

        #region State Specific

        public bool CanJump = true;

        #endregion
    }
}
