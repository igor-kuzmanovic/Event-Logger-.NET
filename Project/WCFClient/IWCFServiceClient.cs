﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFServiceCommon;

namespace WCFClient
{
    internal interface IWCFServiceClient : IWCFService, IDisposable
    {

    }
}
