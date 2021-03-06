﻿#region License
// Copyright (c) 2017 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Cbor;
using Newtonsoft.Json.Cbor.Converters;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using NUnit.Framework;
using Newtonsoft.Json.Tests.TestObjects;

namespace Newtonsoft.Json.Cbor.Tests.Converters
{
    [TestFixture]
    public class RegexConverterTests : TestFixtureBase
    {
        public class RegexTestClass
        {
            public Regex Regex { get; set; }
        }

        [Test]
        [TestCaseSource(typeof(RegexTestCases), nameof(RegexTestCases.CborToRegex))]
        public void SerializeToCbor(string expected, Regex input)
        {
            MemoryStream ms = new MemoryStream();
            CborDataWriter writer = new CborDataWriter(ms);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new CborDataRegexConverter());

            serializer.Serialize(writer, new RegexTestClass { Regex = input });

            Assert.AreEqual(expected, BytesToHex(ms.ToArray()));
        }

        [Test]
        [TestCaseSource(typeof(RegexTestCases), nameof(RegexTestCases.CborToRegex))]
        public void DeserializeFromCbor(string input, Regex expected)
        {
            MemoryStream ms = new MemoryStream(HexToBytes(input));
            CborDataReader reader = new CborDataReader(ms);
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new CborDataRegexConverter());

            var actual = serializer.Deserialize<RegexTestClass>(reader).Regex;

            Assert.AreEqual(expected.ToString(), actual.ToString());
            Assert.AreEqual(expected.Options, actual.Options);
        }

        [Test]
        public void ConvertEmptyRegexBson()
        {
            throw new NotImplementedException();
            //Regex input = new Regex(string.Empty);

            //MemoryStream ms = new MemoryStream();
            //CborDataWriter writer = new CborDataWriter(ms);
            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new CborDataRegexConverter());

            //serializer.Serialize(writer, new RegexTestClass { Regex = input });

            //ms.Seek(0, SeekOrigin.Begin);
            //CborDataReader reader = new CborDataReader(ms);
            //serializer.Converters.Add(new CborDataRegexConverter());

            //RegexTestClass c = serializer.Deserialize<RegexTestClass>(reader);

            //Assert.AreEqual("", c.Regex.ToString());
            //Assert.AreEqual(RegexOptions.None, c.Regex.Options);
        }

        [Test]
        public void ConvertRegexWithAllOptionsBson()
        {
            throw new NotImplementedException();
            //Regex input = new Regex(
            //    "/",
            //    RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.ExplicitCapture);

            //MemoryStream ms = new MemoryStream();
            //CborDataWriter writer = new CborDataWriter(ms);
            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new CborDataRegexConverter());

            //serializer.Serialize(writer, new RegexTestClass { Regex = input });

            //string expected = "14-00-00-00-0B-52-65-67-65-78-00-2F-00-69-6D-73-75-78-00-00";
            //string bson = BytesToHex(ms.ToArray());

            //Assert.AreEqual(expected, bson);

            //ms.Seek(0, SeekOrigin.Begin);
            //CborDataReader reader = new CborDataReader(ms);
            //serializer.Converters.Add(new CborDataRegexConverter());

            //RegexTestClass c = serializer.Deserialize<RegexTestClass>(reader);

            //Assert.AreEqual("/", c.Regex.ToString());
            //Assert.AreEqual(RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.ExplicitCapture, c.Regex.Options);
        }
    }

    public class RegexTestCases
    {
        public static IEnumerable<TestCaseData> CborToRegex
        {
            get
            {
                yield return new TestCaseData(
                    "A1-65-52-65-67-65-78-D8-23-66-2F-61-62-63-2F-75",
                    new Regex("abc", RegexOptions.None));
                yield return new TestCaseData(
                    "A1-65-52-65-67-65-78-D8-23-67-2F-61-62-63-2F-69-75",
                    new Regex("abc", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
                yield return new TestCaseData(
                    "A1-65-52-65-67-65-78-D8-23-78-19-2F-5E-5B-61-2D-7A-5D-2B-5C-5B-5B-30-2D-39-5D-2B-5C-5D-24-2F-69-6D-73-75-78",
                    new Regex(@"^[a-z]+\[[0-9]+\]$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.ExplicitCapture));
                yield return new TestCaseData(
                    "A1-65-52-65-67-65-78-D8-23-76-2F-5E-5B-61-2D-7A-5D-2B-5C-5B-5B-30-2D-39-5D-2B-5C-5D-24-2F-69-6D",
                    new Regex(@"^[a-z]+\[[0-9]+\]$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ECMAScript));
                yield return new TestCaseData(
                    "A1-65-52-65-67-65-78-D8-23-72-5E-5B-61-2D-7A-5D-2B-5C-5B-5B-30-2D-39-5D-2B-5C-5D-24",
                    new Regex(@"^[a-z]+\[[0-9]+\]$", RegexOptions.ECMAScript));
            }
        }
    }
}