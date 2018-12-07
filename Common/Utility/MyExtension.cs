using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Common.Utility
{
    public static class MyExtension
    {
        public static void CopyPropertiesTo<T, U>(this T source, U dest)
        {
            if (source == null)
                return;

            var plistsource = from prop1 in typeof(T).GetProperties() where prop1.CanRead select prop1;
            var plistdest = from prop2 in typeof(U).GetProperties() where prop2.CanWrite select prop2;

            foreach (PropertyInfo destprop in plistdest)
            {
                //if not nullable, not an enum, not primitive then skip.
                if (Nullable.GetUnderlyingType(destprop.PropertyType) == null && !destprop.PropertyType.IsEnum
                    && !IsPrimitive(destprop.PropertyType))//|| !Nullable.GetUnderlyingType(destprop.PropertyType).IsEnum)
                    continue;

                var sourceprops = plistsource.Where((p) => p.Name == destprop.Name);
                //&& destprop.PropertyType.IsAssignableFrom(p.GetType()));               
                foreach (PropertyInfo sourceprop in sourceprops)
                {
                    //if the source is not primitive type skip
                    if (Nullable.GetUnderlyingType(sourceprop.PropertyType) == null && !sourceprop.PropertyType.IsEnum
                    && !IsPrimitive(sourceprop.PropertyType))
                        continue;


                    // should only be one
                    if (sourceprop.PropertyType == typeof(DateTime))
                    {
                        var dt = sourceprop.GetValue(source, null);
                        if (dt != null && !string.IsNullOrEmpty(Convert.ToString(dt)))
                        {
                            if (DateTime.Parse(dt.ToString()).Year < 1753 || DateTime.Parse(dt.ToString()).Year > 9999)
                            {
                                continue;
                            }
                        }
                    }
                    if (sourceprop.PropertyType == typeof(DateTime?))
                    {
                        var dt = sourceprop.GetValue(source, null);
                        if (dt != null && !string.IsNullOrEmpty(Convert.ToString(dt)))
                        {
                            if (DateTime.Parse(dt.ToString()).Year < 1753 || DateTime.Parse(dt.ToString()).Year > 9999)
                            {
                                continue;
                            }
                        }
                    }

                    destprop.SetValue(dest, sourceprop.GetValue(source, null), null);
                }
            }
        }

        private static bool IsPrimitive(Type t)
        {
            // TODO: put any type here that you consider as primitive as I didn't
            // quite understand what your definition of primitive type is
            return new[] {
            typeof(string),typeof(bool),
            typeof(char),typeof(char?),
            typeof(byte),typeof(byte?),
            typeof(sbyte),typeof(sbyte?),
            typeof(ushort),typeof(ushort?),
            typeof(short),typeof(short?),
            typeof(uint),typeof(uint?),
            typeof(int),typeof(int?),
            typeof(ulong),typeof(ulong?),
            typeof(long),typeof(long?),
            typeof(float),typeof(float?),
            typeof(double),typeof(double?),
            typeof(decimal),typeof(decimal?),
            typeof(DateTime),typeof(DateTime?),
        }.Contains(t);
        }


        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string ToFormmatedDate(this string item)
        {
            if (string.IsNullOrEmpty(item))
                return "";

            DateTime _date;
            string[] _dateformats = { "dd/MM/yyyy", "yyyy-MM-dd", "MMM dd yyyy hh:mmtt", "MMM  d yyyy hh:mmtt", "MMM  d yyyy  h:mmtt" };


            item = item.Split(new string[] { "T" }, StringSplitOptions.None)[0];

            if (DateTime.TryParseExact(item, _dateformats, new CultureInfo("en-US"), DateTimeStyles.None, out _date))
            {
                return _date.ToString("dd-MMM-yyyy");
            }
            else
            {
                return item;
            }
        }
    }

    public static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string)) return true;
            return (type.IsValueType & type.IsPrimitive);
        }

        private static object Copy(this object originalObject)
        {
            return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }
        private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
        {
            if (originalObject == null)
                return null;
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect))
                return originalObject;
            if (visited.ContainsKey(originalObject))
                return visited[originalObject];
            if (typeof(Delegate).IsAssignableFrom(typeToReflect))
                return null;
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false)
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if (filter != null && filter(fieldInfo) == false)
                    continue;
                if (IsPrimitive(fieldInfo.FieldType))
                    continue;
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        public static T Copy<T>(this T original)
        {
            return (T)Copy((object)original);
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<Object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    //namespace ArrayExtensions
    //{
    public static class ArrayExtensions
    {
        public static void ForEach(this Array array, Action<Array, int[]> action)
        {
            if (array.LongLength == 0) return;
            ArrayTraverse walker = new ArrayTraverse(array);
            do action(array, walker.Position);
            while (walker.Step());
        }
    }

    internal class ArrayTraverse
    {
        public int[] Position;
        private int[] maxLengths;

        public ArrayTraverse(Array array)
        {
            maxLengths = new int[array.Rank];
            for (int i = 0; i < array.Rank; ++i)
            {
                maxLengths[i] = array.GetLength(i) - 1;
            }
            Position = new int[array.Rank];
        }

        public bool Step()
        {
            for (int i = 0; i < Position.Length; ++i)
            {
                if (Position[i] < maxLengths[i])
                {
                    Position[i]++;
                    for (int j = 0; j < i; j++)
                    {
                        Position[j] = 0;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
