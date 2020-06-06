using Bogus;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public void OnGet(bool generateDummyTestResults = false)
        {
            if (generateDummyTestResults)
            {
                var dummyTestResultsSingle = new Faker<TestResults.SingleTestResultsTemplate>()
                    .RuleFor(o => o.Result, f => f.Random.Double(0, 2000))
                    .Generate(50);

                var dummyTestResultsGenerator = new Faker<TestResults>()
                    //Ensure all properties have rules. By default, StrictMode is false
                    //Set a global policy by using Faker.DefaultStrictMode
                    .StrictMode(false)
                    .RuleFor(o => o.TestDate, f => f.Date.Past())
                    //Pikc a random set of results from the pre-generated ones
                    .RuleFor(o => o.Test1Results, f => f.PickRandom<TestResults.SingleTestResultsTemplate>(dummyTestResultsSingle))
                    .RuleFor(o => o.Test2Results, f => f.PickRandom<TestResults.SingleTestResultsTemplate>(dummyTestResultsSingle))
                    .RuleFor(o => o.Test3Results, f => f.PickRandom<TestResults.SingleTestResultsTemplate>(dummyTestResultsSingle));
                ////A nullable int? with 80% probability of being null.
                ////The .OrNull extension is in the Bogus.Extensions namespace.
                //.RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));

                var equipments = _dbContext.Equipments.ToList();

                equipments.FirstOrDefault().TestResults.AddRange(dummyTestResultsGenerator.Generate(10));
                equipments.ElementAtOrDefault(1).TestResults.AddRange(dummyTestResultsGenerator.Generate(10));

                _dbContext.SaveChanges();

            }

            var listOfUpdatedEntriesId = new List<int> { 71, 75 };
            var adjacent =
               from tr in _dbContext.TestResults
               where listOfUpdatedEntriesId.Contains(tr.Id)
               select new
               {
                   PrevId = Sql.ToNullable(Sql.Ext.Lag(tr.Id, Sql.Nulls.None).Over().PartitionBy(tr.EquipmentId).OrderBy(tr.TestDate).ToValue()),
                   CurrentId = tr.Id,
                   NextId = Sql.ToNullable(Sql.Ext.Lead(tr.Id, Sql.Nulls.None).Over().PartitionBy(tr.EquipmentId).OrderBy(tr.TestDate).ToValue()),
               };

            foreach (var item in adjacent)
            {
                var value = item.CurrentId;
            }
        }
    }
}
