using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var list = BreadmakerDb.Breadmakers
                .Select(list_item => new
                {
                    list_detail = list_item.title,
                    list_reviews = list_item.Reviews.Count(),
                    list_avg = (Double)BreadmakerDb.Reviews
                        .Where(i => i.BreadmakerId == list_item.BreadmakerId)
                        .Select(i => i.stars).Sum() / list_item.Reviews.Count(),
                })
                .ToList();

            var BMList = list
                .Select(list_item => new
                {
                    details = list_item.list_detail,
                    reviews = list_item.list_reviews,
                    avg = list_item.list_avg,
                    adjust = ratingAdjustmentService.Adjust(list_item.list_avg, list_item.list_reviews)
                })
                .OrderByDescending(i => i.adjust)
                .ToList();


            Console.WriteLine("[#]  Reviews  Average  Adjust  Description");
            for (var j = 0; j < 3; j++)
            {
                var row = BMList[j];
                Console.WriteLine($"[{j + 1}]  {row.reviews,7}  {Math.Round(row.avg, 2),-7}  {Math.Round(row.adjust, 2),-6}   {row.details}");
            }
        }
    }
}
