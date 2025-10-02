using FirstFloor.ModernUI.Windows;
using Microsoft.EntityFrameworkCore;
using QuotationSystem2.DataAccessLayer.Interfaces;
using QuotationSystem2.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Linq.Expressions;

namespace QuotationSystem2.DataAccessLayer.Repositories
{
    internal class QuoteWriteRepo : IQuoteWriteRepo
    {
        private readonly QuotationSystem2DBContext _context;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public QuoteWriteRepo(QuotationSystem2DBContext context)
        {
            _context = context;
        }

        public async Task AddRecord<T>(T record) where T : class
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record), "Record cannot be null");
            }
            await _context.Set<T>().AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task EditRecord<T>(T record) where T : class
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            var entry = _context.Entry(record);

            if (entry.State == EntityState.Detached)
            {
                // 找出是否有舊的被追蹤
                var key = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey();
                if (key == null)
                    throw new InvalidOperationException($"{typeof(T).Name} does not have a PrimaryKey");
                var keyValues = key?.Properties.Select(p => p.PropertyInfo?.GetValue(record)).ToArray();
                if (keyValues == null || keyValues.Length == 0)
                    throw new InvalidOperationException($"{typeof(T).Name} does not have a PrimaryKey Value");

                var existingEntity = await _context.Set<T>().FindAsync(keyValues);
                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached; // Detach the old value
                }

                _context.Update(record); // Attach the new value
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditWaterMeterRecordAsync(WaterMeterRecord record)
        {
            var existingRecord = await _context.WaterMeterRecords
                                    .Where(r => r.WaterMeterID == record.WaterMeterID)
                                    .Include(r => r.WaterMeterRecordSockets)
                                    .Include(r => r.WaterMeterRecordNipples)
                                    .FirstOrDefaultAsync();
            if (existingRecord == null)
            {
                _context.Update(record);
            }
            else
            {
                // Update main table values
                _context.Entry(existingRecord).CurrentValues.SetValues(record);

                #region -------- NIPPLES --------
                foreach (var old in existingRecord.WaterMeterRecordNipples.ToList())
                {
                    if (!record.WaterMeterRecordNipples.Any(n => n.NippleID == old.NippleID))
                        _context.Remove(old);
                }

                foreach (var newItem in record.WaterMeterRecordNipples)
                {
                    MessageBox.Show($"NippleID: {newItem.NippleID}");
                    var existing = existingRecord.WaterMeterRecordNipples
                        .FirstOrDefault(n => n.NippleID == newItem.NippleID);

                    if (existing != null)
                    {
                        // 更新舊的屬性
                        _context.Entry(existing).CurrentValues.SetValues(newItem);
                    }
                    else
                    {
                        // 新增新的
                        existingRecord.WaterMeterRecordNipples.Add(newItem);
                    }
                }
                #endregion

                #region -------- SOCKETS --------
                foreach (var old in existingRecord.WaterMeterRecordSockets.ToList())
                {
                    if (!record.WaterMeterRecordSockets.Any(s => s.SocketID == old.SocketID))
                        _context.Remove(old);
                } // Delete the deleted items from Db

                foreach (var newItem in record.WaterMeterRecordSockets)
                {
                    var existing = existingRecord.WaterMeterRecordSockets
                        .FirstOrDefault(s => s.SocketID == newItem.SocketID);

                    if (existing != null) // Has existing -> update value
                    {
                        _context.Entry(existing).CurrentValues.SetValues(newItem);
                    }
                    else // No existing -> add new
                    {
                        existingRecord.WaterMeterRecordSockets.Add(newItem);
                    }
                }
                #endregion
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditSealingPlateRecordAsync(SealingPlateRecord record)
        {
            var existingRecord = await _context.SealingPlateRecords
                                    .Where(r => r.SealingPlateID == record.SealingPlateID)
                                    .FirstOrDefaultAsync();

            if (existingRecord == null)
            {
                _context.Update(record);
            }
            else
            {
                // Update main table values
                _context.Entry(existingRecord).CurrentValues.SetValues(record);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllRecordsAsync<T>() where T : class
        {
            await _semaphore.WaitAsync();
            try
            {
                var dbSet = _context.Set<T>();
                dbSet.RemoveRange(dbSet);
                await _context.SaveChangesAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task DeleteRecordsByIDAsync<T>(List<int> ids) where T : class
        {
            await _semaphore.WaitAsync();
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(T))
                    ?? throw new InvalidOperationException($"Cannot find EntityType for {typeof(T).Name}");

                var pk = entityType.FindPrimaryKey()
                    ?? throw new InvalidOperationException($"{typeof(T).Name} does not have a PrimaryKey");

                if (pk.Properties.Count != 1)
                    throw new NotSupportedException("Only single primary key is currently supported");


                var keyName = pk.Properties[0].Name;

                await _context.Set<T>()
                    .Where(e => ids.Contains(EF.Property<int>(e, keyName)))
                    .ExecuteDeleteAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

    }
}
