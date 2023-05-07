using System;


namespace Tube_Walking_Guide
{
    internal static class Utils
    {

        public static Station[] ReverseArrayStation(Station[] array)
        {
            int length = array.Length;
            Station[] result = new Station[length];
            for (int i = 0; i < length - 1; i++)
            {
                result[length - 1 - i] = array[i];
            }
            return result;
        }

        public static Station[] ExtendStationArray(Station[] array)
        {
            int newLength = array.Length + 1;
            Station[] newArray = new Station[newLength];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }
            return newArray;
        }
    }
}
