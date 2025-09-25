using BullearApp.Data;
using BullearApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Bullear.Services
{
    public class TradeService
    {
        private readonly BullearDbContext _context;

        public TradeService(BullearDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade?> GetTradeByIdAsync(int id)
        {
            return await _context.Trades.FindAsync(id);
        }

        public async Task<Trade> AddTradeAsync(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
            return trade;
        }

        public async Task<Trade?> UpdateTradeAsync(Trade trade)
        {
            var existingTrade = await _context.Trades.FindAsync(trade.Id);
            if (existingTrade == null)
                return null;

            _context.Entry(existingTrade).CurrentValues.SetValues(trade);
            await _context.SaveChangesAsync();
            return existingTrade;
        }

        public async Task<bool> DeleteTradeAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade == null)
                return false;

            _context.Trades.Remove(trade);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<FidelityUpload>> GetAllFidelityUploadsAsync()
        {
            return await _context.FidelityUploads.ToListAsync();
        }

        public async Task<FidelityUpload> AddFidelityUploadAsync(FidelityUpload upload)
        {
            _context.FidelityUploads.Add(upload);
            await _context.SaveChangesAsync();
            return upload;
        }

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Position> AddPositionAsync(Position position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<int> ImportFidelityCsvAsync(string filePath)
        {
            var uploads = new List<FidelityUpload>();
            
            try
            {
                // Test database connection
                Console.WriteLine("Testing database connection...");
                var canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Database connection test: {canConnect}");
                
                if (!canConnect)
                {
                    throw new InvalidOperationException("Cannot connect to database");
                }
                
                // Test table access
                Console.WriteLine("Testing FidelityUploads table access...");
                var existingCount = await _context.FidelityUploads.CountAsync();
                Console.WriteLine($"Existing records in FidelityUploads table: {existingCount}");
                Console.WriteLine($"Reading CSV file: {filePath}");
                var lines = await File.ReadAllLinesAsync(filePath);
                Console.WriteLine($"File has {lines.Length} lines");
                
                // Skip first two lines and start from line 3 (index 2) which contains headers
                if (lines.Length < 3)
                {
                    Console.WriteLine("File has less than 3 lines, throwing exception");
                    throw new InvalidOperationException("CSV file must have at least 3 lines (2 empty + headers)");
                }
                
                var headerLine = lines[2];
                Console.WriteLine($"Header line: {headerLine}");
                var headers = ParseCsvLine(headerLine);
                Console.WriteLine($"Parsed headers: {string.Join(", ", headers)}");
                
                // Process data rows starting from line 4 (index 3)
                int processedRows = 0;
                for (int i = 3; i < lines.Length; i++)
                {
                    var line = lines[i].Trim();
                    processedRows++;
                    
                    // Stop processing if we encounter a blank line or empty first column
                    if (string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine($"Stopping at empty line {i}");
                        break;
                    }
                    
                    var values = ParseCsvLine(line);
                    if (values.Length == 0 || string.IsNullOrEmpty(values[0]))
                    {
                        Console.WriteLine($"Stopping at empty first column, line {i}");
                        break;
                    }
                    
                    Console.WriteLine($"Processing row {i}: {line}");
                    var upload = CreateFidelityUploadFromCsvRow(headers, values);
                    if (upload != null)
                    {
                        uploads.Add(upload);
                        Console.WriteLine($"Added upload record for symbol: {upload.Symbol}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create upload record for line {i}");
                    }
                }
                
                Console.WriteLine($"Processed {processedRows} rows, created {uploads.Count} upload records");
                
                // Bulk insert for better performance
                if (uploads.Count > 0)
                {
                    Console.WriteLine("Saving to database...");
                    _context.FidelityUploads.AddRange(uploads);
                    var savedCount = await _context.SaveChangesAsync();
                    Console.WriteLine($"Database save completed. {savedCount} records saved.");
                }
                else
                {
                    Console.WriteLine("No records to save to database");
                }
                
                return uploads.Count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ImportFidelityCsvAsync: {ex}");
                throw new InvalidOperationException($"Error importing CSV file: {ex.Message}", ex);
            }
        }
        
        private string[] ParseCsvLine(string line)
        {
            var values = new List<string>();
            var currentValue = "";
            var inQuotes = false;
            
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue.Trim());
                    currentValue = "";
                }
                else
                {
                    currentValue += c;
                }
            }
            
            // Add the last value
            values.Add(currentValue.Trim());
            
            return values.ToArray();
        }
        
        private FidelityUpload? CreateFidelityUploadFromCsvRow(string[] headers, string[] values)
        {
            try
            {
                var upload = new FidelityUpload();
                
                for (int i = 0; i < Math.Min(headers.Length, values.Length); i++)
                {
                    var header = headers[i].Trim();
                    var value = values[i].Trim();
                    
                    switch (header)
                    {
                        case "Run Date":
                            upload.RunDate = ParseDateTime(value);
                            break;
                        case "Action":
                            upload.Action = value ?? string.Empty;
                            break;
                        case "Symbol":
                            upload.Symbol = value ?? string.Empty;
                            break;
                        case "Description":
                            upload.Description = value ?? string.Empty;
                            break;
                        case "Type":
                            upload.Type = value ?? string.Empty;
                            break;
                        case "Quantity":
                            upload.Quantity = ParseInt(value);
                            break;
                        case "Price ($)":
                            upload.Price = ParseDecimal(value);
                            break;
                        case "Commission ($)":
                            upload.Commission = ParseDecimal(value);
                            break;
                        case "Fees ($)":
                            upload.Fees = ParseDecimal(value);
                            break;
                        case "Accrued Interest ($)":
                            upload.AccruedInterest = ParseDecimal(value);
                            break;
                        case "Amount ($)":
                            upload.Amount = ParseDecimal(value);
                            break;
                        case "Cash Balance ($)":
                            upload.CashBalance = ParseDecimal(value);
                            break;
                        case "Settlement Date":
                            upload.SettlementDate = ParseDateTime(value);
                            break;
                    }
                }
                
                return upload;
            }
            catch (Exception ex)
            {
                // Log error and skip this row
                Console.WriteLine($"Error parsing CSV row: {ex.Message}");
                return null;
            }
        }
        
        private DateTime ParseDateTime(string value)
        {
            if (string.IsNullOrEmpty(value))
                return DateTime.MinValue;
                
            if (DateTime.TryParse(value, out DateTime result))
                return result;
                
            return DateTime.MinValue;
        }
        
        private int ParseInt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
                
            if (int.TryParse(value, out int result))
                return result;
                
            return 0;
        }
        
        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0.0m;
                
            // Remove currency symbols and commas
            value = value.Replace("$", "").Replace(",", "").Trim();
                
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return result;
                
            return 0.0m;
        }
    }
}
