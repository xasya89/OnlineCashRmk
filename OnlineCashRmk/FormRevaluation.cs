using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineCashRmk.Models;
using OnlineCashRmk.Services;
using OnlineCashTransportModels.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk
{
    public partial class FormRevaluation : Form
    {
        private readonly IDbContextFactory<DataContext> _dbContextFactory;
        private readonly BindingList<RevaluationPosition> _positions;
        private readonly IServiceProvider _servicePrivider;
        public FormRevaluation(IDbContextFactory<DataContext> dbContextFactory, IServiceProvider serviceProvider)
        {
            _dbContextFactory = dbContextFactory;
            _positions = new BindingList<RevaluationPosition>();
            _servicePrivider = serviceProvider;
            InitializeComponent();
            var searchControll = new SearchGoodsControll(_dbContextFactory);
            searchControll.ProductSelected += (g) =>
            {
                if (g == null) return;
                if (_positions.Where(x => x.GoodId == g.Id).Any())
                    return;
                _positions.Add(new RevaluationPosition
                {
                    GoodId = g.Id,
                    GoodName = g.Name,
                    Unit = g.Unit,
                    PriceOld = g.Price,
                    PriceNew = 0,
                    Quantity = null
                });
            };
            searchControll.Dock = DockStyle.Fill;
            searchPanel.Controls.Add(searchControll);
            SetupUI();

            revaluationDataGridView.CellParsing += revaluationDataGridView_CellParsing;
        }

        private void SetupUI()
        {
            revaluationDataGridView.AutoGenerateColumns = false;
            // Настройка колонок
            revaluationDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.GoodName),
                DataPropertyName = nameof(RevaluationPosition.GoodName),
                HeaderText = "Товар",
                ReadOnly = true,
                Width = 300
            });

            revaluationDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.UnitStr),
                DataPropertyName = nameof(RevaluationPosition.UnitStr),
                HeaderText = "Ед.",
                ReadOnly = true,
                Width = 60
            });

            revaluationDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.PriceOld),
                DataPropertyName = nameof(RevaluationPosition.PriceOld),
                HeaderText = "Старая цена",
                ReadOnly = true, // ← запрещено редактировать
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            revaluationDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.PriceNew),
                DataPropertyName = nameof(RevaluationPosition.PriceNew),
                HeaderText = "Новая цена",
                ReadOnly = false, // ← запрещено редактировать
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            var quantityColumn = new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.Quantity),
                DataPropertyName = nameof(RevaluationPosition.Quantity),
                HeaderText = "Количество",
                ReadOnly = false,
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N3" }
            };


            quantityColumn.DefaultCellStyle.BackColor = Color.LightGreen; // или любой цвет
            quantityColumn.DefaultCellStyle.SelectionBackColor = Color.Green;
            revaluationDataGridView.Columns.Add(quantityColumn);

            revaluationDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.AmountOld),
                DataPropertyName = nameof(RevaluationPosition.AmountOld),
                HeaderText = "Сумма было",
                ReadOnly = true, // ← запрещено редактировать
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            revaluationDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = nameof(RevaluationPosition.AmountNew),
                DataPropertyName = nameof(RevaluationPosition.AmountNew),
                HeaderText = "Сумма стало",
                ReadOnly = true, // ← запрещено редактировать
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            revaluationDataGridView.DataSource = _positions;
            //revaluationDataGridView.def
            //dgv.CellValueChanged += OnCellValueChanged;
            //dgv.KeyDown += OnDgvKeyDown;

            _positions.ListChanged += (s, e) =>
            {
                string _amountOld = _positions.Sum(x => x.AmountOld).ToString();
                string _amountNew = _positions.Sum(x => x.AmountNew).ToString();
                toolStripLabelAmount.Text = $"Итого: было {_amountOld} стало {_amountNew}";
            };
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var dgv = (DataGridView)sender;
            if (dgv.Columns[e.ColumnIndex].Name == nameof(RevaluationPosition.Quantity))
            {
                if (TryParseDecimal(e.FormattedValue?.ToString(), out var quantity))
                {
                    // Нормализуем значение (сохраняем как decimal)
                    _positions[e.RowIndex].Quantity = quantity;
                }
                else
                {
                    // Отменяем ввод
                    e.Cancel = true;
                }
            }
        }
        private void revaluationDataGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (revaluationDataGridView.Columns[e.ColumnIndex].Name == nameof(RevaluationPosition.Quantity))
            {
                if (e.Value is string input && !string.IsNullOrWhiteSpace(input))
                {
                    // Нормализуем: точка → запятая
                    if (input.Contains('.') && !input.Contains(','))
                    {
                        input = input.Replace('.', ',');
                    }

                    if (decimal.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out var result))
                    {
                        e.Value = result;
                        e.ParsingApplied = true; // ← КЛЮЧЕВОЙ момент!
                        return;
                    }
                }
                if (e.Value is string input1 && string.IsNullOrWhiteSpace(input1))
                {
                    e.Value = null;
                    e.ParsingApplied = true; // ← КЛЮЧЕВОЙ момент!
                    return;
                }
                // Если не удалось распарсить — отменяем
                e.ParsingApplied = false;
            }
        }

        private bool TryParseDecimal(string input, out decimal result)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                result = 0;
                return false;
            }

            // Удаляем пробелы
            input = input.Trim();

            // Заменяем точку на запятую (только если одна точка и нет запятых)
            if (input.Contains('.') && !input.Contains(','))
            {
                input = input.Replace('.', ',');
            }

            // Теперь парсим с учётом текущей культуры
            return decimal.TryParse(input, NumberStyles.Float, CultureInfo.CurrentCulture, out result);
        }

        private class RevaluationPosition : INotifyPropertyChanged
        {
            public int GoodId { get; set; }
            public string GoodName { get; set; }
            public Units Unit { get; set; }
            public string UnitStr => Unit.GetDisplay();
            private decimal _priceOld;
            public decimal PriceOld
            {
                get { return _priceOld; }
                set
                {
                    _priceOld = value;
                    OnPropertyChanged(nameof(PriceOld));
                    OnPropertyChanged(nameof(AmountOld));
                    OnPropertyChanged(nameof(AmountNew));
                }
            }
            private decimal _priceNew;
            public decimal PriceNew
            {
                get { return _priceNew; }
                set
                {
                    _priceNew = value;
                    OnPropertyChanged(nameof(PriceNew));
                    OnPropertyChanged(nameof(AmountOld));
                    OnPropertyChanged(nameof(AmountNew));
                }
            }
            private decimal? _quantity;
            public decimal? Quantity
            {
                get { return _quantity; }
                set
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(AmountOld));
                    OnPropertyChanged(nameof(AmountNew));
                }
            }
            public decimal? AmountOld => Quantity.HasValue ? Quantity.Value * PriceOld : 0;
            public decimal AmountNew => Quantity.HasValue ? Quantity.Value * PriceNew : 0;

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Обноим цены
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void toolStripSynchGoods_Click(object sender, EventArgs e)
        {
            using var scope = _servicePrivider.CreateScope();
            var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
            using var db = dbContextFactory.CreateDbContext();
            var synchService = scope.ServiceProvider.GetRequiredService<ISynchService>();
            try
            {
                await synchService.SynchGoods();
                var ids = _positions.Select(x => x.GoodId);
                var goods = await db.Goods.Where(x => ids.Contains(x.Id))
                    .AsNoTracking().ToArrayAsync();
                foreach (var p in _positions)
                    p.PriceNew = goods.Where(x => x.Id == p.GoodId).First().Price;
                _positions.ResetBindings();
                toolStripSynchGoods.BackColor = Color.LightGreen;
            }
            catch (Exception ex)
            {
                toolStripSynchGoods.BackColor = Color.LightPink;
            }
        }

        private async void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (_positions.Where(x => !x.Quantity.HasValue).Any())
            {
                MessageBox.Show("Есть строки, где не заполнено количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using var db = _dbContextFactory.CreateDbContext();
                var ids = _positions.Select(x => x.GoodId);
                var goods = await db.Goods.Where(x => ids.Contains(x.Id)).AsNoTracking().ToArrayAsync();
                var revaluation = new Revaluation
                {
                    Create = DateTime.Now,
                    RevaluationGoods = _positions.Select(x =>
                    {
                        var good = goods.Where(g => x.GoodId == g.Id).First();
                        return new RevaluationGood
                        {
                            GoodId = good.Id,
                            Count = x.Quantity,
                            PriceOld = x.PriceOld,
                            PriceNew = x.PriceNew,
                        };
                    }).ToArray()
                };
                db.Revaluations.Add(revaluation);
                await db.SaveChangesAsync();
                var doc = new DocSynch { TypeDoc = TypeDocs.Revaluation, DocId = revaluation.Id };
                db.DocSynches.Add(doc);
                await db.SaveChangesAsync();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolButtonDeleteRow_Click(object sender, EventArgs e)
        {
            if (revaluationDataGridView.CurrentCell is DataGridViewCell cell)
                _positions.RemoveAt(cell.RowIndex);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
