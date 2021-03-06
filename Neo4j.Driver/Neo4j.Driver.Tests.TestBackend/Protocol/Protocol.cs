﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Neo4j.Driver.Tests.TestBackend
{
    public static class Protocol
    {
        public enum Types
        {
            NewDriver,
            NewSession,
            AuthorizationToken,
            SessionRun,
            TransactionRun,
            Result,
            SessionReadTransaction,
            DriverClose,
            SessionClose,
            ResultNext,

            NumTypes
        }

        private static readonly Dictionary<Types, string> TypeNames = 
                    new Dictionary<Types, string> { { Types.NewDriver, "NewDriver" },
                                                    { Types.NewSession, "NewSession" },
                                                    { Types.AuthorizationToken, "AuthorizationToken" },
                                                    { Types.SessionRun, "SessionRun" },
                                                    { Types.TransactionRun, "TransactionRun" },
                                                    { Types.Result, "Result" },
                                                    { Types.SessionReadTransaction, "SessionReadTransaction" },
                                                    { Types.DriverClose, "DriverClose" },
                                                    { Types.SessionClose, "SessionClose" },
                                                    { Types.ResultNext, "ResultNext" }};

        public static string Type(Types t) { return TypeNames[t]; }
        public static Types Type(string t) { return TypeNames.First(x => x.Value == t).Key; }
        public static bool ValidType(string typeName) { return TypeNames.ContainsValue(typeName); }
        public static bool ValidType(Types t) { return TypeNames.ContainsKey(t); }

    }


    internal abstract class IProtocolObject
    {
        public string name { get; set; }
        [JsonPropertyName("id")]
        public string uniqueId { get; internal set; }    //Only exposes the get option so that the serializer will output it.  Don't want to read in on deserialization.
        protected ProtocolObjectManager ObjManager { get; set; }

        public void SetObjectManager(ProtocolObjectManager objManager)
        {
            ObjManager = objManager;
        }

        public void SetUniqueId(string id) { uniqueId = id; }

        public abstract Task Process();        

        public string Encode()      
        {
           return JsonSerializer.Serialize<object>(this); 
        }

        public virtual string Response()
        {
            return Encode();
        }

        protected async Task AysncVoidReturn()
        {
            await Task.Run(() => { });
        }
    }
}
