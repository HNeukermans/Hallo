using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Hallo.Sip;
using Hallo.UnitTest.Builders;
using NUnit.Framework;

namespace Hallo.UnitTest.Sip
{
    [TestFixture]
    public class SipValidatorTests
    {

        [Test]
        public void ValidateMessage_WithValidRequest_ExpectResultIsValidIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var result = v.ValidateMessage(new SipRequestBuilder().Build());

            result.IsValid.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithoutViaHeaders_ExpectResultIsValidIsFalseAndHasRequiredHeadersMissingIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var request = new SipRequestBuilder().WithVias(null).Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasRequiredHeadersMissing.Should().BeTrue();
        }

        private static SipValidator CreateSipValidator()
        {
            return new SipValidator();
        }

        [Test]
        public void ValidateMessage_WithoutCallIdHeaders_ExpectResultIsFalseAndHasRequiredHeadersMissingIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var request = new SipRequestBuilder().WithCallId(null).Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasRequiredHeadersMissing.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithoutFromHeader_ExpectResultIsFalseAndHasRequiredHeadersMissingIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var request = new SipRequestBuilder().WithFrom(null).Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasRequiredHeadersMissing.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithoutToHeader_ExpectResultIsFalseAndHasRequiredHeadersMissingIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var request = new SipRequestBuilder().WithTo(null).Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasRequiredHeadersMissing.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithoutCSeqHeader_ExpectResultIsFalseAndHasRequiredHeadersMissingIsTrue()
        {
            SipValidator v =CreateSipValidator();
            var request = new SipRequestBuilder().WithCSeq(null).Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasRequiredHeadersMissing.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithoutMaxForwardsHeader_ExpectResultIsFalseAndHasRequiredHeadersMissingIsTrue()
        {
            SipValidator v =CreateSipValidator();
            var request = new SipRequestBuilder().WithMaxForwards(null).Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasRequiredHeadersMissing.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithCSeqMethodDifferentFromRequestLineMethod_ExpectResultIsFalseAndHasInvalidSCeqMethodIsTrue()
        {
            SipValidator v =CreateSipValidator();
            var request = new SipRequestBuilder()
                .WithRequestLine(new SipRequestLineBuilder().WithMethod(SipMethods.Register).Build())
                .WithCSeq(new SipCSeqHeaderBuilder().WithCommand(SipMethods.Invite).Build())
                .Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            (result as ValidateRequestResult).Should().NotBeNull();
            (result as ValidateRequestResult).HasInvalidSCeqMethod.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithRequestLineVersionSipThreeDotZero_ExpectResultIsFalseAndHasUnSupportedSipVersionIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var request = new SipRequestBuilder()
                .WithRequestLine(new SipRequestLineBuilder().WithVersion("SIP/3.0").Build())
                .Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeFalse();
            result.HasUnSupportedSipVersion.Should().BeTrue();
        }

        [Test]
        public void ValidateMessage_WithRequestLineVersionSipTwoDotZero_ExpectResultIsTrue()
        {
            SipValidator v = CreateSipValidator();
            var request = new SipRequestBuilder()
                .WithRequestLine(new SipRequestLineBuilder().WithVersion("SIP/2.0").Build())
                .Build();
            var result = v.ValidateMessage(request);

            result.IsValid.Should().BeTrue();
        }
    }
}
