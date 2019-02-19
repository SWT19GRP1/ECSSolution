using System;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
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
            _uut = new ECS(_tempSensor,_heater, _window,-10,25);
        }

        [Test]
        public void Thresholds_ValidUpperTemperatureThresholdSet_NoExceptionsThrown()
        {
            Assert.That(()=>_uut.UpperTemperatureThreshold=20,Throws.Nothing);

        }
        [Test]
        public void Thresholds_ValidLowerTemperatureThresholdSet_NoExceptionsThrown()
        {
            // Check that it doesn't throw 
            // First parameter is a lambda expression, implicitly acting
            Assert.That(() => { _uut.LowerTemperatureThreshold = 20; }, Throws.Nothing);
        }

        #region T==High

        [Test]
        public void Regulate_TempIsAtUpperThreshold_HeaterIsTurnedOff()
        {
            _tempSensor.GetTemp().Returns(_uut.UpperTemperatureThreshold);
            _uut.Regulate();
            _heater.Received(1).TurnOff();

        }

        [Test]
        public void Regulate_TempIsAtUpperThreshold_WindowIsClosed()
        {
            _tempSensor.GetTemp().Returns(_uut.UpperTemperatureThreshold);
            _uut.Regulate();
            _window.Received(1).Close();
        }

            #endregion
        #region T>High
        [Test]
        public void Regulate_TempIsAboveUpperThreshold_HeaterIsTurnedOff()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(27);
            _uut.Regulate();
            _heater.Received(1).TurnOff();
        }
        [Test]
        public void Regulate_TempIsAboveUpperThreshold_WindowIsOpened()
        {
            // Setup the stub with desired response
            _tempSensor.GetTemp().Returns(27);
            _uut.Regulate();
            _window.Received(1).Open();
        }


        #endregion
    }
}
