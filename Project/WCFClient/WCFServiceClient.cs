﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using WCFServiceCommon;

namespace WCFClient
{
    internal class WCFServiceClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        private readonly IWCFService channel;

        public WCFServiceClient() : this("WCFService_Endpoint") { }

        public WCFServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            channel = CreateChannel();
        }

        public byte[] CheckIn()
        {
            byte[] result = null;
            try
            {
                result = channel.CheckIn();
            }
            catch (FaultException e)
            {
                Console.WriteLine("[CheckIn] ERROR = {0}", e.Message);
            }
            return result;
        }

        public void Add(string content)
        {
            try
            {
                channel.Add(content);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Add] ERROR = {0}", e.Message);
            }
        }

        public bool Update(int entryID, string content)
        {
            bool result = false;
            try
            {
                result = channel.Update(entryID, content);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Update] ERROR = {0}", e.Message);
            }
            return result;
        }

        public bool Delete(int entryID)
        {
            bool result = false;
            try
            {
                result = channel.Delete(entryID);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Delete] ERROR = {0}", e.Message);
            }
            return result;
        }

        public EventEntry Read(int entryID, byte[] key)
        {
            EventEntry result = null;
            try
            {
                result = channel.Read(entryID, key);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[Read] ERROR = {0}", e.Message);
            }
            return result;
        }

        public HashSet<EventEntry> ReadAll(byte[] key)
        {
            HashSet<EventEntry> result = null;
            try
            {
                result = channel.ReadAll(key);
            }
            catch (FaultException e)
            {
                Console.WriteLine("[ReadAll] ERROR = {0}", e.Message);
            }
            return result;
        }

        public void Dispose()
        {
            try
            {
                if (State != CommunicationState.Faulted)
                {
                    Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                if (State != CommunicationState.Closed)
                {
                    Abort();
                }
            }
        }
    }
}
