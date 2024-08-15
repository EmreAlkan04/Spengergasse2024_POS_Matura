using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe2.Services
{
    public record SaleStatistics(string StickerTypeName, decimal TotalRevenue);

    public class StickerService
    {
        private readonly StickerContext _db;

        public StickerService(StickerContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Checks the permission for a scanned numberplate.
        /// </summary>
        public bool HasPermission(string numberplate, DateTime dateTime, VehicleType carType)
        {
            var platenumber = _db.Stickers
                .Include(s => s.StickerType)
                .Include(s => s.Customer)
                .FirstOrDefault(s => s.Numberplate == numberplate && s.StickerType.VehicleType == carType);

            if(platenumber == null)
            {
                //throw new ServiceException("Kein Eintrag gefunden");
                return false; 

            }

            DateTime validUntil = platenumber.ValidFrom.AddDays(platenumber.StickerType.DaysValid);

            if(dateTime < platenumber.ValidFrom || dateTime > validUntil)
            {
                //throw new ServiceException("Datum falsch");
                return false; 
            }

            return true;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the total revenue for every sticker type sold in a specific year.
        /// Use PurchaseDate.Year to get the year of purchase.
        /// Hint: To use Sum() you have to cast Price to double.
        ///       After that you have to cast the sum back to decimal.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<SaleStatistics> CalcSaleStatistics(int year)
        {
            var totalRevenueEachStickerType = _db.Stickers
                .Where(s => s.PurchaseDate.Year == year)
                .GroupBy(s => s.StickerType.Name)
                .Select(s => new SaleStatistics(s.Key, (decimal)s.Sum(s => (double)s.Price)))
                //.GroupBy(s => s.Sum(s => (double)s.Price))
                .ToList();

            return totalRevenueEachStickerType;


            //throw new NotImplementedException();
        }
    }
}