using System;

namespace DotsBT
{
    public struct BTNodeComparisionBaseFunc
    {
        public static bool CompareValue<T>(EMathOperation operation , T value, T target, T min, T max) where T : unmanaged,IComparable<T>
        {
            switch (operation)
            {
                default:
                    return false;
                case EMathOperation.LessThan:
                    return value.CompareTo(target) < 0;
                case EMathOperation.LessThanOrEqualTo:
                    return value.CompareTo(target) <= 0;
                case EMathOperation.EqualTo:
                    return value.CompareTo(target) == 0;
                case EMathOperation.NotEqualTo:
                    return value.CompareTo(target) != 0;
                case EMathOperation.GreaterThanOrEqualTo:
                    return value.CompareTo(target) >= 0;
                case EMathOperation.GreaterThan:
                    return value.CompareTo(target) > 0;
                case EMathOperation.BetweenAndExcludeBoth:
                    return value.CompareTo(min) > 0 && value.CompareTo(max) < 0;
                case EMathOperation.BetweenAndIncludeLeft:
                    return value.CompareTo(min) >= 0 && value.CompareTo(max) < 0;
                case EMathOperation.BetweenAndIncludeRight:
                    return value.CompareTo(min) > 0 && value.CompareTo(max) <= 0;
                case EMathOperation.BetweenAndIncludeBoth:
                    return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
            }
        }
    }
}