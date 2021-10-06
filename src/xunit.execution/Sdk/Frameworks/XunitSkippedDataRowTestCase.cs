﻿using System;
using System.ComponentModel;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
    /// <summary>
    /// Represents a test case that had a valid data row, but the data row was generated by a data attribute with the skip property set.
    /// </summary>
    /// <remarks>This class is only ever used if the discoverer is pre-enumerating theories and the data row is serializable.</remarks>
    public class XunitSkippedDataRowTestCase : XunitTestCase
    {
        string skipReason;

        /// <summary/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public XunitSkippedDataRowTestCase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitSkippedDataRowTestCase"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        /// <param name="defaultMethodDisplay">Default method display to use (when not customized).</param>
        /// <param name="testMethod">The test method this test case belongs to.</param>
        /// <param name="skipReason">The reason that this test case will be skipped</param>
        /// <param name="testMethodArguments">The arguments for the test method.</param>
        [Obsolete("Please call the constructor which takes TestMethodDisplayOptions")]
        public XunitSkippedDataRowTestCase(IMessageSink diagnosticMessageSink,
                                           TestMethodDisplay defaultMethodDisplay,
                                           ITestMethod testMethod,
                                           string skipReason,
                                           object[] testMethodArguments = null)
            : this(diagnosticMessageSink, defaultMethodDisplay, TestMethodDisplayOptions.None, testMethod, skipReason, testMethodArguments) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitSkippedDataRowTestCase"/> class.
        /// </summary>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages</param>
        /// <param name="defaultMethodDisplay">Default method display to use (when not customized).</param>
        /// <param name="defaultMethodDisplayOptions">Default method display options to use (when not customized).</param>
        /// <param name="testMethod">The test method this test case belongs to.</param>
        /// <param name="skipReason">The reason that this test case will be skipped</param>
        /// <param name="testMethodArguments">The arguments for the test method.</param>
        public XunitSkippedDataRowTestCase(IMessageSink diagnosticMessageSink,
                                           TestMethodDisplay defaultMethodDisplay,
                                           TestMethodDisplayOptions defaultMethodDisplayOptions,
                                           ITestMethod testMethod,
                                           string skipReason,
                                           object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
        {
            this.skipReason = skipReason;
        }

        /// <inheritdoc/>
        public override void Deserialize(IXunitSerializationInfo data)
        {
            // This happens before base.Deserialize because the base method will call our overridden GetSkipReason
            // method, so we need the skip reason value before we delegate into deserialization.
            skipReason = data.GetValue<string>("SkipReason");

            base.Deserialize(data);
        }

        /// <inheritdoc/>
        protected override string GetSkipReason(IAttributeInfo factAttribute)
        {
            return skipReason;
        }

        /// <inheritdoc/>
        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue("SkipReason", skipReason);
        }
    }
}
