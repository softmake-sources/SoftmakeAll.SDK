using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.DataAccess
{
  public static class DatabaseValues
  {
    #region Methods
    public static System.String GetNullableString(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToString(Value);
    }
    public static System.Int16? GetNullableInt16(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToInt16(Value);
    }
    public static System.Int32? GetNullableInt32(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToInt32(Value);
    }
    public static System.Int64? GetNullableInt64(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToInt64(Value);
    }
    public static System.Decimal? GetNullableDecimal(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToDecimal(Value);
    }
    public static System.Boolean? GetNullableBoolean(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToBoolean(Value);
    }
    public static System.DateTime? GetNullableDateTime(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return System.Convert.ToDateTime(Value);
    }
    public static System.DateTimeOffset? GetNullableDateTimeOffset(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return (System.DateTimeOffset)Value;
    }
    public static System.Byte[] GetNullableByteArray(System.Object Value)
    {
      if ((Value == null) || (System.Convert.IsDBNull(Value)))
        return null;
      else
        return ((System.Byte[])Value);
    }
    public static System.Guid GetNullableGUID(System.Object Value)
    {
      System.Guid Result = System.Guid.Empty;

      if ((Value != null) && (!(System.Convert.IsDBNull(Value))))
        System.Guid.TryParse(System.Convert.ToString(Value), out Result);

      return Result;
    }

    public static System.String GetCorrectVARCHARValue(System.String String)
    {
      if (String == null)
        return "NULL";

      return $"'{String.Replace("'", "''")}'";
    }
    public static System.String GetCorrectNVARCHARValue(System.String String)
    {
      if (String == null)
        return "NULL";

      return $"N'{String.Replace("'", "''")}'";
    }
    public static System.String GetCorrectColumnValue(System.Text.Json.JsonElement JsonElement)
    {
      switch (JsonElement.ValueKind)
      {
        case System.Text.Json.JsonValueKind.Array:
        case System.Text.Json.JsonValueKind.Object:
          return System.String.Concat("'", JsonElement.GetRawText().Replace("'", "''"), "'");

        case System.Text.Json.JsonValueKind.String:
          return System.String.Concat("'", JsonElement.GetString().Replace("'", "''"), "'");

        case System.Text.Json.JsonValueKind.True:
        case System.Text.Json.JsonValueKind.False:
          return System.Convert.ToInt32(JsonElement.GetBoolean()).ToString();

        case System.Text.Json.JsonValueKind.Number:
          return JsonElement.GetRawText();

        default:
          return "NULL";
      }
    }

    public static System.Byte[] HexStringToByteArray(System.String String)
    {
      if (System.String.IsNullOrWhiteSpace(String))
        return null;

      return Enumerable.Range(0, String.Length).Where(x => x % 2 == 0).Select(x => System.Convert.ToByte(String.Substring(x, 2), 16)).ToArray();
    }
    public static System.String ByteArrayToHexString(System.Byte[] ByteArray)
    {
      if ((ByteArray == null) || (!(ByteArray.Any())))
        return null;

      return System.BitConverter.ToString(ByteArray).Replace("-", "");
    }

    public static System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Object>> DataTableToList(System.Data.DataTable DataTable)
    {
      System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Object>> Rows = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Object>>();
      System.Collections.Generic.Dictionary<System.String, System.Object> Row;
      foreach (System.Data.DataRow DataRow in DataTable.Rows)
      {
        Row = new System.Collections.Generic.Dictionary<System.String, System.Object>();
        foreach (System.Data.DataColumn DataColumn in DataTable.Columns)
          Row.Add(DataColumn.ColumnName, System.Convert.IsDBNull(DataRow[DataColumn]) ? null : DataRow[DataColumn]);
        Rows.Add(Row);
      }
      return Rows;
    }
    public static System.Text.Json.JsonElement DataSetToJSON(System.Data.DataSet DataSet) { return SoftmakeAll.SDK.DataAccess.DatabaseValues.DataSetToJSON(DataSet, false); }
    public static System.Text.Json.JsonElement DataSetToJSON(System.Data.DataSet DataSet, System.Boolean IncludeNullValues)
    {
      if (DataSet == null)
        return new System.Text.Json.JsonElement();

      System.Collections.Generic.List<System.Text.Json.JsonElement> JSONDataTables = new System.Collections.Generic.List<System.Text.Json.JsonElement>();
      foreach (System.Data.DataTable DataTable in DataSet.Tables)
        JSONDataTables.Add(SoftmakeAll.SDK.DataAccess.DatabaseValues.DataTableToJSON(DataTable));

      return JSONDataTables.ToJsonElement();
    }
    public static System.Text.Json.JsonElement DataTableToJSON(System.Data.DataTable DataTable) { return SoftmakeAll.SDK.DataAccess.DatabaseValues.DataTableToJSON(DataTable, false); }
    public static System.Text.Json.JsonElement DataTableToJSON(System.Data.DataTable DataTable, System.Boolean IncludeNullValues)
    {
      if (DataTable == null)
        return new System.Text.Json.JsonElement();

      System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Object>> Rows = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Object>>();
      foreach (System.Data.DataRow DataRow in DataTable.Rows)
      {
        System.Collections.Generic.Dictionary<System.String, System.Object> Row = new System.Collections.Generic.Dictionary<System.String, System.Object>();
        foreach (System.Data.DataColumn DataColumn in DataTable.Columns)
        {
          System.Object Value = DataRow[DataColumn];
          System.Boolean IsNullValue = System.Convert.IsDBNull(Value);
          if ((!(IncludeNullValues)) && (IsNullValue))
            continue;
          if (IsNullValue)
            Row.Add(DataColumn.ColumnName, null);
          else
            Row.Add(DataColumn.ColumnName, Value);
        }

        Rows.Add(Row);
      }

      return Rows.ToJsonElement();
    }
    public static System.Text.Json.JsonElement DataRowToJSON(System.Data.DataRow DataRow) { return SoftmakeAll.SDK.DataAccess.DatabaseValues.DataRowToJSON(DataRow, false); }
    public static System.Text.Json.JsonElement DataRowToJSON(System.Data.DataRow DataRow, System.Boolean IncludeNullValues)
    {
      if (DataRow == null)
        return new System.Text.Json.JsonElement();

      System.Collections.Generic.Dictionary<System.String, System.Object> Row = new System.Collections.Generic.Dictionary<System.String, System.Object>();
      foreach (System.Data.DataColumn DataColumn in DataRow.Table.Columns)
      {
        System.Object Value = DataRow[DataColumn];
        System.Boolean IsNullValue = System.Convert.IsDBNull(Value);
        if ((!(IncludeNullValues)) && (IsNullValue))
          continue;
        if (IsNullValue)
          Row.Add(DataColumn.ColumnName, null);
        else
          Row.Add(DataColumn.ColumnName, Value);
      }
      return Row.ToJsonElement();
    }
  }
  #endregion
}