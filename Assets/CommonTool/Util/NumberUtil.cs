using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class NumberUtil
{
    public static string DoubleToStr(double a)
    {
        return Math.Round(a, CommonConfig.RoundDigits).ToString();
    }
    public static string DoubleToStr(double a, int digits)
    {
        return Math.Round(a, digits).ToString();
    }

    public static double Round(double a)
    {
        return Math.Round(a, CommonConfig.RoundDigits);
    }
    
    public static double Round(int a)
    {
        return a;
    }

    public static string DecimalToStr(decimal num)
    {
        return Round(num).ToString(CultureInfo.CurrentCulture);
    }

    public static decimal Round(decimal num)
    {
        return Math.Round(num, CommonConfig.RoundDigits);
    }


}
