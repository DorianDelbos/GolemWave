namespace GolemWave
{
    public partial class Player
    {
        private PlayerInputs inputs;

        private void InitializeInputs()
        {
            inputs = new PlayerInputs();
        }

        private void EnableInputs()
        {
            inputs.Enable();

            inputs.Player.Move.performed += ReadMovement;
            inputs.Player.Move.canceled += ReadMovement;
            inputs.Player.Jump.started += ReadJump;
            inputs.Player.Attack.started += ReadShoot;
            inputs.Player.Attack.canceled += ReadShoot;
            inputs.Player.Sprint.started += StartRun;
            inputs.Player.Sprint.canceled += EndRun;
            inputs.Player.Pause.started += pauseMenu.Toggle;
        }

        private void DisableInputs()
        {
            inputs.Disable();

            inputs.Player.Move.performed -= ReadMovement;
            inputs.Player.Move.canceled -= ReadMovement;
            inputs.Player.Jump.started -= ReadJump;
            inputs.Player.Attack.started -= ReadShoot;
            inputs.Player.Attack.canceled -= ReadShoot;
            inputs.Player.Pause.started -= pauseMenu.Toggle;
        }
    }
}
