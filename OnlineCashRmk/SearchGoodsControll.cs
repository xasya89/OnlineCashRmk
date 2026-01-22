using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineCashRmk
{
    public partial class SearchGoodsControll : UserControl
    {
        private IDbContextFactory<DataContext> _dbContextFactory;
        private CancellationTokenSource? _searchCts;
        private System.Threading.Timer? _debounceTimer;

        public event Action<Good?>? ProductSelected;
        public SearchGoodsControll()
        {
            InitializeComponent();
        }

        public SearchGoodsControll(IDbContextFactory<DataContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            InitializeComponent();
            this.Disposed += (s, e) =>
            {
                _debounceTimer?.Dispose();
                _searchCts?.Dispose();
            };
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Отменяем предыдущий поиск
            _searchCts?.Cancel();

            // Сбрасываем таймер debounce
            _debounceTimer?.Dispose();
            _debounceTimer = new System.Threading.Timer(async _ =>
            {
                var query = ((TextBox)sender!).Text.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(query)) return;

                await SearchProductsAsync(query);
            }, null, 300, Timeout.Infinite); // 300 мс debounce
        }

        private async Task SearchProductsAsync(string query)
        {
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;
            try
            {
                using var context = _dbContextFactory.CreateDbContext();

                var products = await context.Goods
                    .Where(p => p.NameLower.Contains(query) & !p.IsDeleted)
                    .OrderBy(p => p.Name)
                    .Take(20).AsNoTracking().ToListAsync(token);

                if (!token.IsCancellationRequested)
                {
                    // Обновляем UI только если не отменено
                    goodsListBox.Invoke((MethodInvoker)delegate
                    {
                        goodsListBox.DataSource = null;
                        goodsListBox.DisplayMember = nameof(Good.Name);
                        goodsListBox.DataSource = products;
                    });
                }
            }
            catch (OperationCanceledException)
            {
                // Игнорируем отмену
            }
            catch (Exception)
            {
            }
        }

        private void searchTextBox_DoubleClick(object sender, EventArgs e)
        {

        }

        private async void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            bool isBarCode = int.TryParse(searchTextBox.Text, out int code);
            if (goodsListBox.Items.Count == 0 & !isBarCode) return;

            int currentIndex = goodsListBox.SelectedIndex;

            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (currentIndex < goodsListBox.Items.Count - 1)
                        goodsListBox.SelectedIndex = currentIndex + 1;
                    else
                        goodsListBox.SelectedIndex = 0; // зацикливание (опционально)
                    e.Handled = true;
                    break;

                case Keys.Up:
                    if (currentIndex > 0)
                        goodsListBox.SelectedIndex = currentIndex - 1;
                    else
                        goodsListBox.SelectedIndex = goodsListBox.Items.Count - 1; // зацикливание
                    e.Handled = true;
                    break;

                case Keys.Enter:
                    Good _good=null;
                    if (isBarCode)
                    {
                        using var db = _dbContextFactory.CreateDbContext();
                        var barcode = await db.BarCodes.Include(x => x.Good)
                            .Where(x => x.Code == code.ToString())
                            .AsNoTracking().FirstOrDefaultAsync();
                        _good = barcode?.Good;
                    }
                    if (_good==null && goodsListBox.SelectedItem is Good good)
                        _good = good;
                    SelectProduct(_good);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;

                case Keys.Escape:
                case Keys.Back:
                    Clear();
                    e.Handled = true;
                    break;
            }
        }

        private void SelectProduct(Good? good)
        {
            ProductSelected?.Invoke(good);
            Clear();
        }

        public void Clear()
        {
            searchTextBox.Clear();
            goodsListBox.DataSource = null;
            searchTextBox.Focus();
        }

        private void goodsListBox_DoubleClick(object sender, EventArgs e)
        {
            if (goodsListBox.SelectedItem is Good good)
                SelectProduct(good);
        }

        private void goodsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && goodsListBox.SelectedItem is Good good)
                SelectProduct(good);
        }
    }
}
