using GestRehema.Data;
using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestRehema.Services
{
    public interface IExpenseService
    {
        Expense AddOrUpdateExpense(Expense expense);
        List<string> GetCategories();
        List<Expense> GetExpenses(string? category = null, DateTime? date = null, int skip = 0, int take = 100);
        List<Expense> GetExpenses(int skip, int take = 100);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly AppDbContext _dbContext;

        public ExpenseService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public Expense AddOrUpdateExpense(Expense expense)
        {
            var regExpense = _dbContext.Expenses.FirstOrDefault(x => x.Id == expense.Id);
            if (regExpense != null)
            {
                regExpense.Owner = expense.Owner;
                regExpense.PayementId = expense.PayementId;
                regExpense.Title = expense.Title;
                regExpense.UpdatedAt = DateTime.UtcNow;
                regExpense.Description = expense.Description;
                regExpense.EmployeeId = expense.EmployeeId;
                regExpense.Category = expense.Category;
                regExpense.Amount = expense.Amount;
                _dbContext.Update(regExpense);
                _dbContext.SaveChanges();
                return _dbContext.Expenses.First(x => x.Id == regExpense.Id);
            }
            else
            {
                expense.UpdatedAt = DateTime.UtcNow;
                _dbContext.Add(expense);
                _dbContext.SaveChanges();
                return _dbContext.Expenses.First(x => x.CreatedAt == expense.CreatedAt);
            }
        }

        public List<Expense> GetExpenses(int skip, int take = 100)
        => _dbContext.Expenses
            .Include(x => x.Employee)
            .Include(x => x.Payement)
            .OrderByDescending(x => x.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .ToList();

        public List<Expense> GetExpenses(string? category = null, DateTime? date = null, int skip = 0, int take = 100)
        => _dbContext.Expenses
            .Include(x => x.Employee)
            .Include(x => x.Payement)
            .Where(x => (category == null || x.Category == category) && (date == null || x.CreatedAt.Date == date.Value))
            .OrderByDescending(x => x.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .ToList();

        public List<string> GetCategories()
           => _dbContext.Expenses.Select(x => x.Category)
            .ToList()
            .DistinctBy(x => x)
            .ToList();
    }
}
