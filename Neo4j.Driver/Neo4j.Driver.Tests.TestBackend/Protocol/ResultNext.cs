﻿using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Neo4j.Driver;
using System.Linq;

namespace Neo4j.Driver.Tests.TestBackend
{
    internal class ResultNext : IProtocolObject
    {
        public ResultNextType data { get; set; } = new ResultNextType();
        [JsonIgnore]
        public IRecord Records { get; set; }

        public class ResultNextType
        {
            public string resultId { get; set; }
        }

        public override async Task Process()
        {
            try
            {

                var results = ((Result)ObjManager.GetObject(data.resultId)).Results;

                if (await results.FetchAsync().ConfigureAwait(false))
                    Records = results.Current;
                else
                    Records = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Process NewDriver protocol object, failed with - {ex.Message}");
            }
        }

        public override string Response()
        {
            if (!(Records is null))
            {
                //Generate list of ordered records
                var valuesList = Records.Keys.Select(v => NativeToCypher.Convert(Records[v]));
                return new Response("Record", new { values = valuesList }).Encode();
            }
            else
            {
                return new Response("NullRecord", new {}).Encode();
            }
        }
    }
}
