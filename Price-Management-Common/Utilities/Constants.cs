using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_Management_Common.Utilities
{
    public class Constants
    {
        public const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const int LargeIntegerValue = 100000000;

        public enum Vehiclepricepercentages
        {
            bicycle = 10,
            motorbike = 15,
            parcel_car = 20,
            small_van = 30,
            large_van = 40,
        }
    }
}
