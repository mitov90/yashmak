﻿namespace Yashmak.Common.Extentions
{
    using System;

    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object obj)
        {
            var result = Activator.CreateInstance(typeof(T));

            foreach (var property in obj.GetType().GetProperties())
            {
                try
                {
                    result.GetType()
                          .GetProperty(property.Name)
                          .SetValue(result, property.GetValue(obj));
                }
                catch
                {
                    continue;
                }
            }

            return (T)result;
        }
    }
}