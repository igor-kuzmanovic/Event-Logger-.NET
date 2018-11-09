﻿using System.ServiceModel;

namespace IDSServiceCommon
{
    [ServiceContract]
    public interface IIDSService
    {
        [OperationContract]
        void Alarm(string message);
    }
}
