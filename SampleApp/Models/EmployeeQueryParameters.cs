using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Models
{
    public class EmployeeQueryParameters
    {
        public EmployeeQueryParameters()
        {
            SearchString = string.Empty;
            CurrentPage = 1;
            PageSize = 10;
            Status = true;
        }

        [FromQuery(Name = "search_string")]
        public string SearchString { get; set; }

        [FromQuery(Name = "initial_date")]
        public DateTime InitialDate { get; set; }

        [FromQuery(Name = "final_date")]
        public DateTime FinalDate { get; set; }

        [FromQuery(Name = "current_page")]
        public int CurrentPage { get; set; }

        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; }

        [FromQuery(Name = "status")]
        public bool Status { get; set; }
    }
}
