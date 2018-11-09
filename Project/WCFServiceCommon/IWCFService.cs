﻿using System.Collections.Generic;
using System.ServiceModel;

namespace WCFServiceCommon
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        byte[] CheckIn();

        [OperationContract]
        void Add(string entry);

        [OperationContract]
        void Update(int entryId, string entry);

        [OperationContract]
        void Delete(int entryId);

        [OperationContract]
        object Read(int entryId, byte[] key);

        [OperationContract]
        HashSet<object> ReadAll(byte[] key);
    }
}
