using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.ExtensionMethods;
using System.Data;

namespace Services.Tests.ExtensionMethods
{
    [TestClass]
    public class DataTableExtensionMethodTests
    {
        [TestMethod]
        public void ToCsv_NoColumnsDataTable_ReturnNull()
        {
            //Arrange
            var dataTable = new DataTable();

            //Act
            var csv = dataTable.ToCsv();

            //Assert
            Assert.IsNull(csv);
        }

        [TestMethod]
        public void ToCsv_NoRowsDataTable_ReturnOnlyHeaders()
        {
            //Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Id" });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Name" });

            //Act
            var csv = dataTable.ToCsv();

            //Assert
            Assert.AreEqual("Id,Name"+Environment.NewLine, csv);
        }

        [TestMethod]
        public void ToCsv_1RowsDataTable_ReturnHeaderRowAnd1DataRow()
        {
            //Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Id", DataType = typeof(Int32) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string) });
            var dataRow = dataTable.NewRow();
            dataRow["Id"] = 1;
            dataRow["Name"] = "Luke Skywalker";
            dataTable.Rows.Add(dataRow);

            var expectedResult = "Id,Name" + Environment.NewLine;
            expectedResult += "1,Luke Skywalker" + Environment.NewLine;

            //Act
            var csv = dataTable.ToCsv();

            //Assert
            Assert.AreEqual(expectedResult, csv);
        }

        [TestMethod]
        public void ToCsv_2RowsDataTable_ReturnHeaderRowAnd2DataRows()
        {
            //Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Id", DataType = typeof(Int32) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Name", DataType = typeof(string) });
            var dataRow = dataTable.NewRow();
            dataRow["Id"] = 1;
            dataRow["Name"] = "Luke Skywalker";
            dataTable.Rows.Add(dataRow);
            var dataRow2 = dataTable.NewRow();
            dataRow2["Id"] = 2;
            dataRow2["Name"] = "Obi Wan Kanoodle";
            dataTable.Rows.Add(dataRow2);

            var expectedResult = "Id,Name" + Environment.NewLine;
            expectedResult += "1,Luke Skywalker" + Environment.NewLine;
            expectedResult += "2,Obi Wan Kanoodle" + Environment.NewLine;

            //Act
            var csv = dataTable.ToCsv();

            //Assert
            Assert.AreEqual(expectedResult, csv);
        }
    }
}
