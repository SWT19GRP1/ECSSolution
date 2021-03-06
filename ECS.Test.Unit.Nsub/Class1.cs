﻿using System;
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
            Assert.That(() => _uut.UpperTemperatureThreshold = 25, Throws.Nothing);
        }

        [Test]
        public void Regulate_TempIOsLow_HeaterIsTurnedOn()
        {
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold - 10);
            _uut.Regulate();
            _heater.Received(1).TurnOn();
        }

        [Test]
        public void Regulate_TempIsLow_WindowIsClosed()
        {
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold - 10);
            _uut.Regulate();
            _window.Received(1).Close();
        }

        [Test]
        public void Regulate_TempIsAtLowerThresh_HeaterIsTurnedOff()
        {
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold);
            _uut.Regulate();
            _heater.Received(1).TurnOff();
        }

        [Test]
        public void RegulateTempIsAtLowerThresh_WindowIsClosed()
        {
            _tempSensor.GetTemp().Returns(_uut.LowerTemperatureThreshold);
            _uut.Regulate();
            _window.Received(1).Close();
        }

        [Test]
        public void Regulate_TempIsBetweenLowerAndUpperThresh_HeaterIsTurnedOff()
        {
            _tempSensor.GetTemp().Returns(_uut.UpperTemperatureThreshold - 5);
            _uut.Regulate();
            _heater.Received(0).TurnOn();
        }
        [Test]
        public void Regulate_TempIsBetweenLoweAndUpperThresh_WindowIsNotOpen()
        {
            _tempSensor.GetTemp().Returns(_uut.UpperTemperatureThreshold - 5);
            _uut.Regulate();
            _window.Received(0).Open();
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
