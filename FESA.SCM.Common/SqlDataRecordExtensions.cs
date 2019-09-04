using System;
using Microsoft.SqlServer.Server;

namespace FESA.SCM.Common
{
    public static class SqlDataRecordExtensions
    {
        public static void SetNullableImage(this SqlDataRecord rec, int index, byte[] value)
        {
            if (value != null)
                rec.SetBytes(index, 0, value, 0, value.Length);
            else
                rec.SetDBNull(index);
        }

        public static void SetNullableInt32(this SqlDataRecord rec, int index, int? value)
        {
            if (value != null)
                rec.SetInt32(index, value.Value);
            else
                rec.SetDBNull(index);
        }

        public static void SetNullableString(this SqlDataRecord rec, int index, string value)
        {
            if (value != null)
                rec.SetString(index, value);
            else
                rec.SetDBNull(index);
        }

        public static void SetNullableDateTime(this SqlDataRecord rec, int index, DateTime? value)
        {
            if (value != null && value != DateTime.MinValue)
                rec.SetDateTime(index, value.Value);
            else
                rec.SetDBNull(index);
        }
    }
}