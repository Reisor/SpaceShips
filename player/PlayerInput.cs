
using System;

using Sce.PlayStation.Core.Input;

using API.Framework;
using API;

namespace SpaceShips
{
	public class PlayerInput : GameActor
	{
		public PlayerInput(Game gs, string name) : base (gs, name)
		{}

		private static float FilterAnalogValue( float value, float deadzone )
    	{
            float sign = ( value > 0.0f ? 1.0f : -1.0f );
    		value *= sign;

    		if ( value < deadzone ) return 0.0f;
    		else return sign * ( value - deadzone ) / ( 1.0f - deadzone );
    	}

		public float LeftRightAxis()
		{
			if((gs.PadData.Buttons & GamePadButtons.Left) != 0)
				return -1.0f;

			if((gs.PadData.Buttons & GamePadButtons.Right) != 0)
				return 1.0f;

			if (gs.PadData.AnalogLeftX != 0)
				return FilterAnalogValue( gs.PadData.AnalogLeftX, 0.08f );
			else
				return FilterAnalogValue( gs.PadData.AnalogRightX, 0.08f );
		}

		public float UpDownAxis()
		{
			GamePadData data = GamePad.GetData(0);

			if((gs.PadData.Buttons & GamePadButtons.Up) != 0)
				return -1.0f;

			if((gs.PadData.Buttons & GamePadButtons.Down) != 0)
				return 1.0f;

			if (gs.PadData.AnalogLeftX != 0)
				return FilterAnalogValue( gs.PadData.AnalogLeftY, 0.08f );
			else
				return FilterAnalogValue( gs.PadData.AnalogRightY, 0.08f );
		}
		
		public bool BombButton()
		{
			GamePadData data = GamePad.GetData(0);

			if((gs.PadData.ButtonsDown & GamePadButtons.Cross) != 0)
				return true;

			return false;
		}

		public bool AttackButton()
		{
			GamePadData data = GamePad.GetData(0);

			if((gs.PadData.ButtonsDown & GamePadButtons.Square) != 0)
				return true;

			return false;
		}

		public bool SpecialButton()
        {
			GamePadData data = GamePad.GetData(0);

			if((gs.PadData.ButtonsDown & GamePadButtons.Triangle) != 0)
				return true;

			return false;
		}

		public bool SelectButton()
        {
			GamePadData data = GamePad.GetData(0);

			if((data.ButtonsDown & GamePadButtons.Select) != 0)
				return true;

			return false;
		}

		public bool StartButton()
        {
			GamePadData data = GamePad.GetData(0);

			if ((gs.PadData.ButtonsDown & GamePadButtons.Start) != 0)
				return true;

			return false;
		}

		public bool AnyButton()
		{
			return
				AttackButton() ||
				BombButton() ||
				SpecialButton() ||
				SelectButton() ||
				StartButton();
		}
	}
}

