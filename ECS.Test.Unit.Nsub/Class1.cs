using System;
using NSubstitute;
using NUnit;
using NUnit.Framework;

namespace ECS.Test.Unit.Nsub
{
    [TestFixture]
    public class ECSUnitTest
    {
        private ECS _uut;
        private IHeater _heater;
        private ITempSensor _tempSensor;
        private IWindow _window;

        [SetUp]
        public void Setup()
        {
            _heater = Substitute.For<IHeater>();
            _tempSensor = Substitute.For<ITempSensor>();
            _window = Substitute.For<IWindow>();
            _uut = new ECS(_tempSensor,_heater, _window,
                -10,25);
        }

        [Test]
        public void ThreshHolds_ValidUpperTemperatureThreshHoldSet_NoExceptionsThrown()
        {
            _uut
                .When()
            
        }


    }
}
