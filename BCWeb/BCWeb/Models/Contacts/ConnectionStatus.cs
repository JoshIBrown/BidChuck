﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BCWeb.Models.Contacts
{
    public enum ConnectionStatus
    {
        Connected,
        InvitationSent,
        InvitationPending,
        BlackListed,
        NotConnected,
        Self
    }
}