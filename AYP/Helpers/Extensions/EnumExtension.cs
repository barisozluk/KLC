﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AYP.Helpers.Extensions
{
    public static class EnumExtension
    {
        public static string Name(this Enum enumType)
        {
            return Enum.GetName(enumType.GetType(), enumType);
        }


    }
}
