using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newses.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Newses.Controllers
{
    public class StoryController : Controller
    {
        private readonly IConfiguration _configuration;
        public StoryController(IConfiguration configuration)
        {
            this._configuration = configuration;

        }

        public IActionResult Index()
        {
            StoryViewModel storyViewModel = new StoryViewModel();
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("AllStory", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.Fill(dataTable);
            }
            if(dataTable.Rows.Count>0)
            { 
                
                var newTable = dataTable.AsEnumerable().OrderByDescending(r => r.Field<DateTime>("Date")).CopyToDataTable();
                
                return View(newTable);
            }
            else
            {
                var newTable = dataTable.AsEnumerable().OrderByDescending(r => r.Field<DateTime>("Date")).CopyToDataTable();

                return View(newTable);

            }
              
        }
        
        public IActionResult AddOrEdit(int? id)
        {
           
            StoryViewModel storyViewModel = new StoryViewModel();
           
            if (id > 0 )
            {
                DataTable dataTable = new DataTable();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("StoryBYId", sqlConnection);
                    sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                    sqlDataAdapter.Fill(dataTable);

                }
                storyViewModel.Title = dataTable.Rows[0]["Title"].ToString();
                storyViewModel.Body = dataTable.Rows[0]["Body"].ToString();
                storyViewModel.date = Convert.ToDateTime(dataTable.Rows[0]["Date"]);
                storyViewModel.id = Convert.ToInt32(dataTable.Rows[0]["id"]);
            }

            return View(storyViewModel);
        }
        [HttpPost]
        public IActionResult AddorEdit(StoryViewModel storyViewModel)
        {

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                var name = User.Identity.Name;
                sqlConnection.Open();
                SqlCommand SqlCmd = new SqlCommand("AddorEdit", sqlConnection);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.AddWithValue("Title", storyViewModel.Title);
                SqlCmd.Parameters.AddWithValue("Body", storyViewModel.Body);
                SqlCmd.Parameters.AddWithValue("Datetime", storyViewModel.date);
                SqlCmd.Parameters.AddWithValue("Author", name);
                SqlCmd.Parameters.AddWithValue("ID", storyViewModel.id);
                SqlCmd.ExecuteNonQuery();
            }

            return RedirectToAction(nameof(Index));
        }   
        

        public IActionResult Delete(int? id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand SqlCmd = new SqlCommand("DeleteBYId", sqlConnection);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.AddWithValue("ID", id);
                SqlCmd.ExecuteNonQuery();

            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult GetByIDforJson(int? id)
        {
            
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("StoryBYId", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                sqlDataAdapter.Fill(dataTable);

            }
            string JsonString = DataTableToJSONWithStringBuilder(dataTable);
            ViewData["JsonString"] = JsonString;

            return View();
        }
        public IActionResult GetByIDforXml(int? id)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("StoryBYId", sqlConnection);
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("id", id);
                sqlDataAdapter.Fill(dataTable);
                DataSet ds = new DataSet();
                ds.Tables.Add(dataTable);
                string dsXml = ds.GetXml();

                return Content(dsXml, "text/xml");
            }
        }
        public string DataTableToJSONWithStringBuilder(DataTable table)
        {
            var JSONString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JSONString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JSONString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JSONString.Append("}");
                    }
                    else
                    {
                        JSONString.Append("},");
                    }
                }
                JSONString.Append("]");
            }
            return JSONString.ToString();
        }

    }
}
