using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using OnlineCashRmk.DataModels;
using OnlineCashTransportModels.Shared;

namespace OnlineCashRmk
{
    public partial class FormHistory : Form
    {
        private readonly IDbContextFactory<DataContext> _dbFactory;
        public FormHistory(IDbContextFactory<DataContext> dbFactory)
        {
            _dbFactory = dbFactory;
            InitializeComponent();
            
            GetHistory();
        }

        async void GetHistory()
        {
            using var db = _dbFactory.CreateDbContext();
            var docSynches = await db.DocSynches
                .Where(d => DateTime.Compare(d.Create.Date, DateTime.Now.AddDays(-5).Date) > 0 || DateTime.Compare(d.Create.Date, DateTime.Now.Date) == 0)
                .OrderByDescending(d => d.Create)
                .AsNoTracking().ToListAsync();
            listBox1.DataSource = docSynches;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var docSynch = (DocSynch)listBox1.SelectedItem;
            if (docSynch.TypeDoc == TypeDocs.OpenShift)
                RenderOpenShift(docSynch.DocId);
            if (docSynch.TypeDoc == TypeDocs.CloseShift)
                RenderCloseShift(docSynch.DocId);
            if (docSynch.TypeDoc == TypeDocs.Buy)
                RenderBuy(docSynch.DocId);
            if (docSynch.TypeDoc == TypeDocs.WriteOf)
                RenderWriteOf(docSynch.DocId);
            if (docSynch.TypeDoc == TypeDocs.Arrival)
                RenderArrival(docSynch.DocId);
            if (docSynch.TypeDoc == TypeDocs.CashMoney)
                RenderCashMoney(docSynch.DocId);
            if (docSynch.TypeDoc == TypeDocs.Revaluation)
                RenderRevaluation(docSynch.DocId);
        }

        private async void RenderOpenShift(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            Shift shift = await _db.Shifts.Where(s => s.Id == docId)
                .AsNoTracking().FirstAsync();
            richTextBox1.Text = $"Открытие смены {shift.Start.ToString("dd.MM.yy HH:mm")}"+Environment.NewLine;
        }

        private async void RenderCloseShift(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            Shift shift = await _db.Shifts.Where(s => s.Id == docId)
                .AsNoTracking().FirstAsync();
            richTextBox1.Text = @$"Закрытие смены {shift.Start.ToString("dd.MM.yy HH:mm")}

Итоги смены:
---------------- 
Наличные:   {shift.SumNoElectron} 
Безналичные:    {shift.SumElectron}  
Продажи за сегодня:\t{shift.SumSell}
---------------- 
Возвраты:   {shift.SummReturn}
---------------- 
Акция 2+1: {shift.PromotionSum}";
        }

        private async void RenderArrival(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            Arrival arrival = await _db.Arrivals.Where(a => a.Id == docId)
                .Include(a => a.ArrivalGoods)
                .ThenInclude(a => a.Good)
                .Include(a => a.Supplier)
                .AsNoTracking().FirstAsync();
            List<ArrivalPositionDataModel> positions = new List<ArrivalPositionDataModel>();
            var goods = _db.Goods.ToList();
            foreach (var arrivalGood in arrival.ArrivalGoods)
            {
                decimal priceSell = goods.Where(g => g.Id == arrivalGood.GoodId).FirstOrDefault().Price;
                positions.Add(new ArrivalPositionDataModel
                {
                    GoodName = arrivalGood.Good.Name,
                    Unit=arrivalGood.Good.Unit,
                    PriceArrival=arrivalGood.PriceArrival,
                    PriceSell= arrivalGood.PriceSell,
                    NdsPercent=arrivalGood.Nds,
                    Count=arrivalGood.Count,
                    ExpiresDate=arrivalGood.ExpiresDate
                });
            };
            var sumAllNds = positions.Sum(a => a.SumNds).ToSellFormat();
            var sumAllArrival = positions.Sum(a => a.SumArrival).ToSellFormat();
            var sumAllSell = positions.Sum(a => a.SumSell).ToSellFormat();

            richTextBox1.Text = @$"Приход {arrival.Num} от {arrival.DateArrival.ToString("dd.MM")} 
Поставщик {arrival.Supplier.Name}
Всего по закупке: {sumAllArrival} НДС: {sumAllNds} По продаже: {sumAllSell}


@@@";

            RtfTable table = new RtfTable(arrival.ArrivalGoods.Count+1, 11, 120);
            table.SetColumnWidths(720, 720, 720, 720, 720, 720, 720, 720, 720, 720, 720);

            string[] cells =new string[11] { "Наименование", "Ед", "Цена", "Цена продажи", "Наценка %", "Кол-во", "% НДС", "НДС", "Сумма покупки", "Сумма продажи", "Срок годности" };
            for (int r = 0; r < positions.Count + 1; r++)
                for (int c = 0; c < 11; c++)
                    if (r == 0)
                        table.Contents[r, c] = cells[c];
                    else
                        switch (c)
                        {
                            case 0:
                                table.Contents[r, c] = positions[r-1].GoodName;
                                break;
                            case 1:
                                table.Contents[r, c] = positions[r - 1].UnitStr;
                                break;
                            case 2:
                                table.Contents[r, c] = positions[r - 1].PriceArrival.ToSellFormat();
                                break;
                            case 3:
                                table.Contents[r, c] = positions[r - 1].PriceSell.ToSellFormat();
                                break;
                            case 4:
                                table.Contents[r, c] = positions[r - 1].PricePercent;
                                break;
                            case 5:
                                table.Contents[r, c] = positions[r - 1].Count.ToSellFormat();
                                break;
                            case 6:
                                table.Contents[r, c] = positions[r - 1].NdsPercent;
                                break;
                            case 7:
                                table.Contents[r, c] = positions[r - 1].SumNdsStr;
                                break;
                            case 8:
                                table.Contents[r, c] = positions[r - 1].SumArrival.ToSellFormat();
                                break;
                            case 9:
                                table.Contents[r, c] = positions[r - 1].SumSell.ToSellFormat();
                                break;
                            case 10:
                                table.Contents[r, c] = positions[r - 1].ExpiresDate?.ToString("dd.MM.yy");
                                break;
                        }
            richTextBox1.Rtf = richTextBox1.Rtf.Replace("@@@", table.ToString());
        }

        private async void RenderWriteOf(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            var writeof = await _db.Writeofs
                .Include(w => w.WriteofGoods)
                .ThenInclude(g => g.Good)
                .Where(w => w.Id == docId)
                .AsNoTracking().FirstAsync();
            richTextBox1.Text = $@"Списание товара от {writeof.DateCreate.ToString("dd.MM.yy")}
На сумму - {writeof.SumAll.ToSellFormat()}
{writeof.Note}

@@@";
            string[] cells = new string[5] { "Наименование", "Ед", "Кол-во", "Цена", "Сумма" };
            RtfTable table = new RtfTable(writeof.WriteofGoods.Count + 1, cells.Length, 120);
            table.SetColumnWidths(720, 720, 720, 720, 720);
            for (int r = 0; r < writeof.WriteofGoods.Count + 1; r++)
                for (int c = 0; c < cells.Length; c++)
                    if (r == 0)
                        table.Contents[r, c] = cells[c];
                    else
                        switch (c)
                        {
                            case 0:
                                table.Contents[r, c] = writeof.WriteofGoods[r - 1].Good.Name;
                                break;
                            case 1:
                                table.Contents[r, c] = writeof.WriteofGoods[r - 1].Good.Unit.GetDisplay();
                                break;
                            case 2:
                                table.Contents[r, c] = writeof.WriteofGoods[r - 1].Count.ToString();
                                break;
                            case 3:
                                table.Contents[r, c] = writeof.WriteofGoods[r - 1].Good.Price.ToSellFormat();
                                break;
                            case 4:
                                table.Contents[r, c] = writeof.WriteofGoods[r - 1].Sum.ToSellFormat();
                                break;
                        };
            richTextBox1.Rtf = richTextBox1.Rtf.Replace("@@@", table.ToString());
        }

        private async void RenderBuy(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            var check = await _db.CheckSells
                .Include(c => c.CheckPayments)
                .Include(c => c.CheckGoods)
                .ThenInclude(c => c.Good)
                .Where(c => c.Id == docId)
                .AsNoTracking().FirstAsync();
            string document = $@"{(check.TypeSell==TypeSell.Sell ? "Покупка" : "Возврат")} {check.DateCreate.ToString("dd.MM.yy HH:mm")} на сумму {check.CheckPayments.Sum(p=>p.Sum).ToSellFormat()}
Оплаты
@payments

Чек
@@@";
            string paymentsStr = "";
            paymentsStr += check.SumElectron > 0 ? " Безнал: " + check.SumElectron : "";
            paymentsStr += check.SumCash > 0 ? " Нал: " + check.SumCash : "";
            document = document.Replace("@payments", paymentsStr);
            richTextBox1.Text = document;

            string[] cells = new string[5] { "Наименование", "Цена", "Кол-во", "Акция", "Сумма" };
            var checkGoods = check.CheckGoods.ToList();
            RtfTable tableCheckGoods = new RtfTable(check.CheckGoods.Count() + 1, cells.Length, 120);
            tableCheckGoods.SetColumnWidths(2500, 720, 950, 950, 1100);
            for (int r = 0; r < check.CheckGoods.Count() + 1; r++)
                for (int c = 0; c < cells.Length; c++)
                    if (r == 0)
                        tableCheckGoods.Contents[r, c] = cells[c];
                    else
                        switch (c)
                        {
                            case 0:
                                tableCheckGoods.Contents[r, c] = checkGoods[r-1].Good?.Name;
                                break;
                            case 1:
                                tableCheckGoods.Contents[r, c] = checkGoods[r-1].Cost.ToSellFormat();
                                break;
                            case 2:
                                tableCheckGoods.Contents[r, c] = checkGoods[r - 1].Count.ToString();
                                break;
                            case 3:
                                var promotionQuantity = checkGoods[r - 1].PromotionQuantity == 0 ? 
                                    "" : checkGoods[r - 1].PromotionQuantity.ToString();
                                tableCheckGoods.Contents[r, c] = promotionQuantity;
                                break;
                            case 4:
                                tableCheckGoods.Contents[r, c] = checkGoods[r - 1].Sum.ToSellFormat();
                                break;
                        };
            richTextBox1.Rtf=richTextBox1.Rtf.Replace("@@@", tableCheckGoods.ToString());
            
        }

        private async void RenderCashMoney(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            CashMoney cashMoney = await _db.CashMoneys.Where(m => m.Id == docId)
                .AsNoTracking().FirstAsync();
            richTextBox1.Text = @$"{cashMoney.TypeOperation.GetDisplay()} от {cashMoney.Create.ToString("dd.MM.yy")}
На сумму {cashMoney.Sum.ToSellFormat()}
{cashMoney.Note}";
        }

        private async void RenderRevaluation(int docId)
        {
            using var _db = _dbFactory.CreateDbContext();
            var revaluation = await _db.Revaluations
                .Include(r => r.RevaluationGoods).ThenInclude(r => r.Good)
                .Where(r => r.Id == docId)
                .AsNoTracking().FirstAsync();
            richTextBox1.Text = $@"Переоценка товара от {revaluation.Create.ToString("dd.MM.yy")}

@@@";
            string[] cells = new string[7] { "Наименование", "Ед", "Кол-во", "Цена", "Сумма", "Цена", "Сумма" };
            RtfTable table = new RtfTable(revaluation.RevaluationGoods.Count + 1, cells.Length, 120);
            table.SetColumnWidths(800, 720, 750, 750, 750, 750, 750);
            var revaluationGoods = revaluation.RevaluationGoods.ToList();
            for (int r = 0; r < revaluation.RevaluationGoods.Count + 1; r++)
                for (int c = 0; c < cells.Length; c++)
                    if (r == 0)
                        table.Contents[r, c] = cells[c];
                    else
                        switch (c)
                        {
                            case 0:
                                table.Contents[r, c] = revaluationGoods[r - 1].Good.Name;
                                break;
                            case 1:
                                table.Contents[r, c] = revaluationGoods[r - 1].Good.Unit.GetDisplay();
                                break;
                            case 2:
                                table.Contents[r, c] = revaluationGoods[r - 1].Count?.ToString();
                                break;
                            case 3:
                                table.Contents[r, c] = revaluationGoods[r - 1].PriceOld.ToSellFormat();
                                break;
                            case 4:
                                table.Contents[r, c] = revaluationGoods[r - 1].SumOld.ToSellFormat();
                                break;
                            case 5:
                                table.Contents[r, c] = revaluationGoods[r - 1].PriceNew.ToSellFormat();
                                break;
                            case 6:
                                table.Contents[r, c] = revaluationGoods[r - 1].SumNew.ToSellFormat();
                                break;
                        };
            richTextBox1.Rtf = richTextBox1.Rtf.Replace("@@@", table.ToString());
        }
    }

    public class RtfTable
    {
        public int InternalMargin = 180;
        public int NumRows, NumCols;
        public int[] ColumnWidths = null;
        public string[,] Contents = null;

        public RtfTable(int num_rows, int num_cols, int internal_margin)
        {
            NumRows = num_rows;
            NumCols = num_cols;
            InternalMargin = internal_margin;
            ColumnWidths = Enumerable.Repeat(1440, NumCols).ToArray();

            Contents = new string[NumRows, NumCols];
            for (int r = 0; r < NumRows; r++)
                for (int c = 0; c < NumCols; c++)
                    Contents[r, c] = "";
        }

        public void SetColumnWidths(params int[] widths)
        {
            for (int c = 0; c < NumCols; c++)
                ColumnWidths[c] = widths[c];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            string column_widths_string = ColumnWidthsString();

            for (int r = 0; r < NumRows; r++)
            {
                // Start the row.
                sb.Append(@"\trowd");
                sb.Append(@"\trgaph" + InternalMargin.ToString());

                // Column widths.
                sb.Append(column_widths_string);

                // Column contents.
                for (int c = 0; c < NumCols; c++)
                {
                    sb.Append(@"\pard\intbl{" +
                        Contents[r, c]?.Replace(@"\", @"\\") +
                        @"}\cell");
                }

                // End the row.
                sb.Append(@"\row");
            }
            return sb.ToString();
        }

        private string ColumnWidthsString()
        {
            StringBuilder sb = new StringBuilder();
            int total = 0;
            for (int c = 0; c < NumCols; c++)
            {
                total += ColumnWidths[c];
                sb.Append(@"\cellx" + total.ToString());
            }
            return sb.ToString();
        }
    }
}
