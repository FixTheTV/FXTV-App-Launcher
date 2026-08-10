using System;
using System.Collections.Generic;
using System.Text;

namespace FXTVGame.Backend.Network
{
    public enum ClientState
    {
        Connected,      
        Authenticated,  
        InLobby,        
        InGame          
    }
}
