﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;

namespace rtc_shim_iotcore
{
    /// <summary>
    /// Used to comunicate to piFace RTC shim.
    /// Exposes methods to get and set the clock.
    /// First you have to call Initialize, then you can use GetTime and SetTime and also control connectivity with IsConnected
    /// </summary>
    public class MCP7940N
    {
        public static DateTime FailedDateTimeRead0 = new DateTime(2002, 2, 2);
        public static DateTime FailedDateTimeRead1 = new DateTime(2003, 3, 3);
        public static DateTime FailedDateTimeRead2 = new DateTime(2004, 4, 4);
        public static bool Initialized = false;
        private static MCP7940N _instance;
        bool Connected { get; set; }
        private I2cDevice Slave { get; set; }

        private byte I2C_ADDRESS = 0x6f;
        public static string CHANNEL = null;
        private string I2C_CHANNEL;
        public static MCP7940N Instance()
        {
            if (_instance == null)
            {
                if (CHANNEL == null)
                {
                    _instance = new MCP7940N();
                }
                else
                {
                    _instance = new MCP7940N(CHANNEL);
                }
            }
            return _instance;
        }

        private MCP7940N(string i2c_channel = "I2C1")
        {
            this.I2C_CHANNEL = i2c_channel;
        }

        public byte GetI2cAddress()
        {
            return I2C_ADDRESS;
        }

        public async Task Initialize()
        {
            if (Initialized && Connected)
                return;
            var DIS = await DeviceInformation.FindAllAsync(I2cDevice.GetDeviceSelector(I2C_CHANNEL));
            if (!DIS.Any())
            {
                throw new Exception("No I2C connected.");
            }

            var i2cSettings = new I2cConnectionSettings(this.GetI2cAddress());
            i2cSettings.BusSpeed = I2cBusSpeed.FastMode;
            var i2cDevice = await I2cDevice.FromIdAsync(DIS[0].Id, i2cSettings);
            this.Slave = i2cDevice ?? throw new Exception("device " + i2cSettings.SlaveAddress + " at ic " + DIS[0].Id + " unavailable");
            Initialized = true;
            Connected = true;
        }

        public DateTime GetTime()
        {
            byte[] readBuffer = new byte[7];
            try
            {
                this.Slave.WriteRead(new byte[] { 0x00 }, readBuffer);
            }
            catch (Exception)
            {
                return FailedDateTimeRead0;
            }

            return BufferToDateTime(readBuffer);
        }
        public void SetTime(DateTime dateTime)
        {
            byte[] writeBuffer = DateTime2Byte(dateTime);
            this.Slave.Write(writeBuffer);
        }

        private byte bcd2dec(byte val)
        {
            return (byte)(((int)val / 16 * 10) + ((int)val % 16));
        }

        private byte byte2bcd(int val)
        {
            return (byte)(((int)val / 10 * 16) + ((int)val % 10));
        }
        private DateTime BufferToDateTime(byte[] dateTimeBuffer)
        {
            //set 7th bit to zero
            byte sec_mask = 0x7f;
            byte second = bcd2dec((byte)(dateTimeBuffer[0] & sec_mask));
            var minute = bcd2dec(dateTimeBuffer[1]);
            var hour = bcd2dec(dateTimeBuffer[2]);
            var dayofWeek = bcd2dec(dateTimeBuffer[3]) & 7;
            var day = bcd2dec(dateTimeBuffer[4]);
            var month = bcd2dec((byte)(dateTimeBuffer[5] & 31));
            var year = 2000 + bcd2dec(dateTimeBuffer[6]);
            if (year == 0 || month == 0 || day == 0)
            {
                return FailedDateTimeRead1;
            }
            if (month > 12 || day > 31 || hour > 23 || minute > 59 || second > 59)
            {
                return FailedDateTimeRead2;
            }
            return new DateTime(year, month, day, hour, minute, second);
        }

        private byte[] DateTime2Byte(DateTime dateTime)
        {
            var dateArray = new byte[9];
            bool leap_year = dateTime.Year % 4 == 0;
            dateArray[0] = 0;
            //if we don't set 7-th bit to 1, clock won't start
            dateArray[1] = (byte)(byte2bcd(Convert.ToByte(dateTime.Second)) | 128);
            //dateArray[1] = byte2bcd(Convert.ToByte(dateTime.Second));
            dateArray[2] = byte2bcd(Convert.ToByte(dateTime.Minute));
            dateArray[3] = byte2bcd(Convert.ToByte(dateTime.Hour));
            dateArray[4] = byte2bcd(Convert.ToByte(dateTime.DayOfWeek) | 32);
            dateArray[5] = byte2bcd(Convert.ToByte(dateTime.Day));
            dateArray[6] = byte2bcd(Convert.ToByte(leap_year ? dateTime.Month + 32 : dateTime.Month));
            dateArray[7] = byte2bcd(Convert.ToByte(dateTime.Year - 2000));
            dateArray[8] = 0;

            return dateArray;
        }
        public bool IsConnected()
        {
            return Connected;
        }
    }
}