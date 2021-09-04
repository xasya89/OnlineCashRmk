using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Microsoft.EntityFrameworkCore;
using OnlineCashRmk.Models;

namespace OnlineCashRmk
{
    public partial class FormMenu : Form
    {
        IConfiguration configuration;
        string serverName = "";
        int idShop = 1;
        string cashierName = "";
        string cashierInn = "";
        public FormMenu()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);
            configuration = builder.Build();
            serverName = configuration.GetSection("serverName").Value;
            idShop = Convert.ToInt32(configuration.GetSection("idShop").Value);
            cashierName = configuration.GetSection("cashierName").Value;
            cashierInn = configuration.GetSection("cashierInn").Value;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Загрузить товары
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.LightBlue;
            Task.Run(async () =>
            {
                bool statusSuccess = true;
                try
                {
                    DataContext db = new DataContext();
                    var str = await new HttpClient().GetAsync($"{serverName}/api/Goodssynchnew/{idShop}").Result.Content.ReadAsStringAsync();
                    List<Good> goods = JsonSerializer.Deserialize<List<Good>>(str);
                    foreach (var good in goods)
                    {
                        var goodDb = db.Goods.Include(g=>g.BarCodes).Where(g => g.Uuid == good.Uuid).FirstOrDefault();
                        if (goodDb == null)
                        {
                            var newgood = new Good
                            {
                                Uuid = good.Uuid,
                                Name = good.Name,
                                Article = good.Name,
                                BarCode = good.BarCode,
                                Unit = good.Unit,
                                Price = good.Price,
                                SpecialType=good.SpecialType,
                                VPackage=good.VPackage
                            };
                            db.Goods.Add(newgood);
                            //добавление штрих кодов
                            foreach (var barcode in good.BarCodes)
                                db.BarCodes.Add(new BarCode
                                {
                                    Good = newgood,
                                    Code = barcode.Code
                                });
                        }
                        else
                        {
                            goodDb.Name = good.Name;
                            goodDb.Unit = good.Unit;
                            goodDb.BarCode = good.BarCode;
                            goodDb.Price = good.Price;
                            goodDb.SpecialType = good.SpecialType;
                            goodDb.VPackage = good.VPackage;
                            //добавление новых или измененных штрих кодов
                            foreach (var barcode in good.BarCodes)
                                if (goodDb.BarCodes.Count(b => b.Code == barcode.Code) == 0)
                                    db.BarCodes.Add(new BarCode { Good = goodDb, Code = barcode.Code });
                            //Удаление не зарегестрированных на сервере штрихкодов
                            foreach (var barcodeDb in goodDb.BarCodes)
                                if (good.BarCodes.Count(b => b.Code == barcodeDb.Code) == 0)
                                    db.BarCodes.Remove(barcodeDb);
                        }
                    };
                    await db.SaveChangesAsync();
                }
                catch (SystemException ex)
                {
                    statusSuccess = false;
                }
                var action = new Action(() =>
                  {
                      button1.BackColor = statusSuccess ? Color.LightGreen : Color.LightPink;
                  });
                Invoke(action);
            });
        }
        //Приходы
        private void button3_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start($"{serverName}/Arrivals/Create?ShopId={idShop}");
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId;
            BrowserApplication browser;
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey == null)
                {
                    browser = BrowserApplication.Unknown;
                }
                object progIdValue = userChoiceKey.GetValue("Progid");
                if (progIdValue == null)
                {
                    browser = BrowserApplication.Unknown;
                }
                progId = progIdValue.ToString();
                switch (progId)
                {
                    case "IE.HTTP":
                        browser = BrowserApplication.InternetExplorer;
                        break;
                    case "FirefoxURL":
                        browser = BrowserApplication.Firefox;
                        break;
                    case "ChromeHTML":
                        browser = BrowserApplication.Chrome;
                        break;
                    case "OperaStable":
                        browser = BrowserApplication.Opera;
                        break;
                    case "SafariHTML":
                        browser = BrowserApplication.Safari;
                        break;
                    case "AppXq0fevzme2pys62n3e0fbqa7peapykr8v":
                        browser = BrowserApplication.Edge;
                        break;
                    default:
                        browser = BrowserApplication.Unknown;
                        break;
                }
                if (progId.ToLower().IndexOf("firefox") > -1)
                    browser = BrowserApplication.Firefox;
            }
            //System.Diagnostics.Process.Start("explorer.exe", $"{serverName}/Arrivals/Create?ShopId={idShop}");
            System.Diagnostics.Process.Start(new ProcessStartInfo($"{serverName}/Arrivals/Create?ShopId={idShop}") { UseShellExecute = true } );
            Close();
        }

        //Инверторизация
        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo($"{serverName}/Stocktaking/Create?idShop={idShop}") { UseShellExecute = true });
            Close();
        }

        //Кредиты
        private void button5_Click(object sender, EventArgs e)
        {
            FormCreditList fr = new FormCreditList();
            //fr.TopMost = true;
            fr.Show();
            Close();
        }
    }
}
