using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DemocraMap.Services
{
    public class GovDataService
    {
        private readonly IWebHostEnvironment _env;

        public GovDataService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Generic CSV loader
        private async Task<List<T>> LoadCsvAsync<T>(string fileName, ClassMap<T> map)
        {
            var filePath = Path.Combine(_env.WebRootPath, "csv", fileName);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap(map);
            var records = csv.GetRecords<T>().ToList();

            return await Task.FromResult(records);
        }

        // ---- File-specific loaders ----

        public Task<List<IndigenousRegion>> GetIndigenousRegionsAsync() =>
            LoadCsvAsync("Underrepresented Indigenous Regions.csv", new IndigenousRegionMap());

        public Task<List<MigrantRegion>> GetMigrantRegionsAsync() =>
            LoadCsvAsync("Underrepresented Migrant Population Centers.csv", new MigrantRegionMap());

        public Task<List<OutreachLocation>> GetProposedOutreachAsync() =>
            LoadCsvAsync("Proposed NEEC Outreach Locations (Consolidated Priorities).csv", new OutreachLocationMap());

        public Task<List<EnrolmentGap>> GetIndigenousEnrolmentGapsAsync() =>
            LoadCsvAsync("Indigenous Enrolment Gaps.csv", new EnrolmentGapMap());

        public Task<List<DivisionEnrolRate>> GetDivisionEnrolRatesAsync() =>
            LoadCsvAsync("2025-06-div-enrol-rate.csv", new DivisionEnrolRateMap());

        public Task<List<MigrantParticipationGap>> GetMigrantParticipationGapsAsync() =>
            LoadCsvAsync("Migrant_Participation_Gaps.csv", new MigrantParticipationGapMap());


    }

    // ---- POCOs + CSV Maps ----

    public class IndigenousRegion
    {
        public string Region { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GroupType { get; set; }
        public string EnrolmentRate { get; set; }
        public int EducationAccessScore { get; set; }
        public string IndigenousPopulationPercent { get; set; }
        public string RegionInGap { get; set; }
    }

    public sealed class IndigenousRegionMap : ClassMap<IndigenousRegion>
    {
        public IndigenousRegionMap()
        {
            Map(m => m.Region).Name("Region");
            Map(m => m.Latitude).Name("Latitude");
            Map(m => m.Longitude).Name("Longitude");
            Map(m => m.GroupType).Name("Group Type");
            Map(m => m.EnrolmentRate).Name("Enrolment Rate");
            Map(m => m.EducationAccessScore).Name("Education Access Score");
            Map(m => m.IndigenousPopulationPercent).Name("Indigenous Pop. %");
            Map(m => m.RegionInGap).Name("Region in Gap");
        }
    }

    public class MigrantRegion
    {
        public string Region { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GroupType { get; set; }
        public string EnrolmentRate { get; set; }
        public int EducationAccessScore { get; set; }
        public string OverseasBornPercent { get; set; }
        public string RegionInGap { get; set; }
    }

    public sealed class MigrantRegionMap : ClassMap<MigrantRegion>
    {
        public MigrantRegionMap()
        {
            Map(m => m.Region).Name("Region");
            Map(m => m.Latitude).Name("Latitude");
            Map(m => m.Longitude).Name("Longitude");
            Map(m => m.GroupType).Name("Group Type");
            Map(m => m.EnrolmentRate).Name("Enrolment Rate");
            Map(m => m.EducationAccessScore).Name("Education Access Score");
            Map(m => m.OverseasBornPercent).Name("Overseas-born %");
            Map(m => m.RegionInGap).Name("Region in Gap");
        }
    }

    public class OutreachLocation
    {
        public string Region { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GroupType { get; set; }
        public string EnrolmentRate { get; set; }
        public int EducationAccessScore { get; set; }
        public string TargetGroupPercent { get; set; }
        public string RegionInGap { get; set; }
    }

    public sealed class OutreachLocationMap : ClassMap<OutreachLocation>
    {
        public OutreachLocationMap()
        {
            Map(m => m.Region).Name("Region");
            Map(m => m.Latitude).Name("Latitude");
            Map(m => m.Longitude).Name("Longitude");
            Map(m => m.GroupType).Name("Group Type");
            Map(m => m.EnrolmentRate).Name("Enrolment Rate");
            Map(m => m.EducationAccessScore).Name("Education Access Score");
            Map(m => m.TargetGroupPercent).Name("Target Group %");
            Map(m => m.RegionInGap).Name("Region in Gap");
        }
    }

    public class EnrolmentGap
    {
        public string Region { get; set; }
        public string EnrolmentRate { get; set; }
        public string GapDescription { get; set; }
        public string PossibleInterventions { get; set; }
    }

    public sealed class EnrolmentGapMap : ClassMap<EnrolmentGap>
    {
        public EnrolmentGapMap()
        {
            Map(m => m.Region).Name("Region");
            Map(m => m.EnrolmentRate).Name("Enrolment Rate");
            Map(m => m.GapDescription).Name("Gap Description");
            Map(m => m.PossibleInterventions).Name("Possible Interventions");
        }
    }
    public class DivisionEnrolRate
    {
        public string Division { get; set; }
        public string State { get; set; }
        public double EnrolmentRate { get; set; }
        public int EnrolledVoters { get; set; }
        public int EligibleVoters { get; set; }
    }

    public sealed class DivisionEnrolRateMap : ClassMap<DivisionEnrolRate>
    {
        public DivisionEnrolRateMap()
        {
            Map(m => m.Division).Name("Division");
            Map(m => m.State).Name("State");
            Map(m => m.EnrolmentRate).Name("Enrolment Rate");
            Map(m => m.EnrolledVoters).Name("Enrolled Voters");
            Map(m => m.EligibleVoters).Name("Eligible Voters");
        }
    }

    public class MigrantParticipationGap
    {
        public string Region { get; set; }
        public string KeyDataPoint { get; set; }
        public string GapDescription { get; set; }
        public string PossibleInterventions { get; set; }
    }

    public sealed class MigrantParticipationGapMap : ClassMap<MigrantParticipationGap>
    {
        public MigrantParticipationGapMap()
        {
            Map(m => m.Region).Name("Region / Electorate (example)");
            Map(m => m.KeyDataPoint).Name("Key Data Point");
            Map(m => m.GapDescription).Name("Gap Description");
            Map(m => m.PossibleInterventions).Name("Possible Interventions");
        }
    }

}