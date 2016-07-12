// Copyright (c) 2002-2016 "Neo Technology,"
// Network Engine for Objects in Lund AB [http://neotechnology.com]
// 
// This file is part of Neo4j.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System.Collections.Generic;
using Neo4j.Driver.Internal.Result;
using Neo4j.Driver.V1;

namespace Neo4j.Driver.Internal.Messaging
{
    internal interface IMessageRequestHandler
    {
        void HandleInitMessage(string clientNameAndVersion, IDictionary<string, object> authToken);
        void HandleRunMessage(string statement, IDictionary<string, object> parameters);
        void HandlePullAllMessage();
        void HandleDiscardAllMessage();
        void HandleResetMessage();
        void HandleAckFailureMessage();
    }

    internal interface IMessageResponseHandler
    {
        bool HasError { get; }
        Neo4jException Error { get; }
        void HandleSuccessMessage(IDictionary<string, object> meta);
        void HandleFailureMessage(string code, string message);
        void HandleIgnoredMessage();
        void HandleRecordMessage(object[] fields);
        void EnqueueMessage(IRequestMessage requestMessage, IResultBuilder resultBuilder = null);
        int UnhandledMessageSize { get; }
    }
}