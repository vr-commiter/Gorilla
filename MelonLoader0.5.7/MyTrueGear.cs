using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;
using TrueGearSDK;


namespace MyTrueGear
{
    public class TrueGearMod
    {
        private static TrueGearPlayer _player = null;

        public TrueGearMod() 
        {
            _player = new TrueGearPlayer("1533390","GorillaTag");
            _player.Start();
        }    

       

        public void Play(string Event)
        { 
            _player.SendPlay(Event);
        }

    }
}
