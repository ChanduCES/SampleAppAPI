using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SampleApp.Migrations
{
    /// <inheritdoc />
    public partial class sp_getEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE OR ALTER   PROCEDURE [dbo].[Usp_GetEmployees]
                        @searchText AS VARCHAR(100)='',
                        @pageNo INT ,  
                        @pageSize INT 
                        AS
                        BEGIN
                         DECLARE @query AS VARCHAR(MAX)='',@conditions AS VARCHAR(MAX)='', @pagination AS VARCHAR(MAX)=''

                         SET @query='SELECT * FROM Employees '

                         IF(ISNULL(@searchText,'')<>'')
                         BEGIN
  
                           SET @conditions='
                           WHERE
                           Name LIKE ''%'+@searchText+'%''  '
                         END

 

                         IF(@pageSize>0 AND @pageNo>0)
                         BEGIN
                          SET @pagination=' OFFSET '+(CAST(((@pageNo - 1) * @pageSize) AS varchar(10)))+' ROWS
                          FETCH NEXT '+(CAST(@pageSize AS varchar(10)))+' ROWS ONLY'
                         END

                         IF(@conditions<>'') SET @query+=@conditions
                         SET @query+='ORDER BY Name'
                         IF(@pagination<>'') SET @query+=@pagination

                         EXEC(@query)
                        END";

            migrationBuilder.Sql(sp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
