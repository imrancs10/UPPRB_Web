using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UPPRB_Web.Infrastructure
{
    public interface ISendMessageStrategy
    {
        void SendMessages();
    }
}