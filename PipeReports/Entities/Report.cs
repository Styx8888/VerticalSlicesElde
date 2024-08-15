namespace PipeReports.API.Entities;

public class Report
{
    public int Id { get; set; }

    public string Client { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string Address { get; set; } = string.Empty;

    public string PostCode { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? ClientRef { get; set; }

    public string? LogoUrl { get; set; }

    public string? RequirementsText { get; set; }

    // other entities impl later

    //public virtual Location Location { get; set; }

    //public virtual SiteInformation SiteInformation { get; set; }

    //public virtual List<SectionalImages> SectionalImagesList { get; set; }

    //public virtual Summary Summary { get; set; }
}